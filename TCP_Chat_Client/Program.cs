using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("[CLIENT]");
            Socket socket_sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(address, 7632);
            Console.WriteLine("Нажмите Энтер для подключения");
            Console.ReadLine();
            socket_sender.Connect(endPoint);
            while(true)
            {
                Console.WriteLine("Введите сообщение: ");
                string message = Console.ReadLine();
                byte[] bytes = Encoding.Unicode.GetBytes(message);
                socket_sender.Send(bytes);
                Console.WriteLine("Посылка \"" + message + "\" отправлена");
                byte[] byte_answers = new byte[1024];
                int num_bytes=socket_sender.Receive(byte_answers);
                string answer=Encoding.Unicode.GetString(byte_answers, 0, num_bytes);
                Console.WriteLine(answer);
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
