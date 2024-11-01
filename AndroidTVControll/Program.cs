// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using AndroidTVControll;

TcpListener server = new TcpListener(IPAddress.Any, 47777);
Console.WriteLine("start");
server.Start();
while (true)   //we wait for a connection
{
    Console.WriteLine("wait client");
    TcpClient client = server.AcceptTcpClient();
    NetworkStream ns = client.GetStream();
    Console.WriteLine("Connected");
    byte[] msg = new byte[1024];
    var bytesReceive = ns.Read(msg, 0, msg.Length);
    var str = Encoding.Default.GetString(msg, 0, bytesReceive);
    Console.WriteLine(str);
    switch (str)
    {
        case "UpTv":
            UpTV();
            break;
        case "DownTv":
            DownTV();
            break;
    }
    var hello = Encoding.Default.GetBytes("Ok");
    ns.Write(hello, 0, hello.Length);
    client.Close();
}

async void  UpTV()
{
    await WOLCLient.WakeOnLan("48:87:B8:A0:DE:E8");
    Adb.SendConnect("192.168.1.121");
    Adb.SendHdmiEnabled();
}

void DownTV()
{
    Adb.SendConnect("192.168.1.121");
    Adb.SendDown();
}