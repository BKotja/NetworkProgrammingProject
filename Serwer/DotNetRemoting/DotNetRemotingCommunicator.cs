using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Server.DotNetRemoting
{
    public class DotNetRemotingCommunicator : ICommunicator
    {
        private TcpChannel _channel;

        public DotNetRemotingCommunicator(TcpChannel channel)
        {
            _channel = channel;
        }
        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            ChannelServices.RegisterChannel(_channel, false);
            DotNetRemotingMarshalingObj _dotNetRemotingMarshalingObj = new DotNetRemotingMarshalingObj(new DotNetRemotingMarshalingObj.CommandD(onCommand));
            RemotingServices.Marshal(_dotNetRemotingMarshalingObj, "command");
        }

        public void Stop()
        {
            ChannelServices.UnregisterChannel(_channel);
        }
    }
}
