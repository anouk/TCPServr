using System.Text;
using System;

namespace TCPServr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Server ThisServer;
        private void Form1_Load(object sender, EventArgs e)
        {//16384
            ThisServer = new Server(16384);
            ThisServer.OnDataReceived += ThhisServer_OnDataReceived;
            ThisServer.DigitalRecieved += ThisServer_DigitalRecieved;
            ThisServer.AnalogRecieved += ThisServer_AnalogRecieved;
            ThisServer.SerialRecieved += ThisServer_SerialRecieved;            
            ThisServer.Start();
        }

        private void ThisServer_SerialRecieved(int JoinNumber, string Data)
        {
           
        }

        private void ThisServer_AnalogRecieved(int JoinNumber, int Data)
        {
        }

        private void ThisServer_DigitalRecieved(int JoinNumber, bool Data)
        {
        
        }

        private void ThhisServer_OnDataReceived(byte[] data, int bytesRead, System.Net.Sockets.TcpClient client)
        {

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThisServer.SendAnalogFeedback(Convert.ToInt32(txAnaJoin.Text), Convert.ToInt32(txanaValue.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ThisServer.SendDigitalFeedback(Convert.ToInt32(txDigJoin.Text), true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ThisServer.SendSerialFeedback(Convert.ToInt32(txSerialJoin.Text), txSerialValue.Text);
        }
    }
}