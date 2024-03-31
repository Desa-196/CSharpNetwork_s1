using System.Net.Sockets;
using System.Net;
using System.Text;
using Client;


UdpClient udpClient = new UdpClient();

//Устанавляваем таймаут ожидания приема ответа
udpClient.Client.ReceiveTimeout = 1000;

IPEndPoint myIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
IPEndPoint ServeripEndPoint = new IPEndPoint(IPAddress.Any, 0);


while (true)
{
    string? messageText;
    do
    {
        Console.WriteLine("Введите сообщение: ");
        messageText = Console.ReadLine();
    }
    while (string.IsNullOrEmpty(messageText));

    Message message = new Message() { Text = messageText, DateTime = DateTime.Now, NickNameFrom = "Denis", NickNameTo = "Server" };
    string json = message.SerializeMessageToJson();

    byte[] data = Encoding.UTF8.GetBytes(json);

    //Отправляем сообщение
    udpClient.Send(data, 0, myIpEndPoint);

    //Ждем ответа от сервера
    try
    {
        byte[] buffer = udpClient.Receive(ref ServeripEndPoint);
        if (buffer == null) break;

        Message? receiveMessage = Message.DeserializeFromJsonToMessage(Encoding.UTF8.GetString(buffer));

        if (receiveMessage.Text == "ok") Console.WriteLine("Сообщение доставлено!");
        else Console.WriteLine("Ошибка получения сообщения сервером!");

    }
    //Если прошел таймаут и сервер не ответил
    catch(System.Net.Sockets.SocketException e)
    {
        Console.WriteLine("Сервер не ответил за указанный промежуток времени!");
    }
    

    
}
