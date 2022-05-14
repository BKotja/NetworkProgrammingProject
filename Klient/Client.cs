using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {

        public Client() { }

        private void Ping(Medium medium, string[] message)
        {
            int pingCount = 1;
            string request = string.Empty;
            string response = string.Empty;
            List<TimeSpan> elapsed = new List<TimeSpan>();

            if (message.Length == 4)
            {
                bool success = int.TryParse(message[3], out pingCount);
                if (!success)
                    pingCount = 1;
            }

            for (int i = 0; i < pingCount; i++)
            {
                request = PingPong.Ping(int.Parse(message[1]), int.Parse(message[2]));

                Stopwatch timer = Stopwatch.StartNew();
                response = medium.QA(request);
                timer.Stop();

                Console.WriteLine(string.Format("{0} ({1} bytes) - {2} ", response.Split(' ')[0], Encoding.ASCII.GetByteCount(response), timer.Elapsed));
                elapsed.Add(timer.Elapsed);
            }
            Console.WriteLine(string.Format("Average time: {0}", PingAverageTime(elapsed)));
        }

        private TimeSpan PingAverageTime(List<TimeSpan> elapsed)
        {
            return TimeSpan.FromMilliseconds(elapsed.Select(s => s.TotalMilliseconds).Average());
        }

        private void ChatCommander(Medium medium, string message)
        {
            string response = string.Empty;

            response = medium.QA(message + " \n");

            Console.WriteLine(response);
        }

        static void Main()
        {
            Console.WriteLine("Client");
            Client _client = new Client();
            bool flag = true;
            string strMessage;
            string[] strMessageSplitted;

            Medium medium = null;

            Console.WriteLine("Choose protocol (tpc/udp):");
            while (flag)
            {
                strMessageSplitted = Console.ReadLine().Split(' ');
                switch (strMessageSplitted[0])
                {
                    case "tcp":
                        TcpClient tcpClient = new TcpClient(Config.SERVER_IP, Config.PORT_TCP);
                        NetworkStream stream = tcpClient.GetStream();
                        medium = new MediumTCP(stream);
                        flag = false;
                        break;
                    case "udp":
                        UdpClient udpClient = new UdpClient(Config.SERVER_IP, Config.PORT_UDP);
                        medium = new MediumUDP(udpClient);
                        flag = false;
                        break;
                    case "exit":
                        return;
                }
            }

            while (true)
            {
                strMessage = Console.ReadLine();
                strMessageSplitted = strMessage.Split(' ');
                switch (strMessageSplitted[0])
                {
                    case "ping":
                        _client.Ping(medium, strMessageSplitted);
                        break;
                    case "chat":
                        _client.ChatCommander(medium, strMessage);
                        break;
                    case "exit":
                        //stream.Close();
                        return;
                }
            };
        }
    }
}

//TcpClient tcpClient = new TcpClient("127.0.0.1", Config.PORT);
//NetworkStream stream = tcpClient.GetStream();

//string message = "ping 1024\n";
//byte[] data = Encoding.ASCII.GetBytes(message);

//for (int i = 0; i < 10; i++)
//{
//    stream.Write(data, 0, data.Length);
//    Console.Write("Wysłane: {0}", message);

//    byte[] msg = new byte[256];
//    int lenght;
//    string response;
//    do
//    {
//        lenght = stream.Read(msg, 0, msg.Length);
//        response = Encoding.ASCII.GetString(msg, 0, lenght);
//        Console.WriteLine("Pobrane: " + response);

//    } while (stream.DataAvailable);
//}

//tcpClient.Close();
