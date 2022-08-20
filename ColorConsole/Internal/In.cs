
namespace ColorConsole.Internal;

internal static class In
{
    private static string? _errorEmpty = null;
    internal static string ErrorEmpty
    {
        get => _errorEmpty is null ? "Error" : _errorEmpty;
        set => _errorEmpty = value;
    }

    private static bool TryRead(out string text)
    {
        text = Console.ReadLine() ?? string.Empty;
        return text != string.Empty;
    }

    private static bool TryRead(ContextIn ctx, out string text)
    {
        text = Console.ReadLine() ?? string.Empty;

        if (text == string.Empty)
            ctx.ShowText(true, ErrorEmpty);

        return text != string.Empty;
    }

    private static bool TryReadNumber<T>(ContextIn ctx, out T? number)
    {
        bool tRead = TryRead(out string text);
        
        if (tRead)
        {
            var tNum = Input.InternalTryParse(text, out number);
            return tRead && tNum;
        }
        else
            ctx.ShowText(true, ErrorEmpty);
        

        number = default;
        return tRead;
    }

    internal static string Read(string message, bool sameline = false)
    {
        string? result = default;
        bool resume = false;
        var context = new ContextIn(message, sameline);

        context.ShowText();

        while (!resume)
        {
            resume = TryRead(context, out result);
        }

        _ = result ?? throw new NullReferenceException(nameof(result));

        return result;
    }

    internal static string Read(string message, Func<string, bool> condition, string conditionerror, bool sameline = false)
    {
        string? result = default;
        bool resume = false;
        var context = new ContextIn(message, sameline, conditionerror);

        context.ShowText();

        while (!resume)
        {
            TryRead(context, out result);
            var bCondition = condition.Invoke(result);

            if (result != string.Empty && !bCondition)
                context.ShowText(true);
            else if (result != string.Empty && bCondition)
                resume = true;
        }

        _ = result ?? throw new NullReferenceException(nameof(result));

        return result;
    }

    internal static T ReadNumber<T>(string message, bool sameline = false, string? error = null)
    {
        T? number = default;
        bool parsed = false;
        var context = new ContextIn(message, sameline, error);

        context.ShowText();

        while (!parsed)
        {
            if (TryRead(context, out string unparsed))
            {
                var bParsed = Input.InternalTryParse(unparsed, out number);

                if (bParsed)
                    parsed = true;
                else
                    context.ShowText(true);
            }
        }

        _ = number ?? throw new NullReferenceException(nameof(number));

        return number;
    }

    internal static T ReadNumber<T>(
        string message,
        IEnumerable<T> range,
        bool sameline = false,
        string? error = null) where T : notnull
    {
        var result = default(T);
        var any = false;
        var parsed = false;
        var context = new ContextIn(message, sameline, error);

        context.ShowText();

        while (!any || !parsed)
        {
            parsed = TryReadNumber(context, out result);

            if (parsed)
            {
                any = range.Any(n => n.Equals(result));

                if (any == false)
                    context.ShowText(true);
            }

        }

        _ = result ?? throw new NullReferenceException(nameof(parsed));

        return result;
    }

    internal static T ReadNumber<T>(
        string message,
        (T min, T max) between,
        bool sameline = false, 
        string? error = null) where T : notnull
    {
        var result = default(T);
        var parsed = false;
        var any = false;
        var context = new ContextIn(message, sameline, error);

        context.ShowText();
        while (!any || !parsed)
        {
            parsed = TryReadNumber(context, out result);

            if (parsed && result is not null)
            {
                if ( (dynamic)result >= (dynamic)between.min &&
                    (dynamic)result <= (dynamic)between.max )
                    any = true;
                else
                    context.ShowText(true);
            }
        }

        _ = result ?? throw new NullReferenceException(nameof(parsed));

        return result;
    }
}