using Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.RS232
{
    public class RS232Listener : IListener
    {
        private SerialPort _serialPort;

        public RS232Listener()
        {
            _serialPort = new SerialPort(Config.SERVER_SERIAL_PORT, 9600, Parity.None, 8, StopBits.One);
        }

        public RS232Listener(string config)
        {
            if (config != "")
            {
                string[] splitted = config.Split(' ');
                _serialPort = new SerialPort(splitted[0]);
            }
        }

        public void Start(CommunicatorD onConnect)
        {
            if (_serialPort != null)
            {
                onConnect(new RS232Communicator(_serialPort));
            }
        }

        public void Stop()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
            }
        }
    }
}
