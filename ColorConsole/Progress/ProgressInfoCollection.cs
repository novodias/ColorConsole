namespace ColorConsole.Progress;

public class ProgressInfoCollection
{
    private readonly IList<ProgressInfo> _listProgress;
    private static readonly object _listLock = new();
    private Point2D _cursor;
    private bool _started = false;
    
    public bool IsCompleted 
    {
        get => _listProgress.All(p => p.IsCompleted);
    }
    public int Count 
    { 
        get => _listProgress.Count; 
    } 

    public ProgressInfoCollection()
    {
        _cursor = CConsole.GetCursor;
        _listProgress = new List<ProgressInfo>();
    }

    public ProgressInfoCollection(int capacity)
    {
        _cursor = CConsole.SetUpCursor(capacity);
        _listProgress = new List<ProgressInfo>(capacity);
    }

    public void Update(int index, int value)
    {
        if (!_started)
            _started = true;

        var l = _listProgress[index].Length + value;

        lock (_listLock)
        {
            _listProgress[index].Report(l);
        }
    }

    public async Task StartCopyToAsync(int index, Stream source, WatchStream destination)
    {
        if (!_started)
            _started = true;

        destination.OnLengthUpdate += (s, e) =>
        {
            lock(_listLock)
            {
                _listProgress[index].Report(e.Length);
            }
        };

        await source.CopyToAsync(destination)
            .ConfigureAwait(false);
    }

    private void ClearProgress()
    {
        string space = new(' ', Console.WindowWidth);
        for (int i = 0; i < _listProgress.Count; i++)
        {
            Console.CursorTop = _listProgress[i].Position.Y;
            Console.Write(space);
        }
    }   

    public void Add(long length, string? text = null)
    {
        if (_cursor.Y == Console.WindowHeight)
        {
            lock (_listLock)
            {
                if (_started)
                    ClearProgress();
                
                for (int i = 0; i < Count; i++)
                    _listProgress[i].DecreaseTopByOne();

                Console.Write(Environment.NewLine);
                _cursor.Y--;
            }
        }

        var info = text is null ?
            new ProgressInfo(_cursor, length) :
            new ProgressInfo(_cursor, length, text);

        _listProgress.Add(info);

        _cursor.Y++;
    }

    public void Add(long length, Point2D pos, string? text = null)
    {
        var info = text is null ?
            new ProgressInfo(pos, length) :
            new ProgressInfo(pos, length, text);

        _listProgress.Add(info);

    }

}
