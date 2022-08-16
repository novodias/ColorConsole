using ColorConsole.Progress;

namespace ColorConsole.Extensions;

public static class StreamExtension
{

    public static void CopyToWithProgress(this Stream source, WatchStream destination, ProgressInfo info) 
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (!source.CanRead)
            throw new ArgumentException("Has to be readable", nameof(source));
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));
        if (!destination.CanWrite)
            throw new ArgumentException("Has to be writable", nameof(destination));

        destination.OnLengthUpdate += (s, e) =>
        {
            info.Report(e.Length);
        };

        var buffer = new byte[81920];
        int bytesRead;
        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) != 0) 
        {
            destination.Write(buffer, 0, bytesRead);
        }
    }

    public static async Task CopyToAsyncWithProgress(this Stream source, WatchStream destination, ProgressInfo info, CancellationToken cancellationToken = default) 
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (!source.CanRead)
            throw new ArgumentException("Has to be readable", nameof(source));
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));
        if (!destination.CanWrite)
            throw new ArgumentException("Has to be writable", nameof(destination));

        destination.OnLengthUpdate += (s, e) =>
        {
            info.Report(e.Length);
        };

        var buffer = new byte[81920];
        int bytesRead;
        while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0) 
        {
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
        }
    }
}
