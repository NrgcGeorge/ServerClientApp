using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class SynchronousSocketListener
    {

        public static string data = null;

        public static IPHostEntry ipHostInfo;
        public static IPAddress ipAddress;
        public static IPEndPoint localEndPoint;
        public static byte[] bytes;

        private static void Initialize()
        {
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, 11000);
            bytes = new Byte[1024];
        }

        public static void StartListening()
        {
            Initialize();

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    data = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        break;
                    }

                    Console.WriteLine("Text received : {0}", data);

                    Console.WriteLine("Type your message:");
                    string message = Console.ReadLine();

                    byte[] msg = Encoding.ASCII.GetBytes(data + "\n" + message);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }


        public static int Main(String[] args)
        {
            StartListening();
            return 0;
        }
    }

}