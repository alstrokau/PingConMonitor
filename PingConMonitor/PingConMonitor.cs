using System.Net.NetworkInformation;
using PingConMonitor.DataStructures;

Console.WriteLine("Hello, Ping!\nv0.0");

const string Address = "8.8.8.8";

Ping ping = new();
List<PingPoint> points = [];
LastTimes lastTimes = new();
Console.CursorVisible = false;
bool exitRequest = false;
int sleepDuration = 500;

while (!exitRequest)
{
    PingReply reply = ping.Send(Address);
    points.Add(new PingPoint(reply.RoundtripTime));
    lastTimes.Update(reply.RoundtripTime);

    ColorizeByReplyTime(reply);

    Console.Write($"{reply.Status}: {reply.RoundtripTime}\t");
    lastTimes.ShowAll();
    Console.WriteLine();

    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo key = Console.ReadKey();
        Console.ForegroundColor = ConsoleColor.Blue;

        if (key.KeyChar == 'q')
        {
            exitRequest = true;
        }

        if (key.Key == ConsoleKey.UpArrow)
        {
            sleepDuration += sleepDuration < 100 ? 25 : 100;
            Console.WriteLine($"SleepDuration: {sleepDuration}");
        }
        else if (key.Key == ConsoleKey.DownArrow)
        {
            sleepDuration -= sleepDuration > 100 ? 100 : 25;
            sleepDuration = Math.Max(sleepDuration, 25);
            Console.WriteLine($"SleepDuration: {sleepDuration}");
        }
    }

    Thread.Sleep(sleepDuration);
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