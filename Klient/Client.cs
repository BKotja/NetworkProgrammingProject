using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {

        public Client() { }

        private void PingCommander(Medium medium, string[] message)
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

        private void FileCommander(Medium medium, string[] message)
        {
            string response = string.Empty;
            string request = string.Empty;

            switch (message[1])
            {
                case "list":
                    request = string.Format("{0} {1}", message[0], message[1]);
                    break;
                case "get":
                    request = string.Format("{0} {1} {2}", message[0], message[1], message[2]);
                    break;
                case "put":
                    string filePath = string.Format("{0}", message[3]);
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("File not found\n");
                        return;
                    }
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    string fileBase64 = Convert.ToBase64String(fileBytes);
                    request = string.Format("{0} {1} {2} {3}", message[0], message[1], message[2], fileBase64);
                    break;
                default:
                    return;
            }

            response = medium.QA(request + "\n");

            if (message[1] == "get" && response != "404\n")
            {
                string fileServerPath = string.Format("{0}\\{1}", message[3], message[2]);
                File.WriteAllBytes(fileServerPath, Convert.FromBase64String(response.Split('\n')[0]));
                Console.WriteLine("File downloaded successfully");

            }
            else
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
                        _client.PingCommander(medium, strMessageSplitted);
                        break;
                    case "chat":
                        _client.ChatCommander(medium, strMessage);
                        break;
                    case "file":
                        _client.FileCommander(medium, strMessageSplitted);
                        break;
                    case "exit":
                        return;
                }
            };
        }
    }
}
