using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PingPong
    {
        public static string Ping(int requestSize, int responseSize)
        {
            string request = string.Format("ping {0} ", responseSize);
            int length = (requestSize - request.Length - 2) > 0 ? (requestSize - request.Length - 2) : requestSize;

            request += GenerateBytes(length) + "\n";
            return request;
        }

        private static string GenerateBytes(int count)
        {
            Random r = new Random();

            char[] arr = new char[count];
            for (int i = 0; i < count; i++)
                arr[i] = (char)r.Next(48, 122);
            return new string(arr);
        }

        public static string Pong(string line)
        {
            string[] answ = line.Split();
            string response = "pong ";
            int length = (int.Parse(answ[1]) - response.Length - 1 > 0) ? (int.Parse(answ[1]) - response.Length - 1) : int.Parse(answ[1]);

            response += GenerateBytes(length) + "\n";
            return response;
        }
    }
}
