using System.Diagnostics;

namespace AndroidTVControll;

public class Adb
{
    private static string _adbPath = @"adb";
    public static void SendHdmiEnabled()
    {
        var command =
            "shell am start -a android.intent.action.VIEW -d content://android.media.tv/passthrough/com.tcl.tvinput%2F.TvPassThroughService%2FHW16";
        var log = AdbSend(command);
        if (log.Contains("offline"))
        {
            Thread.Sleep(500);
            SendHdmiEnabled();
        }
    }

    public static void SendConnect(string ip)
    {
        var command =
            $"connect {ip}";
        AdbSend(command);
    }

    public static void SendDown()
    {
        var command =
            $"shell input keyevent 26";
        AdbSend(command);
    }

    private static string AdbSend(string command)
    {
        var procInfo = new ProcessStartInfo(_adbPath, command);
        string log = "";
        procInfo.UseShellExecute = false;
        procInfo.CreateNoWindow = true;
        procInfo.RedirectStandardOutput = true;
        procInfo.RedirectStandardError = true;
        var process = new Process();
        process.StartInfo = procInfo;
        process.ErrorDataReceived += (sender, args) =>
        {
            log += args.Data;
        };
        process.OutputDataReceived += (sender, args) =>
        {
            log += args.Data;
        };


        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        Console.WriteLine(log);
        return log;
    }
}