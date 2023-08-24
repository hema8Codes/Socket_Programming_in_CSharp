using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientStarter
{
    public class Program
    {
        public static void Main(string[] args)
        {
           Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipaddr = null;

            try
            {
                Console.WriteLine("**** This is a Socket Client Starter ****");
                Console.WriteLine(" Please Type a Valid Server IP Address and Press Enter ");

                string strIPAddress = Console.ReadLine();

                Console.WriteLine("Please Supply a valid Port Number b/w 0 - 65535 and Press Enter: ");
                string strPortInput = Console.ReadLine();
                int nPortInput = 0;

                if(!IPAddress.TryParse(strIPAddress, out ipaddr))
                {
                    Console.WriteLine("Invalid server IP supplied");
                    return;
                }

                if(!int.TryParse(strPortInput.Trim(), out nPortInput))
                {
                    Console.WriteLine("Invalid port number supplied, return");
                    return;
                }

                if(nPortInput <= 0 || nPortInput > 65535)
                {
                    Console.WriteLine("Port number must be between 0 and 65535.");
                }

                System.Console.WriteLine(string.Format("IPAddress: {0} - Port: {1}", ipaddr.ToString(), nPortInput));

                client.Connect(ipaddr, nPortInput);

                Console.WriteLine("Connected to the server, type text and press enter to send it to the server, type <EXIT> to close.");

                string inputCommand = string.Empty;

                while(true)
                {
                    inputCommand = Console.ReadLine();

                    if (inputCommand.Equals("<EXIT>"))
                    {
                        break;
                    }

                    byte[] buffSend =  Encoding.ASCII.GetBytes(inputCommand);

                    client.Send(buffSend);

                    byte[] buffReceived = new byte[128];

                    int nRecv = client.Receive(buffReceived);

                    Encoding.ASCII.GetString(buffReceived, 0, nRecv);

                    Console.WriteLine("Data received: {0}", Encoding.ASCII.GetString(buffReceived, 0, nRecv));
                }

                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " There is something wrong");
            }
            finally
            {
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);
                    }
                    client.Close();
                    client.Dispose();
                }

                
            }

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }
    }
}
