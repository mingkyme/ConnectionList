using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConnectionList_Client
{
    public partial class MainWindow : Window
    {
        readonly int PORT = 9999;
        TcpClient client;
        NetworkStream stream = default(NetworkStream);

        public MainWindow()
        {
            InitializeComponent();
            client = new TcpClient();
            client.Connect("127.0.0.1", PORT);
            stream = client.GetStream();
            string nickName = "Mingky";
            byte[] nickNameBytes = Encoding.UTF8.GetBytes(nickName);
            stream.WriteByte((byte)nickNameBytes.Length);
            stream.Write(nickNameBytes, 0, nickNameBytes.Length);


            Thread t_hanlder = new Thread(ReceiveMessage);
            t_hanlder.IsBackground = true;
            t_hanlder.Start();
        }
        private void ReceiveMessage()
        {
            while(true)
            {
                try
                {
                    int len = stream.ReadByte();
                    byte[] receiveBytes = new byte[len];
                    stream.Read(receiveBytes, 0, len);
                    string changedText = Encoding.UTF8.GetString(receiveBytes);
                    MessageBox.Show(changedText);
                }
                catch
                {

                }
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            stream.Close();
            client.Close();
        }
    }
}
