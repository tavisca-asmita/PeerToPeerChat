using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientChat
{
    public class SocketListener
    {

        public static void Main(string[] args)
        {
            StartServer();
        }
        
        public static void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];

            //IPAddress ipAddress = IPAddress.Parse("172.16.5.234");
            
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            

            try
            {


                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);

                listener.Listen(5);

                Console.WriteLine("Waiting for connection!!!!");
                
                Socket handler = listener.Accept();

                string message = null;

                byte[] data = null;

                while (true)
                {
                    data = new byte[1024];
                    int dataRecieved = handler.Receive(data);
                    message = Encoding.ASCII.GetString(data, 0, dataRecieved);

                    Console.WriteLine("Client : {0}", message);

                    if (message.ToLowerInvariant().Equals("bye"))
                        break;

                    string serverMessage = Console.ReadLine();
                    byte[] dataSent = Encoding.ASCII.GetBytes(serverMessage);
                    handler.Send(dataSent);
                }
                
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }
    }
}
