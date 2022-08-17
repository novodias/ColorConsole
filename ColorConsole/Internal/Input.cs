using System.Text;

namespace ColorConsole.Internal;

internal static class Input
{
    private static readonly HashSet<Type> _numericTypes = new()
        {
            typeof(byte), typeof(sbyte), typeof(ushort),
            typeof(uint), typeof(ulong), typeof(short),
            typeof(int), typeof(long), typeof(decimal),
            typeof(double), typeof(float)
        };

    private static IReadOnlyList<ValueInfo<T>> GetValueInfoList<T>(
        IReadOnlyList<T> choices, IReadOnlyList<string>? messages = null)
    {
        var count = choices.Count;
        var list = new List<ValueInfo<T>>(count);
        var format = "&[[green]{0};&] - {1}";

        foreach (var current_index in Enumerable.Range(0, count))
        {
            var value = choices[current_index];
            var number = current_index + 1;
            var information = messages != null ?
                messages[current_index] : string.Format(format, current_index + 1, value);

            var valueInfo = new ValueInfo<T>()
            {
                Value = value,
                Information = information,
                Number = number
            };

            list.Add(valueInfo);
        }

        return list;
    }

    internal static ValueInfo<T> InternalSelect<T>(
        string message, IReadOnlyList<T> choices, IReadOnlyList<string>? messages = null, bool keyselectable = false)
    {
        if (!choices.Any())
            throw new ArgumentOutOfRangeException(nameof(choices));

        var internals = GetValueInfoList(choices, messages);

        if (keyselectable)
        {
            var help = "Use as teclas setas UP e DOWN ou W e S para selecionar\n";
            message = help + message;

            CConsole.WriteLine(message);
            return new Selection<T>(internals).Init();
        }
        else
        {
            var informations = internals.Select(ctx => ctx.Information);
            var selection = new StringBuilder();

            foreach (var line in informations)
                selection.AppendLine(line);

            var numbers = internals.Select(ctx => ctx.Number);

            int parsed = CConsole.ReadNumber(
                message + "\n" + selection + "\n",
                numbers
            );

            var info = internals.First(ctx => ctx.Number == parsed);

            return info;
        }
    }

    internal static bool InternalTryParse<T>(string unparsed, out T? result)
    {
        var type = typeof(T);

        if (!_numericTypes.Any(t => t.Equals(type)))
            throw new Exception("Type inserted is not a number");

        var method = type.GetMethods()
            .Where(m => m.Name == "TryParse")
            .FirstOrDefault(x => x.IsStatic);

#pragma warning disable
        var parameters = new object[] { unparsed, null };
        var success = (bool)method.Invoke(null, parameters);

        if (success)
            result = (T)parameters[1];
        else
            result = default;
#pragma warning restore

        // var method = type.GetMethods()
        //     .Where(m => m.Name == "Parse")
        //     .FirstOrDefault(x => x.IsStatic);

        // result = (T)method.Invoke(obj: null, parameters: new object[] { unparsed });

        return success;
    }
}