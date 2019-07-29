using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectionList_Server
{
    class HandleClient
    {
        TcpClient clientSocket = null;
        NetworkStream stream;
        
        // 접속자 IP
        private IPAddress ip;
        public IPAddress IP
        {
            get { return ip; }
            set { ip = value; }
        }

        // 접속자 이름 ( 맨 처음 입력 값이 닉네임)
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public void StartClient(TcpClient clientSocket)
        {
            this.clientSocket = clientSocket;
            stream = clientSocket.GetStream();

            int len = stream.ReadByte();
            byte[] receiveByte = new byte[len];
            stream.Read(receiveByte, 0, len);

            name = Encoding.UTF8.GetString(receiveByte);
            ip = ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address;

            Console.WriteLine("{0}님이 입장하였습니다.", name);

            Thread t_hanlder = new Thread(Connection);
            t_hanlder.IsBackground = true;
            t_hanlder.Start();
        }

        // 연결이 종료 됐을 때
        public delegate void DisconnectedHandler(HandleClient clientSocket);
        public event DisconnectedHandler OnDisconnected;

        private void Connection()
        {
            try
            {
                byte[] buffer = new byte[1024];
                string msg = string.Empty;

                // 접속 유지 중인지 확인
                while (true)
                {
                    stream = clientSocket.GetStream();
                    int len = stream.ReadByte();
                    if(len == -1)
                    {
                        break;
                    }
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine(string.Format("SocketException : {0}", se.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception : {0}", ex.Message));
            }
            finally
            {
                if (clientSocket != null)
                {
                    Console.WriteLine("{0}님이 퇴장하였습니다.", name);
                    OnDisconnected?.Invoke(this);
                    clientSocket.Close();
                    stream.Close();
                    
                }
            }
        }
        public void SendMessage(string msg)
        {
            byte[] sendMessages = Encoding.UTF8.GetBytes(msg);
            stream.WriteByte((byte)sendMessages.Length);
            stream.Write(sendMessages,0,sendMessages.Length);
        }
    }
}
