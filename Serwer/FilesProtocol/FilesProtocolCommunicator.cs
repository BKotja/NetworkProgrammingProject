using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.FilesProtocol
{
    public class FilesProtocolCommunicator : ICommunicator
    {
        private string fileName;
        public FilesProtocolCommunicator(string fileName)
        {
            this.fileName = fileName;
        }
        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            StreamReader streamReader = null;
            while (streamReader == null)
            {
                try
                {
                    streamReader = new StreamReader(fileName);
                }
                catch (Exception ex)
                {
                    
                }
            }

            while (!streamReader.EndOfStream)
            {
                File.WriteAllText(fileName.Replace(".txt", ".data"), onCommand(streamReader.ReadLine()));
                Console.Write(string.Format("FilesProtocol | get: {0}", fileName));
            }
            streamReader.Close();
        }

        public void Stop()
        {

        }
    }
}
