using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

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
            Console.WriteLine("Введите Имя: ");
            string name = Console.ReadLine();
            SendMessage(socket_sender, name);
            Action<Socket> taskSendMessage = SendMessageForTask;
            taskSendMessage.BeginInvoke(socket_sender, null, null);
            
            
            while(true)
            { 
                string answer = GetMessage(socket_sender);
                Console.WriteLine(answer);  
            }
            Console.ReadLine();
        }
        public static void SendMessageForTask(Socket socket)
        {
            while(true)
            {
                string message = Console.ReadLine();
                if(message=="platypus")
                {
                    Platypus platypus = new Platypus()
                    {
                        Size = 2,Color="Brown"
                        
                    };
                    XmlSerializer xmlData = new XmlSerializer(typeof(Platypus));
                    MemoryStream stream = new MemoryStream();
                    xmlData.Serialize(stream, platypus);
                    stream.Position = 0;
                    Platypus platypus2 = xmlData.Deserialize(stream) as Platypus;
                    byte[] bytes = stream.ToArray();
                    //SendMessage(socket, message);
                    socket.Send(bytes);
                }
                if(message=="dumpling")
                {
                    Dumpling dumpling = new Dumpling() {IsFried=true , Name="Иван", Description="Его имя Иван, он крутой" };
                    string text=JsonConvert.SerializeObject(dumpling);
                    SendMessage(socket, text);
                }
                else
                {
                    SendMessage(socket, message);
                }
                
            }
        }
        public static void SendMessage(Socket socket, string message)
        {
            byte[] bytes_answer = Encoding.Unicode.GetBytes(message);
            socket.Send(bytes_answer);
        }
        public static string GetMessage(Socket socket)
        {
            byte[] bytes = new byte[1024];
            int num_bytes = socket.Receive(bytes);
            return Encoding.Unicode.GetString(bytes, 0, num_bytes);

        }
    }
}
