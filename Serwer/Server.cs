using Server.TCP;
using Server.UDP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        List<IListener> _listeners = new List<IListener>();
        List<ICommunicator> _communicators = new List<ICommunicator>();
        Dictionary<string, IServiceModule> _services = new Dictionary<string, IServiceModule>();

        public Server() { }

        void AddService(string name, IServiceModule service)
        {
            _services.Add(name, service);
        }

        void AddCommunicator(ICommunicator communicator)
        {
            _communicators.Add(communicator);
            communicator.Start(GetService, RemoveCommunicator);
        }

        void AddListener(IListener listener)
        {
            _listeners.Add(listener);
            listener.Start(new CommunicatorD(AddCommunicator));
        }

        void RemoveService(string name, IServiceModule service)
        {
            _services.Remove(name);
        }

        void RemoveCommunicator(ICommunicator communicator)
        {
            _communicators.Remove(communicator);
        }

        void RemoveListener(IListener listener)
        {
            _listeners.Remove(listener);
        }

        string GetService(string serviceName)
        {
            string[] service = serviceName.Split(' ');
            string _serviceName = service[0];
            string _serviceParams = string.Join(" ",service.Skip(1));
            IServiceModule serviceModule = null;
            _services.TryGetValue(_serviceName, out serviceModule);

            if (serviceModule != null)
                return serviceModule.AnswerCommand(string.Format("{0} {1}", _serviceName, _serviceParams));
            else
                throw new MissingMethodException();

        }

        static void Main()
        {
            Console.WriteLine("Server logs");
            Server _server = new Server();

            _server.AddService("ping", new PingPongServiceModule());
            _server.AddService("chat", new ChatServiceModule());

            _server.AddListener(new TCPListener());
            _server.AddListener(new UDPListener());
        }
    }
}
