namespace TCPServr
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txAnaJoin = new System.Windows.Forms.TextBox();
            this.txanaValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txDigJoin = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txSerialJoin = new System.Windows.Forms.TextBox();
            this.txSerialValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txAnaJoin
            // 
            this.txAnaJoin.Location = new System.Drawing.Point(290, 103);
            this.txAnaJoin.Name = "txAnaJoin";
            this.txAnaJoin.Size = new System.Drawing.Size(100, 23);
            this.txAnaJoin.TabIndex = 1;
            // 
            // txanaValue
            // 
            this.txanaValue.Location = new System.Drawing.Point(419, 103);
            this.txanaValue.Name = "txanaValue";
            this.txanaValue.Size = new System.Drawing.Size(162, 23);
            this.txanaValue.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 102);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Send Analog";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(135, 151);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Send Digital";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txDigJoin
            // 
            this.txDigJoin.Location = new System.Drawing.Point(290, 151);
            this.txDigJoin.Name = "txDigJoin";
            this.txDigJoin.Size = new System.Drawing.Size(100, 23);
            this.txDigJoin.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(135, 202);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Send Serial";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(290, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Join Number";
            // 
            // txSerialJoin
            // 
            this.txSerialJoin.Location = new System.Drawing.Point(290, 202);
            this.txSerialJoin.Name = "txSerialJoin";
            this.txSerialJoin.Size = new System.Drawing.Size(100, 23);
            this.txSerialJoin.TabIndex = 7;
            // 
            // txSerialValue
            // 
            this.txSerialValue.Location = new System.Drawing.Point(419, 202);
            this.txSerialValue.Name = "txSerialValue";
            this.txSerialValue.Size = new System.Drawing.Size(162, 23);
            this.txSerialValue.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(480, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Value";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txSerialValue);
            this.Controls.Add(this.txSerialJoin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txDigJoin);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txanaValue);
            this.Controls.Add(this.txAnaJoin);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txAnaJoin;
        private TextBox txanaValue;
        private Button button1;
        private Button button2;
        private TextBox txDigJoin;
        private Button button3;
        private Label label1;
        private TextBox txSerialJoin;
        private TextBox txSerialValue;
        private Label label2;
    }
}