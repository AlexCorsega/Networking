using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace Networking
{
    class Program
    {
        static TcpListener _tcpListener;
        static WebClient _webClient;
        static void Main(string[] args)
        {
            //foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    Console.WriteLine("Name: " + netInterface.Name);
            //    Console.WriteLine("Description: " + netInterface.Description);
            //    Console.WriteLine("Addresses: ");
            //    IPInterfaceProperties ipProps = netInterface.GetIPProperties();
            //    foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
            //    {
            //        Console.WriteLine(" " + addr.Address.ToString());
            //    }
            //    Console.WriteLine("");
            //}
            _webClient = new WebClient();
            string downloadString = _webClient.DownloadString("https://ifconfig.me/all.json");
            string ip = downloadString.Substring(16, 14);
            Console.WriteLine(ip);
            TcpListener _tcpListener = new TcpListener(IPAddress.Parse("192.168.1.4"), 1234);
            Console.WriteLine(_tcpListener.Server.RemoteEndPoint);
            Console.WriteLine(_tcpListener.Server.LocalEndPoint);
            _tcpListener.Start(5);
            Console.WriteLine("Server Started...");
            Console.WriteLine("Waiting For Connection...");
            while (true)
            {
                TcpClient client = _tcpListener.AcceptTcpClient();
                Task.Factory.StartNew(SetupServer, client);
            }
            Console.ReadKey();
        }
        private static void SetupServer(object client)
        {
            try
            {
                Console.WriteLine("Client IPAddress : " + (client as TcpClient).Client.RemoteEndPoint);
                while (true)
                {
                    var clients = (TcpClient)client;
                    byte[] bytes = new byte[clients.ReceiveBufferSize];
                    NetworkStream ns = clients.GetStream();
                    ns.Read(bytes, 0, bytes.Length);
                    Console.Clear();
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(bytes));
                    ns.Write(ASCIIEncoding.ASCII.GetBytes("Hi from server"), 0, 14);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.CurrentThread.Abort();
            }

        }
        private static void BeginAcceptCallback(IAsyncResult asyncResult)
        {
        }
    }
}
