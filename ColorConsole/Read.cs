namespace ColorConsole
{
    public static partial class CConsole
    {
        public static string Read(string message, string errormessage = "", bool sameline = false)
        {
            string output = string.Empty;
            void Show()
            {
                if ( sameline )
                    Write(message);
                else
                    WriteLine(message);
            }

            Show();

            bool resume = false;
            while (!resume)
            {
                output = Console.ReadLine() ?? string.Empty;
                resume = !output.Equals(string.Empty);

                if (!resume)
                    WriteLine(errormessage);
            }

            return output.Trim();
        }

        public static string Read(string message, string errormessage, Func<string, bool> condition)
        {
            string output = string.Empty;
            void Show() => WriteLine(message);

            Show();

            bool resume = false;
            while (!resume)
            {
                output = Console.ReadLine() ?? string.Empty;
                resume = !output.Equals(string.Empty) && condition.Invoke(output);

                if (!resume)
                    WriteLine(errormessage);
            }

            return output.Trim();
        }

        public static T ReadNumber<T>(string message) where T : notnull
        {
            return Input.InternalReadNumber<T>(message);
        }

        public static T ReadNumber<T>(string message, IEnumerable<T> range, string errormessage = "", bool sameline = false) where T : notnull
        {
            var parsed = default(T);
            bool any = false;

            while (!any && parsed is not null)
            {
                parsed = Input.InternalReadNumber<T>(message, sameline);
                any = range.Any(num => num.Equals(parsed));

                if (!any && errormessage != string.Empty)
                    if (sameline)
                        Write(errormessage);
                    else
                        WriteLine(errormessage);
            }

            if (parsed is null)
                throw new NullReferenceException(nameof(parsed));

            return parsed;
        }

        public static T ReadNumber<T>(string message, (T min, T max) between, bool sameline = false) where T : notnull
        {
            var parsed = default(T);
            bool any = false;

            while (!any && parsed is not null)
            {
                parsed = Input.InternalReadNumber<T>(message, sameline);
                if ( (dynamic)parsed >= (dynamic)between.min &&
                     (dynamic)parsed <= (dynamic)between.max )
                    any = true;
            }

            if (parsed is null)
                throw new NullReferenceException(nameof(parsed));

            return parsed;
        }
    }
}