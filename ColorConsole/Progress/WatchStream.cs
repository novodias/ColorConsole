namespace ColorConsole.Progress;

public class WatchStream : Stream, IDisposable
{
    private readonly Stream _stream;

    public override bool CanRead => _stream.CanRead;

    public override bool CanSeek => _stream.CanSeek;

    public override bool CanWrite => _stream.CanWrite;

    public override long Length => _stream.Length;

    public override long Position 
    { 
        get => _stream.Position; 
        set => _stream.Position = value; 
    }

    public delegate void OnLengthUpdatedHandler(object sender, StreamEventArgs e);

    public event OnLengthUpdatedHandler? OnLengthUpdate;

    public WatchStream(Stream stream)
        => this._stream = stream;

    public WatchStream(FileStream stream)
        => this._stream = stream;

    public WatchStream(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(nameof(path));

        this._stream = File.OpenWrite(path);
    }

    public static implicit operator WatchStream(FileStream stream)
        => new(stream);

    private void CreateEvent()
    {
        if (OnLengthUpdate != null)
            this.OnLengthUpdate(this, new StreamEventArgs(_stream.Length));
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        this._stream.Write(buffer, offset, count);

        CreateEvent();
    }

    public new async Task WriteAsync(byte[] buffer, int offset, int count)
    {
        await this._stream.WriteAsync(buffer, offset, count);

        CreateEvent();
    }

    public new async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        await this._stream.WriteAsync(buffer, offset, count, cancellationToken);

        CreateEvent();
    }

    public override void Flush()
    {
        _stream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _stream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _stream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _stream.SetLength(value);
    }
}

public class StreamEventArgs : EventArgs
{
    public readonly long Length;

    public StreamEventArgs(long length)
    {
        this.Length = length;
    }
}