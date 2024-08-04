using System.Net.NetworkInformation;
using PingConMonitor.DataStructures;

Console.WriteLine("Hello, Ping!\nv0.0");

const string Address = "8.8.8.8";

Ping ping = new();
List<PingPoint> points = [];
LastTimes lastTimes = new();
Console.CursorVisible = false;

while (!Console.KeyAvailable)
{
    PingReply reply = ping.Send(Address);
    points.Add(new PingPoint(reply.RoundtripTime));
    lastTimes.Update(reply.RoundtripTime);

    ColorizeByReplyTime(reply);

    Console.Write($"{reply.Status}: {reply.RoundtripTime}\t");
    lastTimes.ShowAll();
    Thread.Sleep(500);
    Console.WriteLine();
}

static void ColorizeByReplyTime(PingReply reply)
{
    Console.ForegroundColor = reply.RoundtripTime switch
    {
        > 1500 => ConsoleColor.Red,
        > 250 => ConsoleColor.Yellow,
        > 100 => ConsoleColor.Green,
        _ => ConsoleColor.DarkGreen,
    };

    Console.BackgroundColor = reply.Status switch
    {
        IPStatus.TimedOut => ConsoleColor.DarkGray,
        _ => ConsoleColor.Black,
    };
}