using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public abstract class Medium
    {
        public abstract string QA(string request);
    }

    public class MediumTCP : Medium
    {
        private NetworkStream _networkStream;
        public MediumTCP(NetworkStream stream)
        {
            _networkStream = stream;
        }

        public override string QA(string request)
        {
            byte[] data = Encoding.ASCII.GetBytes(request);
            _networkStream.Write(data, 0, data.Length);
            byte[] msg = new byte[256];
            string response = "";
            int bytes;
            do
            {
                bytes = _networkStream.Read(msg, 0, msg.Length);
                response += Encoding.ASCII.GetString(msg, 0, bytes);
            }
            while (_networkStream.DataAvailable);

            return response;
        }
    }

    public class MediumUDP : Medium
    {
        private UdpClient _client;
        private IPEndPoint _ipEndPoint;

        public MediumUDP(UdpClient udpClient)
        {
            _client = udpClient;
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(Config.SERVER_IP), Config.PORT_UDP);
        }

        public override string QA(string request)
        {
            byte[] data = Encoding.ASCII.GetBytes(request);
            _client.Send(data, data.Length);

            byte[] msg;
            string response = "";

            do
            {
                msg = _client.Receive(ref _ipEndPoint);
                response += Encoding.ASCII.GetString(msg);
            }
            while (response.IndexOf('\n') < 0);

            return response;
        }
    }
}
