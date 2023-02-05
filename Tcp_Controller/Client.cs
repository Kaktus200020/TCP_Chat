using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPChat_Library.Models;
using TCPChat_Library;

namespace Tcp_Controller
{
    class Client
    {
        Hero hero = new Hero();
        Socket socket_sender;
        public Client()
        {
            socket_sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(address, 7632);
            
            socket_sender.Connect(endPoint);
        }
        public void MoveRight()
        {
            hero.X += 50;
            Utility.JsonSerializeAndSend(hero, socket_sender);
        }
    }
}
