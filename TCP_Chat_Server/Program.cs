using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP_Chat_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("[SERVER]");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(address, 7632);
            socket.Bind(endPoint);
            socket.Listen(1);
            Console.WriteLine("Ожидаем звонка от клиента");
            Socket socket_client = socket.Accept();
            Console.WriteLine("Клиент на связи");
            while(true)
            {
                byte[] bytes = new byte[1024];
                int num_bytes = socket_client.Receive(bytes);
                string textFromClient = Encoding.Unicode.GetString(bytes, 0, 20);
                Console.WriteLine(textFromClient);

                // ответное сообщение от сервера к клиенту 
                string answer = "SERVER: OK";
                byte[] bytes_answer = Encoding.Unicode.GetBytes(answer);
                socket_client.Send(bytes_answer);
                 
            }
            
            Console.ReadLine();
        }
    }
}
