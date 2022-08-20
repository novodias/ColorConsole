using ColorConsole.Extensions;

namespace ColorConsole.Internal;

internal class ContextIn
{
    private Point2D _point;
    private readonly string _text;
    private readonly bool _sameline;
    private readonly string? _error;

    public ContextIn(string text, bool sameline = false, string? error = null)
    {
        _text = text;
        _sameline = sameline;
        _error = error;
        _point = GetPoint();
    }

    private Point2D GetPoint()
    {
        // Sum rows with + 1 because when the user presses enter,
        // it goes to a new line. So to avoid this, sum with 2.
        var amount = _text.GetRectangle().rows +
            (_error is not null ? _error.GetRectangle().rows : In.ErrorEmpty.GetRectangle().rows) + 1;

        var cursor = CConsole.GetLine(amount);

        return cursor;
    }

    private void WriteError(string error)
    {
        if (!_sameline)
        {
            CConsole.WriteLine((error?.WithSpaces() ?? "") + "\n" + _text.WithSpaces());
            CConsole.Write(" ".WithSpaces());

            CConsole.SetCursorLeft(0);

            var cursor = CConsole.GetCursor;
            if (cursor.Y == Console.WindowHeight - 1)
            {
                Console.WriteLine();
                _point.DecreaseTop();
                CConsole.SetCursorTop(cursor.Y - 1);
            }
        }
        else
        {
            CConsole.Write((error?.WithSpaces() ?? "") + "\n" + _text.WithSpaces());

            CConsole.SetCursorLeft(_text.Length);
        }
        
    }

    public void ShowText(bool error = false, string? errorempty = null)
    {
        CConsole.SetCursor(_point);

        if (error && _error is not null && errorempty is null)
            WriteError(_error);
        else if (error && errorempty is not null)
            WriteError(errorempty);
        else if (error && _error is null)
            WriteError("[red]ERROR;C");
        else
        {
            // This part shouldn't be called again, so we
            // don't care about setting the cursor everytime.
            if (!_sameline)
                CConsole.WriteLine(_text);
            else
                CConsole.Write(_text);
        }
    }
}