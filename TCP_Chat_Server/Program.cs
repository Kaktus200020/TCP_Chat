using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using TCPChat_Library;
using TCPChat_Library.Models;

namespace TCP_Chat_Server
{
    class Program
    {
        static string messageFromUser = "";
        static List<Socket> sockets = new List<Socket>();
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("[SERVER]");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(address, 7632);
            socket.Bind(endPoint);
            socket.Listen(2);
            Console.WriteLine("Ожидаем звонка от клиента");
            while (true)
            {
                Socket socket_client = socket.Accept();
                sockets.Add(socket_client);
                Console.WriteLine("Клиент на связи");
                Thread threadReceive = new Thread(GetMessageForManager);
                threadReceive.Start(socket_client);
                Thread threadSend = new Thread(SendMessageForManager);

                threadSend.Start(socket_client);
            }



            Console.ReadLine();
        }
        
        
        public static void GetMessageForManager(object socketObj)
        {
            Socket socket = (Socket)socketObj;
            string name = Utility.GetMessage(socket);
            while (true)
            {
                messageFromUser = Utility.GetMessage(socket);
                Console.WriteLine("[" + name + "]: " + messageFromUser);
                //Раскодировать сообщение от пользователя
                //ProcessCommandWord(socket,messageFromUser);
                //ProcessCommandCoding(socket, messageFromUser);
                //byte[] bytes = new byte[1024];
                //int num_bytes = socket.Receive(bytes);
                //ProcessCommandXML (socket, bytes, num_bytes);
                Utility.JsonDeserialize(messageFromUser);
            }

        }
        public static void SendMessageToAllUsers(string message)
        {
            foreach (var socket in sockets)
            {
                Utility.SendMessage(socket, message);
            }
        }
        public static void SendMessageForManager(object socketObj)
        {
            while (true)
            {
                Socket socket = (Socket)socketObj;
                string sendMessage = Console.ReadLine();

                SendMessageToAllUsers(sendMessage);

            }
        }
        private static void ProcessCommandXML(Socket socket, byte[] bytes, int num_bytes)
        {
            XmlSerializer xmlData = new XmlSerializer(typeof(Platypus));
           
            MemoryStream stream = new MemoryStream(bytes,0,num_bytes);
            stream.Position = 0;
            Platypus platypus2 = xmlData.Deserialize(stream) as Platypus;
        }
        private static void ProcessCommandJson(Socket socket, string text)
        {
            Dumpling dumpling=JsonConvert.DeserializeObject<Dumpling>(text);
        }
        private static void ProcessCommandWord(Socket socket, string command)
        {
            if (messageFromUser == "color")
            {
                Console.WriteLine("Пользователь прислал команду color");
                Utility.SendMessage(socket, "сервер принял вашу команду");
            }
        }
        private static void ProcessCommandCoding(Socket socket, string text)
        {
            int health, level, money;
            string[] numsText =text.Split(',');
            health = int.Parse(numsText[0]);
            level = int.Parse(numsText[1]);
            money = int.Parse(numsText[2]);
            Console.WriteLine($"Health:{health}, level:{level},money:{money}");
        }
    }
    

}
