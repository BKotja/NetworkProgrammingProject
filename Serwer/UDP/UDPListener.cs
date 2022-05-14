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
        //private bool isRunning = true;
        //private Thread _thread;

        public void Start(CommunicatorD onConnect)
        {
            // _thread = new Thread(t =>
            // {
            //     while (isRunning)
            //     {
            _udpListener = new UdpClient(Config.PORT_UDP);
            UDPCommunicator _communicator = new UDPCommunicator(_udpListener);
            onConnect(_communicator);
            //    }
            //});

            //_thread.Start();
        }

        public void Stop()
        {
            //isRunning = false;
            _udpListener.Close();
        }
    }
}
