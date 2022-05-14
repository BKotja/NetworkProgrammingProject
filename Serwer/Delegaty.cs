using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public delegate string CommandD(string command);
    public delegate void CommunicatorD(ICommunicator commander);
}
