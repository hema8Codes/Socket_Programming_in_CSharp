using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Xml;

namespace SocketsServerStarter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a socket that listens for incoming connections
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to an IP address and port
            IPAddress ipaddr = IPAddress.Any;
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);
            listenerSocket.Bind(ipep);

            // Start listening for incoming connections with a maximum backlog of 5
            listenerSocket.Listen(5);

            Console.WriteLine("About to accept the incoming connection");

            // Accept an incoming connection
            Socket client = listenerSocket.Accept();
            Console.WriteLine("client connected"+ client.ToString() + " - IP End point" + client.RemoteEndPoint.ToString());

            // Receive data from the client
       
            byte[] buff = new byte[128];
            int numberOfReceivedBytes = 0;

            while (true)
            {

                numberOfReceivedBytes = client.Receive(buff);

                Console.WriteLine("Number of received bytes " + numberOfReceivedBytes);

                Console.WriteLine("Data sent by client is: " + buff);

                // Convert received bytes to a human-readable string
                string receivedText = Encoding.ASCII
                    .GetString(buff, 0, numberOfReceivedBytes);

                Console.WriteLine("Data sent by client is: " + receivedText);

                client.Send(buff);

                if(receivedText == "x")
                {
                    break;
                }

                Array.Clear(buff, 0, buff.Length);
                numberOfReceivedBytes = 0;
            }
            Console.ReadLine();

            // Close the client socket when done
            client.Close();

            // Close the listener socket when done
            listenerSocket.Close();

        }
    }
}
