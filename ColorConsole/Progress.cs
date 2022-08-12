using System;

namespace ColorConsole
{
    public static class Progress
    {
        private static readonly object _progressLock = new();
        private static readonly TimeSpan _interval = TimeSpan.FromSeconds(1); // TimeSpan.FromSeconds(1 / 4)

        public static ProgressInfo Report(ProgressInfo info, long value)
        {
            var now = DateTime.Now.TimeOfDay;
            if ( info.Timer is null ) info.SetTimer(now, _interval);
            if ( now <= info.Timer ) return info;
            
            var percentage = (int)Math.Round((double)(100 * value) / info.Length);
            if ( percentage > 100 )
                return info;

            string progress = GetProgressBar(percentage);

            WriteColor(progress, percentage, info);

            return info;
        }

        private static void WriteColor(string progress, int percentage, ProgressInfo info)
        {
            var now = DateTime.Now.TimeOfDay;
            var consolePosition = (Console.CursorLeft, Console.CursorTop);

            lock (_progressLock)
            {
                if ( info.Position != consolePosition )
                    Console.SetCursorPosition(info.Position.x, info.Position.y);

                if ( info.Text is not null )
                    CConsole.Write(info.Text + " ");

                Console.Write('[');
                for (int i = 0; i < progress.Length; i++)
                {
                    if ( progress[i] == '#' )
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.Write(progress[i]);
                        Console.ResetColor();
                    }
                    else
                        Console.Write(progress[i]);
                }
                Console.Write($"]");
                CConsole.Write($" &[[green]{percentage};%&] ");
                CConsole.Write($"&[[yellow]{now.Subtract(info.TimeComparison):hh\\:mm\\:ss};&]");

                // Console.Write('[');
                // Console.ForegroundColor = ConsoleColor.Yellow;
                // Console.Write($"{now.Subtract(info.TimeComparison):hh\\:mm\\:ss}");
                // Console.ResetColor();
                // Console.Write($"]");
            }
        }

        private static string GetProgressBar(int percentage)
        {
            string progressBar = string.Empty;
            for (int i = 0; i < 20; i++)
            {
                if ( percentage > 0 )
                    progressBar += '#';
                else
                    progressBar += '_';

                percentage -= 5;
            }
            return progressBar;
        }
    }

    public struct ProgressInfo
    {
        public readonly TimeSpan TimeComparison = DateTime.Now.TimeOfDay;
        public TimeSpan? Timer { get; private set; } = null;
        public string? Text { get; init; } = null;
        public (int x, int y) Position { get; init; }
        public long Length { get; init; }

        public ProgressInfo((int left, int top) position, long length)
        {
            this.Length = length;
            this.Position = position;
        }

        public ProgressInfo((int left, int top) position, long length, string text)
        {
            this.Length = length;
            this.Position = position;
            this.Text = text;
        }

        public void SetTimer(TimeSpan now, TimeSpan interval) => this.Timer = now + interval;
    }
}