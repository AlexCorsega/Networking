using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.Win32;

namespace Client
{
    class Program
    {
        static TcpClient _client;
        static Socket _socket;
        static void Main(string[] args)
        {
            _client = new TcpClient("192.168.1.4", 1234);
            //_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //_socket.Connect(IPAddress.Loopback, 1234);
            //_client.Client = _socket;
            //Console.WriteLine($"My IPAddress {_client.Client.LocalEndPoint.ToString()}");
            //_socket.Send(ASCIIEncoding.ASCII.GetBytes(message));
            NetworkStream ns = _client.GetStream();
            while (true)
            {
                Console.Write("Enter Message: ");
                string message = Console.ReadLine();
                byte[] byteTosend = ASCIIEncoding.ASCII.GetBytes(message);
                ns.Write(byteTosend, 0, byteTosend.Length);
                byte[] bytesToRead = new byte[_client.ReceiveBufferSize];
                int bytesRead = ns.Read(bytesToRead, 0, _client.ReceiveBufferSize);
                Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                //Console.Clear();
                //_socket.Close();
            }
            _client.Close();


        }
    }
}
