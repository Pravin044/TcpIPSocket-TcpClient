// A C# program for Client 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpClient
{


    class Program
    {

        // Main Method 
        static void Main(string[] args)
        {

            try
            {

                // Establish the remote endpoint 
                // for the socket. This example 
                // uses port 11111 on the local 
                // computer. 
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = IPAddress.Parse("192.168.1.25");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 8080);



                try
                {

                    // Creation TCP/IP Socket using 
                    // Socket Class Costructor 
                    Socket sender = new Socket(ipAddr.AddressFamily,
                            SocketType.Stream, ProtocolType.Tcp);
                    // Connect Socket to the remote 
                    // endpoint using method Connect() 
                    sender.Connect(localEndPoint);

                    // We print EndPoint information 
                    // that we are connected 
                    Console.WriteLine("Socket connected to -> {0} ",
                                sender.RemoteEndPoint.ToString());

                    // Creation of messagge that 
                    // we will send to Server 
                    byte[] messageSent = { 0x10, 0x20, 0x12, 0x00, 0x21, 0x25, 0x45, 0x1C };/*Encoding.ASCII.GetBytes("Test Client<EOF>");*/
                    int byteSent = sender.Send(messageSent);
                    Thread thr = new Thread(delegate ()
                     {
                         dataRecived(sender);
                     });
                    thr.Start();
                    int count = 0;
                    while (true)
                    {
                        count++;
                        System.Threading.Thread.Sleep(500);
                        if (count > 100)
                        {
                            thr.Abort();
                        }
                    }
                    //Close Socket using
                    //the method Close()
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }

                // Manage of Socket's Exceptions 
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
            Console.ReadKey();
        }

        

        private static void dataRecived(Socket sender)
        {
            while (true)
            {
                // Data buffer 
                byte[] messageReceived = new byte[1024];

                // We receive the messagge using 
                // the method Receive(). This 
                // method returns number of bytes 
                // received, that we'll use to 
                // convert them to string 

                int byteRecv = sender.Receive(messageReceived);
                Console.WriteLine("data recived From server====>" + Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
            }
        }
    }
}

