using ColorConsole.Internal;

namespace ColorConsole
{
    public static partial class CConsole
    {
        public static string Read(string message, bool sameline = false, string? errorempty = null)
        {
            return In.Read(message, sameline, errorempty).Trim();
        }

        public static string Read(string message, Func<string, bool> condition, string error, bool sameline = false, string? errorempty = null)
        {
            return In.Read(message, condition, error, sameline, errorempty);
        }

        public static T ReadNumber<T>(string message, bool sameline = false, string? error = null) where T : notnull
        {
            return In.ReadNumber<T>(message, sameline, error);
        }

        public static T ReadNumber<T>(string message, IEnumerable<T> range, bool sameline = false, string? errormessage = null) where T : notnull
        {
            var parsed = default(T);
            bool any = false;

            while (!any && parsed is not null)
            {
                parsed = ReadNumber<T>(message, sameline);
                any = range.Contains(parsed);

                if (!any && errormessage is not null)
                    Write(errormessage);
            }

            _ = parsed ?? throw new NullReferenceException(nameof(parsed));

            return parsed;
        }

        public static T ReadNumber<T>(string message, (T min, T max) between, bool sameline = false) where T : notnull
        {
            var parsed = default(T);
            bool any = false;

            while (!any && parsed is not null)
            {
                parsed = ReadNumber<T>(message, sameline);

                if ( (dynamic)parsed >= (dynamic)between.min &&
                     (dynamic)parsed <= (dynamic)between.max )
                    any = true;
            }

            _ = parsed ?? throw new NullReferenceException(nameof(parsed));

            return parsed;
        }
    }
}