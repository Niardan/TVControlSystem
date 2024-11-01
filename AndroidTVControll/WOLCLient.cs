using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace AndroidTVControll;

public class WOLCLient
{
    public static async Task WakeOnLan(string macAddress)
    {
        byte[] magicPacket = BuildMagicPacket(macAddress);

       Console.WriteLine($"Waking on MAC: {macAddress}");
        
        try
        {
            using (var client = new UdpClient())
            {
                client.EnableBroadcast = true;
                await client.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(new IPAddress(0xffffffff), 7));
            }
        }
        catch (Exception ex)
        {
             Console.WriteLine($"Exception during wake on lan request. {ex}");
            throw;
        }
    }


    static byte[] BuildMagicPacket(string macAddress) // MacAddress in any standard HEX format
    {
        macAddress = Regex.Replace(macAddress, "[: -]", "");
        byte[] macBytes = Convert.FromHexString(macAddress);

        IEnumerable<byte> header = Enumerable.Repeat((byte)0xff, 6); //First 6 times 0xff
        IEnumerable<byte> data = Enumerable.Repeat(macBytes, 16).SelectMany(m => m); // then 16 times MacAddress
        return header.Concat(data).ToArray();
    }

    static async Task SendWakeOnLan(IPAddress localIpAddress, IPAddress multicastIpAddress, byte[] magicPacket)
    {
        Console.WriteLine($"localAdd: {localIpAddress.ToString()}, multicastIpAddress: {multicastIpAddress.ToString()}");
        using UdpClient client = new(new IPEndPoint(localIpAddress, 0));
        await client.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(multicastIpAddress, 9));
    }
}