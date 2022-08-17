namespace ColorConsole.Internal;

internal static class In
{
    private static void Show(string message, bool sameline, string? error = null)
    {
        var msg = (error != null ? error + Environment.NewLine : string.Empty) + message;
            if (sameline)
                Out.Write(msg);
            else 
                Out.WriteLine(msg);
    }

    private static void ClearLine(Point2D point)
    {
        CConsole.SetCursor(point);
        Out.Write(new string(' ', Console.WindowWidth));
        CConsole.SetCursor(point);
    }

    internal static string Read(string message, bool sameline = false, string? error = null)
    {
        string? result = default;
        bool resume = false;

        var cursor = CConsole.GetCursor;

        Show(message, sameline);

        while (!resume)
        {
            result = Console.ReadLine();
            resume = result != null;

            if (!resume)
            {
                ClearLine(cursor);
                Show(message, sameline, error);
            }
        }

        _ = result ?? throw new NullReferenceException(nameof(result));

        return result;
    }

    internal static string Read(string message, Func<string, bool> condition, string conditionerror, bool sameline = false, string? error = null)
    {
        string? result = default;
        bool resume = false;

        var cursor = CConsole.GetCursor;

        Show(message, sameline);

        while (!resume)
        {
            result = Console.ReadLine();

            if (result != null && !condition.Invoke(result))
            {
                ClearLine(cursor);
                Show(message, sameline, conditionerror);
            }
            else if (result == null)
            {
                ClearLine(cursor);
                Show(message, sameline, error);
            }
            else
                resume = true;
        }

        _ = result ?? throw new NullReferenceException(nameof(result));

        return result;
    }

    internal static T ReadNumber<T>(string message, bool sameline = false, string? error = null)
    {
        bool isDefault = false;
        T? number = default;

        while (!isDefault)
        {
            var unparsed = Read(message, sameline, error);
            number = Input.InternalTryParse<T>(unparsed);

            isDefault = number != null;
        }

        _ = number ?? throw new NullReferenceException(nameof(number));

        return number;
    }
}