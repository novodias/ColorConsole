using ColorConsole.Extensions;

namespace ColorConsole.Progress;
    
internal static class Progress
{
    internal static readonly TimeSpan Interval = TimeSpan.FromSeconds(1 / 4); // TimeSpan.FromSeconds(1 / 4)
    private static readonly object _progressLock = new();

    internal static void Report(ProgressInfo info, double? rate = default)
    {
        var percentage = (int)Math.Round((double)(100 * info.Length) / info.FullLength);
        
        if ( percentage > 100 )
            return;

        string progress = GetProgressBar(percentage);

        WriteColor(progress, percentage, info, rate);
    }

    private static void WriteColor(string progress, int percentage, ProgressInfo info, double? rate = default)
    {
        var now = DateTime.Now.TimeOfDay;

        string text = string.Empty;

        if ( info.Text is not null )
            text += info.Text + " ";

        text += "&[";
        
        if (progress.Contains('#'))
        {
            progress = progress.Insert(0, "[c green, green]");
            progress = progress.Insert(progress.LastIndexOf('#') + 1, ";C");
        }

        var p = percentage < 10 ? $"  {percentage}" :
                percentage < 100 ? $" {percentage}" : 
                                    $"{percentage}";
        
        var timer = $"{now.Subtract(info.CreatedAt):hh\\:mm\\:ss}";

        text += progress + "&]" + 
        $" &[[green]{p};%&]" + 
        $" &[[yellow]{timer};&]";

        // TODO -> DOWNLOAD SPEED PER SECOND

        lock (_progressLock)
        {
            CConsole.SetCursor(info.Position);
            CConsole.Write(text.WithSpaces());
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
                progressBar += ' ';

            percentage -= 5;
        }
        return progressBar;
    }
}