using ColorConsole.Internal;

namespace ColorConsole
{
    public static partial class CConsole
    {
        public static string Read(string message, bool sameline = false)
        {
            return In.Read(message, sameline).Trim();
        }

        public static string ReadCondition(string message, Func<string, bool> condition, string error, bool sameline = false)
        {
            return In.Read(message, condition, error, sameline);
        }

        public static T ReadNumber<T>(string message, bool sameline = false, string? error = null) where T : notnull
        {
            return In.ReadNumber<T>(message, sameline, error);
        }

        public static T ReadNumber<T>(
            string message, IEnumerable<T> range, bool sameline = false, string? error = null) where T : notnull
        {
            return In.ReadNumber(message, range, sameline, error);

            // var result = default(T);
            // var any = false;
            // var parsed = false;
            // var context = new ContextIn(message, sameline, error);

            // context.ShowText();

            // while (!any && !parsed)
            // {
            //     parsed = In.TryReadNumber(context, out result);

            //     if (parsed)
            //         any = range.Any(n => n.Equals(result));

            //     if (any == false)
            //     {
            //         context.ShowText(true);
            //     }
            // }

            // _ = result ?? throw new NullReferenceException(nameof(parsed));

            // return result;
        }

        public static T ReadNumber<T>(
            string message, (T min, T max) between, bool sameline = false, string? error = null) where T : notnull
        {
            return In.ReadNumber(message, between, sameline, error);

            // var result = default(T);
            // var parsed = false;
            // var any = false;
            // var context = new ContextIn(message, sameline, error);

            // context.ShowText();
            // while (!any && !parsed)
            // {
            //     parsed = In.TryReadNumber(context, out result);

            //     if (result is not null)
            //     {
            //         if ( (dynamic)result >= (dynamic)between.min &&
            //             (dynamic)result <= (dynamic)between.max )
            //         {
            //             any = true;
            //         }
            //         else
            //             context.ShowText(true);
            //     }
            // }

            // _ = result ?? throw new NullReferenceException(nameof(parsed));

            // return result;
        }
    }
}