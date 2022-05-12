using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpReciever
{
    class Program
    {
        private static string URL = "https://udprest20220504132553.azurewebsites.net/api/colour";

      private const int Port = 4109; //TODO husk port
      
        static void Main()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            using (UdpClient socket = new UdpClient(ipEndPoint))
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast {0}", socket.Client.LocalEndPoint);
                    byte[] datagramReceived = socket.Receive(ref remoteEndPoint);

                    string message = Encoding.ASCII.GetString(datagramReceived, 0, datagramReceived.Length);
                    Console.WriteLine("Receives {0} bytes from {1} port {2} message {3}", datagramReceived.Length,
                        remoteEndPoint.Address, remoteEndPoint.Port, message);

                    HttpClient client = new HttpClient();

                    //HttpContent content = new StringContent(message, Encoding.UTF8);

                    HttpResponseMessage response = client.PutAsync(URL + "?colour=" + message, null).Result;
                    Console.WriteLine(response.StatusCode);

                    // TODO Kalde rest post

                }

            }
        }

        private static void Parse(string response)
        {
            string[] parts = response.Split(' ');
            foreach (string part in parts)
            {
                Console.WriteLine(part);
            }
            string lightLine = parts[6];
            string lightStr = lightLine.Substring(lightLine.IndexOf(": ") + 2);
            Console.WriteLine(lightStr);
        }
    }
}