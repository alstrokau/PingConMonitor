namespace PingConMonitor.DataStructures
{
    internal class LastTimes
    {
        private Dictionary<long, DateTime> lastTimes = new();

        public void Update(long duration)
        {

            if (duration == 0)
            {
                lastTimes[0] = DateTime.Now;
            }

            foreach (var key in lastTimes.Keys)
            {
                if (key == 0)
                    continue;

                if (duration >= key)
                {
                    lastTimes[key] = DateTime.Now;
                }
            }
        }

        public LastTimes()
        {
            lastTimes = new Dictionary<long, DateTime>()
            {
                { 0, DateTime.Now },
                { 25, DateTime.Now },
                { 50, DateTime.Now },
                { 75, DateTime.Now },
                { 100, DateTime.Now },
                { 200, DateTime.Now },
                { 500, DateTime.Now },
                { 1000, DateTime.Now },
                { 1500, DateTime.Now },
                { 2000, DateTime.Now},
                { 3000, DateTime.Now }
            };
        }

        public void ShowAll()
        {
            double totalSeconds;

            foreach (var key in lastTimes.Keys)
            {
                totalSeconds = (DateTime.Now - lastTimes[key]).TotalSeconds;

                Console.ForegroundColor = totalSeconds switch
                {
                    > 30 => ConsoleColor.DarkGreen,
                    > 10 => ConsoleColor.Green,
                    > 5 => ConsoleColor.Yellow,
                    _ => ConsoleColor.Red,

                };

                Console.Write($"{key}:{SplitTime(totalSeconds)} | ");
            }

        }

        private string SplitTime(double timeSpan) =>
            timeSpan switch
            {
                < 60 => Math.Round(timeSpan) + "s",
                < 3600 => Math.Round(timeSpan / 60) + "+ m",
                < 86400 => Math.Round(timeSpan / 3600) + "+ h",
                _ => Math.Round(timeSpan / 86400) + "+ d"
            };
    }
}
