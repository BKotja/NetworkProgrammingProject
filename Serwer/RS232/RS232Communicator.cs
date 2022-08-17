using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.RS232
{
    //TODO to fix (maybe threads?)
    public class RS232Communicator : ICommunicator
    {
        private SerialPort _serialPort;
        private CommandD _commandD;

        public RS232Communicator(SerialPort serialPort)
        {
            _serialPort = serialPort;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(Handler);
        }

        private void Handler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)sender;
            string newData;

            newData = serialPort.ReadLine();
            Console.Write(string.Format("RS232 | get: {0}\n", newData));

            serialPort.WriteLine(_commandD(newData));

        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            _commandD = onCommand;
            _serialPort.Open();
        }

        public void Stop()
        {
            _serialPort.Close();
        }

    }
}
