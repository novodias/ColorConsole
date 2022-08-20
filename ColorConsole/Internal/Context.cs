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
        // it goes to a new line. So to avoid this sum with 1.
        var amount = _text.GetRectangle().rows +
            (_error is not null ? _error.GetRectangle().rows : 0) + 1;

        // Same with writeline
        if (!_sameline) amount += 1;

        var cursor = CConsole.GetLine(amount);

        return cursor;
    }

    private void WriteError(string error)
    {
        if (!_sameline)
        {
            CConsole.WriteLine((error?.WithSpaces() ?? "") + "\n" + _text.WithSpaces());
            CConsole.Write(" ".WithSpaces());

            CConsole.SetCursor(0, CConsole.GetCursor.Y);
        }
        else
        {
            CConsole.Write((error?.WithSpaces() ?? "") + "\n" + _text.WithSpaces());

            CConsole.SetCursor(_text.Length, CConsole.GetCursor.Y);
        }
    }

    public void ShowText(bool error = false, string? errorempty = null)
    {
        CConsole.SetCursor(_point);

        if (error && _error is not null && errorempty is null)
            WriteError(_error);
        else if (error && errorempty is not null)
            WriteError(errorempty);
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