using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace ConnectionList_Server
{
    public partial class MainWindow : Window
    {
        readonly int PORT = 9999;
        TcpListener server;

        Thread connectionThread;

        ObservableCollection<HandleClient> clients = new ObservableCollection<HandleClient>();
        public MainWindow()
        {
            InitializeComponent();
            XAML_List.ItemsSource = clients;

            server = new TcpListener(IPAddress.Any,PORT);
            server.Start();
            
            // 접속 쓰레드
            connectionThread = new Thread(Connection);
            connectionThread.IsBackground = true;
            connectionThread.Start();
        }
        private void Connection()
        {
            while (true)
            {
                TcpClient tcpClient = server.AcceptTcpClient();

                HandleClient handleClient = new HandleClient();
                handleClient.OnDisconnected += DisconnectionClient;

                handleClient.StartClient(tcpClient);

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    clients.Add(handleClient);
                }));
            }
        }
        private void DisconnectionClient(HandleClient disconnectedClient)
        {
            if(clients.Contains(disconnectedClient))
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    clients.Remove(disconnectedClient);
                }));

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(XAML_List.SelectedIndex == -1)
            {
                MessageBox.Show("선택되지 않은 사용자입니다.");
                return;
            }
            (XAML_List.SelectedItem as HandleClient).SendMessage("GOOD");
        }
    }
}