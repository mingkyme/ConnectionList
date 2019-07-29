using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConnectionList_Server
{

    public partial class Window1 : Window
    {
        readonly static int unityPort = 6568;
        readonly static int wpfPort = 6569;
        UdpClient udp = new UdpClient(wpfPort);
        // TcpClient tcp = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"),wpfPort));

        IPEndPoint unityEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.27"), unityPort);
        IPEndPoint anyEndPoint = new IPEndPoint(IPAddress.Any, wpfPort);
        public Window1()
        {
            InitializeComponent();
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string msg = "DataFromController" + Environment.NewLine + "";
            byte[] msgByteArray = Encoding.UTF8.GetBytes(msg);
            udp.Send(msgByteArray, msgByteArray.Length, unityEndPoint);
            MessageBox.Show("보냄");

        }
    }
}
