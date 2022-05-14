using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface ICommunicator
    {
        void Start(CommandD onCommand, CommunicatorD onDisconnect);
        void Stop();
    }
}
