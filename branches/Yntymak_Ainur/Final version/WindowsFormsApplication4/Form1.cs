using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public StreamReader str, str2;
        public StreamWriter stw, stw2;
        public String receive;
        public String text_to_send;
        public String file_name;
        public OpenFileDialog op;
        public bool file = false;
        public String file_to_send;
        public Byte[] b1;

        public Form1()
        {
            InitializeComponent();

            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP) 
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox3.Text = address.ToString();
                }

            }
        }

        private void button2_Click(object sender, EventArgs e) //Start Server
        {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(textBox4.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            str = new StreamReader(client.GetStream());
            stw = new StreamWriter(client.GetStream());
            stw.AutoFlush = true;

            textBox2.AppendText("Client connected");

            backgroundWorker1.RunWorkerAsync();                   
            backgroundWorker2.WorkerSupportsCancellation = true;

            backgroundWorker4.RunWorkerAsync();
            backgroundWorker3.WorkerSupportsCancellation = true; 

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) //receive data
        {
            while (client.Connected)
            {
                try
                {
                    receive = str.ReadLine();
                    this.textBox2.Invoke(
                        new MethodInvoker(delegate() {textBox2.AppendText(DateTime.Now.ToString("HH:mm:ss tt")+" You: " + receive + "\n");})
                        );
                    receive = "";

                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e) //send data
        {
            if (client.Connected)
            {
                stw.WriteLine(text_to_send);
                this.textBox2.Invoke(
                        new MethodInvoker(delegate() { textBox2.AppendText(DateTime.Now.ToString("HH:mm:ss tt")+ " Me: " + text_to_send + "\n"); })
                    );
            }
            else
            {
                MessageBox.Show("Send failed!");
            }
            backgroundWorker2.CancelAsync();
        }

        private void button3_Click(object sender, EventArgs e) //Connect to Server
        {
            client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(textBox5.Text), int.Parse(textBox6.Text));

            try
            {
                client.Connect(IP_End);
                if (client.Connected) 
                {
                    textBox2.AppendText("Connected to Server\n");
                    stw = new StreamWriter(client.GetStream());
                    str = new StreamReader(client.GetStream());
                    stw2 = new StreamWriter(client.GetStream());
                    str2 = new StreamReader(client.GetStream());
                    stw.AutoFlush = true;
                    stw2.AutoFlush = true;

                    backgroundWorker1.RunWorkerAsync();                   
                    backgroundWorker2.WorkerSupportsCancellation = true;

                    backgroundWorker4.RunWorkerAsync();
                    backgroundWorker3.WorkerSupportsCancellation = true;

                }

            }
            catch (Exception x) 
            {
                MessageBox.Show(x.Message.ToString());
            }


        }

        private void button1_Click(object sender, EventArgs e) //Send button
        {
            if(textBox1.Text != "")
            {
                    text_to_send = textBox1.Text;
                    backgroundWorker2.RunWorkerAsync();
            }

            if (textBox7.Text != "")
            {
                textBox2.AppendText(DateTime.Now.ToString("HH:mm:ss tt") + "File sending...");
            }

            textBox1.Text = "";
            textBox7.Text = "";
        }

        private void button4_Click(object sender, EventArgs e) //browse button
        {
            op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                string t = textBox7.Text;
                t = op.FileName;
                FileInfo fi = new FileInfo(textBox7.Text = op.FileName);
                file_name = fi.Name + "." + fi.Length;

                stw.WriteLine(file_name);
                stw.Flush();
            }

            backgroundWorker3.RunWorkerAsync();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e) // send file
        {
            if (client.Connected)
            {
                b1 = File.ReadAllBytes(op.FileName);

                Stream s = client.GetStream();
                s.Write(b1, 0, b1.Length);
            }
            else
            {
                MessageBox.Show("Send failed!");
            }
            backgroundWorker3.CancelAsync();
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)  // receive file
        {
            while (client.Connected)
            {
                try
                {
                    string rd;
                    byte[] b1;
                    string v;
                    int m;

                    rd = str.ReadLine();
                    v = rd.Substring(rd.LastIndexOf('.') + 1);
                    m = int.Parse(v);

                    Stream s = client.GetStream();
                    b1 = new byte[m];
                    s.Read(b1, 0, b1.Length);
                    File.WriteAllBytes("C:" + "\\" + rd.Substring(0, rd.LastIndexOf('.')), b1);

                    textBox2.AppendText("File received...");
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }
    }
}
