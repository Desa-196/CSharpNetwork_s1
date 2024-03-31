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

    Message requestMessage;

    if (messageServer != null)
    {
        requestMessage = new Message() { Text = "ok", DateTime = DateTime.Now, NickNameFrom = "Server", NickNameTo = messageServer.NickNameFrom };
    }
    else
    {
        requestMessage = new Message() { Text = "error", DateTime = DateTime.Now, NickNameFrom = "Server", NickNameTo = messageServer.NickNameFrom };
    }

    string json = requestMessage.SerializeMessageToJson();

    byte[] data = Encoding.UTF8.GetBytes(json);

    //Отправляем ответное сообщение
    udpClient.Send(data, data.Length, ipEndPoint);
}