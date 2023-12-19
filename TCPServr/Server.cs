using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

namespace TCPServr
{
    public delegate void ServerHandlePacketData(byte[] data, int bytesRead, TcpClient client);
    public delegate void AnalogJoinRecieved(int JoinNumber, int Data);
    public delegate void DigitalJoinRecieved(int JoinNumber, bool Data);
    public delegate void SerialJoinRecieved(int JoinNumber, string Data);
    /// <summary>
    /// Implements a simple TCP server which uses one thread per client
    /// </summary>
    /// 
    public class NetworkBuffer
    {
        public byte[] WriteBuffer;
        public byte[] ReadBuffer;
        public int CurrentWriteByteCount;
    }

  

    public class Server
    {
        public event ServerHandlePacketData OnDataReceived;
        public event AnalogJoinRecieved AnalogRecieved;
        public event DigitalJoinRecieved DigitalRecieved;
        public event SerialJoinRecieved SerialRecieved;

        private TcpListener listener;
        private ConcurrentDictionary<TcpClient, NetworkBuffer> clientBuffers;
        private List<TcpClient> clients;
        private int sendBufferSize = 1024;
        private int readBufferSize = 1024;
        private int port;
        private bool started = false;

        protected virtual void OnDigitalRecieved(int Join, bool Value) //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            DigitalRecieved?.Invoke(Join, Value);
        }
        protected virtual void OnAnalogRecieved(int Join, int Value) //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            AnalogRecieved?.Invoke(Join, Value);
        }
        protected virtual void OnSerialRecieved(int Join, string Value) //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            SerialRecieved?.Invoke(Join, Value);
        }
        /// <summary>
        /// The list of currently connected clients
        /// </summary>
        public List<TcpClient> Clients
        {
            get
            {
                return clients;
            }
        }

        /// <summary>
        /// The number of clients currently connected
        /// </summary>
        public int NumClients
        {
            get
            {
                return clients.Count;
            }
        }

        /// <summary>
        /// Constructs a new TCP server which will listen on a given port
        /// </summary>
        /// <param name="port"></param>
        public Server(int port)
        {
            this.port = port;
            clientBuffers = new ConcurrentDictionary<TcpClient, NetworkBuffer>();
            clients = new List<TcpClient>();
        }

        /// <summary>
        /// Begins listening on the port provided to the constructor
        /// </summary>
        public void Start()
        {
            listener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), port);
            Console.WriteLine("Started server on port " + port);

            Thread thread = new Thread(new ThreadStart(ListenForClients));
            thread.Start();
            started = true;
        }

        /// <summary>
        /// Runs in its own thread. Responsible for accepting new clients and kicking them off into their own thread
        /// </summary>
        private void ListenForClients()
        {
            listener.Start();

            while (started)
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(WorkWithClient));
                Console.WriteLine("New client connected");

                NetworkBuffer newBuff = new NetworkBuffer();
                newBuff.WriteBuffer = new byte[sendBufferSize];
                newBuff.ReadBuffer = new byte[readBufferSize];
                newBuff.CurrentWriteByteCount = 0;
                clientBuffers.GetOrAdd(client, newBuff);
                clients.Add(client);

                clientThread.Start(client);
                Thread.Sleep(15);
            }
        }

        /// <summary>
        /// Stops the server from accepting new clients
        /// </summary>
        public void Stop()
        {
            if (!listener.Pending())
            {
                listener.Stop();
                started = false;
            }
        }

        /// <summary>
        /// This method lives on a thread, one per client. Responsible for reading data from the client
        /// and pushing the data off to classes listening to the server.
        /// </summary>
        /// <param name="client"></param>
        private void WorkWithClient(object client)
        {
            TcpClient tcpClient = client as TcpClient;
            if (tcpClient == null)
            {
                Console.WriteLine("TCP client is null, stopping processing for this client");
                DisconnectClient(tcpClient);
                return;
            }

            NetworkStream clientStream = tcpClient.GetStream();
            int bytesRead;

            while (started)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(clientBuffers[tcpClient].ReadBuffer, 0, readBufferSize);
                }
                catch
                {
                    //a socket error has occurred
                    Console.WriteLine("A socket error has occurred with client: " + tcpClient.ToString());
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                if (OnDataReceived != null)
                {
                    //Send off the data for other classes to handle

                    string[] IncommingData = BitConverter.ToString(clientBuffers[tcpClient].ReadBuffer).Substring(0, ((bytesRead * 2) + bytesRead) - 1).Split('-'); ;
                    if (IncommingData.Length == 1)
                        continue;
                    int Value1 = (Convert.ToInt32(IncommingData[0], 16));
                    int JoinNumber = (Value1 & 0b00000111) << 7 | (Convert.ToInt32(IncommingData[1], 16)) + 1;
                    bool isAnalog = false;
                    bool isDigital = false;
                    bool isSerial = false;

                    if (IncommingData.Length == 2) isDigital = true;
                    if (IncommingData.Length == 4) isAnalog = true;
                    if (IncommingData.Length > 4) isSerial = true;
                    if (isAnalog)
                    {  //value 2 is join number 
                        int Value3 = (Convert.ToInt32(IncommingData[2], 16));
                        int Value4 = (Convert.ToInt32(IncommingData[3], 16));
                        OnAnalogRecieved(JoinNumber, (Value1 & 0b00110000) << 10 | Value3 << 7 | Value4);
                    }
                    if (isDigital)
                    {  //value 2 is join number 
                       //value 2 is join number 
                        int value = Convert.ToInt32(IncommingData[0], 16) >> 5 & 0b1;
                        bool ReturnValue = false;
                        if (value == 0) ReturnValue = true;
                        OnDigitalRecieved(JoinNumber, ReturnValue);
                    }
                    if (isSerial)
                    {  //value 2 is join number 
                        int x = 0;
                        string HexString = "";

                        foreach (string aString in IncommingData)
                        {
                            if (x > 0)
                            {
                                HexString = HexString + aString;
                            }
                            x = x = 1;
                        }



                        //    string woot = ByteArrayToString(bytesRead);
                        OnSerialRecieved(JoinNumber, Encoding.ASCII.GetString(FromHex(HexString)).Substring(1).TrimEnd('?'));
                    }


                    OnDataReceived(clientBuffers[tcpClient].ReadBuffer, bytesRead, tcpClient);
                }

                Thread.Sleep(15);
            }

            DisconnectClient(tcpClient);
        }

        /// <summary>
        /// Removes a given client from our list of clients
        /// </summary>
        /// <param name="client"></param>
        private void DisconnectClient(TcpClient client)
        {
            if (client == null)
            {
                return;
            }

            Console.WriteLine("Disconnected client: " + client.ToString());

            client.Close();

            clients.Remove(client);
            NetworkBuffer buffer;
            clientBuffers.TryRemove(client, out buffer);
        }

        /// <summary>
        /// Adds data to the packet to be sent out, but does not send it across the network
        /// </summary>
        /// <param name="data">The data to be sent</param>
        /// <param name="client">The client to send the data to</param>
        public void AddToPacket(byte[] data, TcpClient client)
        {
            try
            {
                if (clientBuffers[client].CurrentWriteByteCount + data.Length > clientBuffers[client].WriteBuffer.Length)
                {
                    FlushData(client);
                }

                Array.ConstrainedCopy(data, 0, clientBuffers[client].WriteBuffer, clientBuffers[client].CurrentWriteByteCount, data.Length);

                clientBuffers[client].CurrentWriteByteCount += data.Length;
            }
            catch 
            
            { 
            
            
            }   
        }

        /// <summary>
        /// Adds data to the packet to be sent out, but does not send it across the network. This
        /// data gets sent to every connected client
        /// </summary>
        /// <param name="data">The data to be sent</param>
        public void AddToPacketToAll(byte[] data)
        {
            lock (clients)
            {
                foreach (TcpClient client in clients)
                {
                    if (clientBuffers[client].CurrentWriteByteCount + data.Length > clientBuffers[client].WriteBuffer.Length)
                    {
                        FlushData(client);
                    }

                    Array.ConstrainedCopy(data, 0, clientBuffers[client].WriteBuffer, clientBuffers[client].CurrentWriteByteCount, data.Length);

                    clientBuffers[client].CurrentWriteByteCount += data.Length;
                }
            }
        }

        /// <summary>
        /// Flushes all outgoing data to the specified client
        /// </summary>
        /// <param name="client"></param>
        private void FlushData(TcpClient client)
        {
            client.GetStream().Write(clientBuffers[client].WriteBuffer, 0, clientBuffers[client].CurrentWriteByteCount);
            client.GetStream().Flush();
            clientBuffers[client].CurrentWriteByteCount = 0;
        }

        /// <summary>
        /// Flushes all outgoing data to every client
        /// </summary>
        private void FlushDataToAll()
        {
            lock (clients)
            {
                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(clientBuffers[client].WriteBuffer, 0, clientBuffers[client].CurrentWriteByteCount);
                    client.GetStream().Flush();
                    clientBuffers[client].CurrentWriteByteCount = 0;
                }
            }
        }
        public void SendAnalogFeedback(int JoinNumber, int Value)
        {
            byte[] byteData = new byte[4];
            byteData[0] = Convert.ToByte(0b11000000 | ((Value >> 10) & 0b00110000) | ((JoinNumber - 1) >> 7));
            byteData[1] = Convert.ToByte((JoinNumber - 1) & 0b01111111);
            byteData[2] = Convert.ToByte(Value >> 7 & 0b01111111);
            byteData[3] = Convert.ToByte(Value & 0b01111111);

            SendImmediateToAll(byteData);
        }
        static byte[] ConvertHexToByteArray(string hexString)
        {
            int length = hexString.Length;
            byte[] byteArray = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return byteArray;
        }
        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        public void SendDigitalFeedback(int JoinNumber, bool Value)
        {
            byte[] byteData = new byte[2];
            byteData[0] = Convert.ToByte(0b10000000 | (~Convert.ToInt32(Value) << 5 & 0b00100000) | (JoinNumber - 1) >> 7);

            byteData[1] = Convert.ToByte((JoinNumber - 1) & 0b01111111);

            SendImmediateToAll(byteData);


        }
        public void SendSerialFeedback(int JoinNumber, string Value)
        {

            byte[] Stringbytes = Encoding.ASCII.GetBytes(Value);
            byte[] byteData = new byte[Stringbytes.Length + 3];
            byteData[0] = Convert.ToByte(0b11001000 | ((JoinNumber - 1) >> 7));
            byteData[1] = Convert.ToByte((JoinNumber - 1) & 0b01111111);
            for (int i = 2; i < byteData.Length - 1; i++)
            {
                byteData[i] = Stringbytes[i - 2];
            }

            byteData[byteData.Length - 1] = 255;

            SendImmediateToAll(byteData);


        }
        public static string PaddHex(string HexValue)
        {
            if (HexValue.Length == 1)
            {
                HexValue = "0" + HexValue;
            }
            return HexValue;
        }
        /// <summary>
        /// Sends the byte array data immediately to the specified client
        /// </summary>
        /// <param name="data">The data to be sent</param>
        /// <param name="client">The client to send the data to</param>
        public void SendImmediate(byte[] data, TcpClient client)
        {
            if (client == null) return;
            if (data == null) return;
            AddToPacket(data, client);
            FlushData(client);
        }

        /// <summary>
        /// Sends the byte array data immediately to all clients
        /// </summary>
        /// <param name="data">The data to be sent</param>
        public void SendImmediateToAll(byte[] data)
        {
            lock (clients)
            {
                foreach (TcpClient client in clients)
                {
                    AddToPacket(data, client);
                    FlushData(client);
                }
            }
        }
    }
}