using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Server.DotNetRemoting
{
    public class DotNetRemotingListener : IListener
    {
        private TcpChannel _channel;

        public DotNetRemotingListener()
        {
            _channel = new TcpChannel(Config.DOTNETREMOTING_PORT);
        }

        public void Start(CommunicatorD onConnect)
        {
            onConnect(new DotNetRemotingCommunicator(_channel));
        }

        public void Stop()
        {
            ChannelServices.UnregisterChannel(_channel);
        }
    }
}
