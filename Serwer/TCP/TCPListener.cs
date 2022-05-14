using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Server.TCP;

namespace Server.TCP
{
    public class TCPListener : IListener
    {
        private TcpListener _tcpListener;
        private bool isRunning = true;
        private Thread _thread;

        public void Start(CommunicatorD onConnect)
        {
            _tcpListener = new TcpListener(IPAddress.Any, Config.PORT_TCP);
            _tcpListener.Start();

            _thread = new Thread(t =>
            {
                while (isRunning)
                {
                    TcpClient _client = _tcpListener.AcceptTcpClient();
                    TCPCommunicator _communicator = new TCPCommunicator(_client);
                    onConnect(_communicator);
                }
            });

            _thread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            _tcpListener.Stop();
        }
    }


}

//===== TESTING =====

//tcpServer = new TcpListener(IPAddress.Any, 4444);
//tcpServer.Start();
//byte[] bytes = new byte[256];

//while (true)
//{
//    TcpClient client = tcpServer.AcceptTcpClient();
//    string data;
//    int len;
//    NetworkStream stream = client.GetStream();
//    while ((len = stream.Read(bytes, 0, bytes.Length)) > 0)
//    {
//        data = Encoding.ASCII.GetString(bytes, 0, len);
//        Console.Write(data);
//        byte[] msg = Encoding.ASCII.GetBytes(data);
//        stream.Write(msg, 0, msg.Length);
//    }
//}