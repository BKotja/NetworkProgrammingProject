using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DotNetRemotingMarshalingObj : MarshalByRefObject
    {
        public delegate string CommandD(string command);
        private CommandD _onCommand;
        public DotNetRemotingMarshalingObj(CommandD onCommand)
        {
            _onCommand = onCommand;
        }
        public string Generate(string command)
        {
            if (_onCommand != null)
            {
                Console.Write(string.Format(".NET Remoting | get: {0}", command));
                return _onCommand(command);
            }
            else
                return "Marshaling error";
        }
    }
}
