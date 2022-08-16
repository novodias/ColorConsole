namespace ColorConsole.Progress;

public class ProgressInfo
{
    public readonly TimeSpan CreatedAt = DateTime.Now.TimeOfDay;
    public TimeSpan? Timer { get; private set; } = null;
    public Point2D Position { get; private set; }
    public string? Text { get; init; } = null;
    public long FullLength { get; init; }
    public long Length { get; set; } = 0;
    public bool IsCompleted 
    { 
        get 
        {
            if (Length >= FullLength)
                return true;

            return false;
        }
    }

    public ProgressInfo(long length)
    {
        this.Position = CConsole.SetUpCursor(1);
        this.FullLength = length;
    }

    public ProgressInfo(long length, string text)
    {
        this.Position = CConsole.SetUpCursor(1);
        this.FullLength = length;
        this.Text = text;
    }

    public ProgressInfo(Point2D position, long length)
    {
        this.FullLength = length;
        this.Position = position;
    }

    public ProgressInfo(Point2D position, long length, string text)
    {
        this.FullLength = length;
        this.Position = position;
        this.Text = text;
    }

    public ProgressInfo((int left, int top) position, long length)
    {
        this.FullLength = length;
        this.Position = position;
    }

    public ProgressInfo((int left, int top) position, long length, string text)
    {
        this.FullLength = length;
        this.Position = position;
        this.Text = text;
    }

    public void Report(long value)
    {
        Length = value;
        var now = DateTime.Now.TimeOfDay;
        var rate = Length / 1024 * 1024 / now.Subtract(CreatedAt).TotalSeconds;

        if ( Timer is null ) 
        {
            SetTimer(now, Progress.Interval);
            return;
        }
        
        if ( now <= Timer )
            return;

        Progress.Report(this, rate);
        SetTimer(now, Progress.Interval);
    }

    public void DecreaseTopByOne() 
        => Position = (Position.X, Position.Y - 1);

    private void SetTimer(TimeSpan now, TimeSpan interval) => this.Timer = now + interval;
}