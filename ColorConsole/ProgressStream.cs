namespace ColorConsole
{
    public static class ProgressStream
    {
        public static void CopyToWithProgress(this Stream source, Stream destination, ProgressInfo info) 
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));

            var buffer = new byte[81920];
            int bytesRead;
            while ((bytesRead = source.Read(buffer)) != 0) 
            {
                destination.Write(buffer, 0, bytesRead);

                info = Progress.Report(info, destination.Length);
            }
        }

        public static async Task CopyToAsyncWithProgress(this Stream source, Stream destination, ProgressInfo info, CancellationToken cancellationToken = default) 
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));

            var buffer = new byte[81920];
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0) 
            {
                await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);

                info = Progress.Report(info, destination.Length);
            }
        }
    }
}