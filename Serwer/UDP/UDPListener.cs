using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.UDP
{
    public class UDPListener : IListener
    {
        private UdpClient _udpListener;

        public void Start(CommunicatorD onConnect)
        {
            _udpListener = new UdpClient(Config.PORT_UDP);
            UDPCommunicator _communicator = new UDPCommunicator(_udpListener);
            onConnect(_communicator);
        }

        public void Stop()
        {
            _udpListener.Close();
        }
    }
}
