using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TCPChat_Library.Models;

namespace TCPChat_Library
{
    public class Utility
    {
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
        public static void XmlSerializeAndSend(Object obj, Socket socket)
        {
            
            XmlSerializer xmlData = new XmlSerializer(obj.GetType());

            MemoryStream stream = new MemoryStream();

            xmlData.Serialize(stream, obj);

            stream.Position = 0;
          
            byte[] bytes = stream.ToArray();
            
            socket.Send(bytes);
        }
        public static string JsonSerialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static void JsonSerializeAndSend(object obj,Socket socket)
        {
            string text = JsonSerialize(obj);
            SendMessage(socket, text);
        }
        public static void JsonDeserialize(string text)
        {
            Dumpling dumpling = JsonConvert.DeserializeObject<Dumpling>(text);
        }
    }
}
