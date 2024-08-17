using System.Net.NetworkInformation;
using PingConMonitor.DataStructures;

const string Address = "8.8.8.8";

Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Gray;
Console.Clear();
Console.WriteLine("PingConMonitor!\nv0.2");

Ping ping = new();
List<PingPoint> points = [];
LastTimes lastTimes = new();
Console.CursorVisible = false;
bool exitRequest = false;
int sleepDuration = 500;
int timeoutsCount = 0;
DateTime startTime = DateTime.Now;

while (!exitRequest)
{
    PingReply reply = ping.Send(Address);
    points.Add(new PingPoint(reply.RoundtripTime));
    lastTimes.Update(reply.RoundtripTime);

    ColorizeByReplyTime(reply);

    Console.Write($"{reply.Status}: {reply.RoundtripTime}\t");
    if (reply.Status == IPStatus.TimedOut)
    {
        Console.WriteLine($"\nTimeout counts: {++timeoutsCount} in {DateTime.Now - startTime}");
    }

    lastTimes.ShowAll(shortView: Console.WindowWidth <= 100);

    Console.WriteLine();

    ProcessKeys(ref exitRequest, ref sleepDuration);

    Thread.Sleep(sleepDuration);
}

static void ColorizeByReplyTime(PingReply reply)
{
    Console.ForegroundColor = reply.RoundtripTime switch
    {
        > 1500 => ConsoleColor.Red,
        > 250 => ConsoleColor.Yellow,
        > 100 => ConsoleColor.Green,
        0 => ConsoleColor.Black,
        _ => ConsoleColor.DarkGreen,
    };

    Console.BackgroundColor = reply.Status switch
    {
        IPStatus.TimedOut => ConsoleColor.Red,
        _ => ConsoleColor.Black,
    };
}

static void ProcessKeys(ref bool exitRequest, ref int sleepDuration)
{
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
}