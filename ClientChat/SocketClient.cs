using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace ClientChat
{
    public class SocketClient
    {
        public static void Main(string[] args)
        {
            StartClient();
        }


        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = host.AddressList[0];

                //IPAddress ipAddress = IPAddress.Parse("172.16.5.187");
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);

                  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(ipEndPoint);
                    

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                   
                    while (true)
                    {
                        string clientMessage = Console.ReadLine();
                        byte[] message = Encoding.ASCII.GetBytes(clientMessage);
                        int byteSent = sender.Send(message);
                                                
                        if (clientMessage.ToLowerInvariant().Equals("bye"))
                            break;

                        byte[] servermessage = new byte[1024];
                        int receivedMessage = sender.Receive(servermessage);
                        string serverMessage = Encoding.ASCII.GetString(servermessage, 0, receivedMessage);

                        Console.WriteLine("Server : {0}", serverMessage);
                    }
                    

                        
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }
    }
}
    


