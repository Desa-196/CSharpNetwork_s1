using System.Net.Sockets;
using System.Net;
using System.Text;
using Client;


UdpClient udpClient = new UdpClient();
IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);


while (true)
{
    string? messageText;
    do
    {
        Console.Clear();
        Console.WriteLine("Введите сообщение: ");
        messageText = Console.ReadLine();
    }
    while (string.IsNullOrEmpty(messageText));

    Message message = new Message() { Text = messageText, DateTime = DateTime.Now, NickNameFrom = "Denis", NickNameTo = "Server" };
    string json = message.SerializeMessageToJson();

    byte[] data = Encoding.UTF8.GetBytes(json);
    udpClient.Send(data, data.Length, ipEndPoint);
}
