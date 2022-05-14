using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class PingPongServiceModule : IServiceModule
    {
        public string AnswerCommand(string command)
        {
            return PingPong.Pong(command.Split('\n')[0]);
        }
    }

    public class ChatServiceModule : IServiceModule
    {
        private List<Message> Messages = new List<Message>();
        public string AnswerCommand(string command)
        {
            string[] _args = command.Split(' ');
            switch (_args[1])
            {
                case "msg":
                    string[] _message = command.Split('"');
                    return Chat.Msg(_args[2], _args[3], _message[1], ref Messages);
                case "get":
                    List<Message> userMessages = Chat.Get(_args[2], Messages);
                    return userMessages.Count > 0 ? string.Join(" | ", userMessages.Select(m => m.ToString())) : "There is no messages for this user";
                case "getnow":
                //return "";
                case "who":
                //return "";
                default:
                    return "Unknown command";
            }
        }
    }
}
