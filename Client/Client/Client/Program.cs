using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ClientTest
{
    public class SynchronousSocketClient
    {

        static IPHostEntry ipHostInfo;
        static IPAddress ipAddress;
        static IPEndPoint remoteEP;
        static Socket sender;
        static byte[] bytes;

        private static void Initialize()
        {
            remoteEP = new IPEndPoint(IPAddress.Parse("192.168.0.113"), 11000);
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            bytes = new byte[1024];
        }

        public static void StartClient(Message message)
        {
            try
            {
                Initialize();

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    byte[] msg = message.GetByteMessage();
                    int bytesSent = sender.Send(msg);
                    int bytesRec = sender.Receive(bytes);

                    Console.WriteLine("Echo test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
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
            }
        }

        public class Message
        {
            public string MessageText;
            public string IP;
            public static string SEND_MESSAGE;

            public Message(string ip)
            {
                Console.WriteLine("Type your message:");
                MessageText = Console.ReadLine();
                IP = ip;
                Console.WriteLine("Send meesage (y/n):");
                SEND_MESSAGE = Console.ReadLine();
            }

            public byte[] GetByteMessage()
            {
                return Encoding.ASCII.GetBytes(MessageText);
            }

        }

        public static int Main(String[] args)
        {

            do
            {
                StartClient(new Message("192.168.0.113"));
            } while (Message.SEND_MESSAGE != "n");

            return 0;
        }
    }
}