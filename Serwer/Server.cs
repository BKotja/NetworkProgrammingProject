using Server.RS232;
using Server.FilesProtocol;
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
            communicator.Stop();
            _communicators.Remove(communicator);
        }

        void RemoveListener(IListener listener)
        {
            listener.Stop();
            _listeners.Remove(listener);
        }

        string GetService(string serviceName)
        {
            string[] service = serviceName.Split(' ');
            string _serviceName = service[0];
            string _serviceParams = string.Join(" ", service.Skip(1));
            IServiceModule serviceModule = null;
            _services.TryGetValue(_serviceName, out serviceModule);
            
            if (serviceModule != null)
                return serviceModule.AnswerCommand(string.Format("{0} {1}", _serviceName, _serviceParams));
            else
                throw new MissingMethodException();

        }

        void ConfigCommander(string[] command)
        {
            if (command[0] != "conf")
            {
                Console.WriteLine("Unknown command");
                return;
            }

            switch (command[1])
            {
                case "start-all":
                    StartService("all");
                    StartMedium("all");
                    break;
                case "start-service":
                    StartService(command[2]);
                    break;
                case "stop-service":
                    StopService(command[2]);
                    break;
                case "start-medium":
                    StartMedium(command[2]);
                    break;
                case "stop-medium":
                    StopMedium(command[2]);
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }

        void StartService(string serviceName)
        {
            switch (serviceName)
            {
                case "ping":
                    AddService("ping", new PingPongServiceModule());
                    break;
                case "chat":
                    AddService("chat", new ChatServiceModule());
                    break;
                case "file":
                    AddService("file", new FileServiceModule());
                    break;
                case "all":
                    AddService("ping", new PingPongServiceModule());
                    AddService("chat", new ChatServiceModule());
                    AddService("file", new FileServiceModule());
                    break;
                default:
                    Console.WriteLine("Unknown service");
                    break;
            }
        }

        void StopService(string serviceName)
        {
            if (_services.ContainsKey(serviceName))
            {
                RemoveService(serviceName, _services[serviceName]);
                Console.WriteLine(string.Format("Stopped {0} service...", serviceName));
            }
            else
                Console.WriteLine("Unknown service");
        }

        void StartMedium(string mediumName)
        {
            switch (mediumName)
            {
                case "tcp":
                    AddListener(new TCPListener());
                    break;
                case "udp":
                    AddListener(new UDPListener());
                    break;
                case "files":
                    AddListener(new FilesProtocolListener());
                    break;
                case "RS232":
                    AddListener(new RS232Listener());
                    break;
                case "all":
                    AddListener(new TCPListener());
                    AddListener(new UDPListener());
                    AddListener(new FilesProtocolListener());
                    AddListener(new RS232Listener());
                    break;
                default:
                    Console.WriteLine("Unknown service");
                    break;
            }
        }

        void StopMedium(string mediumName)
        {
            switch (mediumName)
            {
                case "tcp":
                    RemoveListener(_listeners.Where(listener => listener is TCPListener).First());
                    break;
                case "udp":
                    RemoveListener(_listeners.Where(listener => listener is UDPListener).First());
                    break;
                default:
                    Console.WriteLine("Unknown service");
                    break;
            }
        }

        void StopServer()
        {
            _listeners.ForEach(listner => listner.Stop());
            _communicators.ForEach(communicator => communicator.Stop());

            _listeners.Clear();
            _communicators.Clear();
            _services.Clear();

            Console.WriteLine("Server stopped...");
        }

        static void Main()
        {
            Console.WriteLine("Server logs");
            Server _server = new Server();
            string command;
            string[] commandParams;

            //_server.AddService("ping", new PingPongServiceModule());
            //_server.AddService("chat", new ChatServiceModule());
            //_server.AddService("file", new FileServiceModule());

            //_server.AddListener(new TCPListener());
            //_server.AddListener(new UDPListener());

            while (true)
            {
                command = Console.ReadLine();
                commandParams = command.Split(' ');
                if (commandParams[0] == "exit")
                {
                    _server.StopServer();
                    return;
                }
                _server.ConfigCommander(commandParams);
            }
        }
    }
}
