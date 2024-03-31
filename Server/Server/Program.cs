using System.Net.Sockets;
using System.Net;
using System.Text;
using Server;

UdpClient udpClient = new UdpClient(1234);
IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
Console.WriteLine("Сервер ждет сообщение от клиента...");

while (true)
{
    byte[] buffer = udpClient.Receive(ref ipEndPoint);
    if (buffer == null) break;
    var messageText = Encoding.UTF8.GetString(buffer);
    Message? messageServer = Message.DeserializeFromJsonToMessage(messageText);
    messageServer?.PrintMessage();
}