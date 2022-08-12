using System.Text;

namespace ColorConsole
{
    internal static class Input
    {
        private static readonly HashSet<Type> _numericTypes = new()
        {
            typeof(byte), typeof(sbyte), typeof(ushort),
            typeof(uint), typeof(ulong), typeof(short),
            typeof(int), typeof(long), typeof(decimal),
            typeof(double), typeof(float)
        };

        internal static (int Rows, int Columns) GetRectangle(this string text)
        {
            int rows = 0, columns = 0;
            var sr = new StringReader(text);

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if ( line.Length > columns )
                    columns = line.Length;

                rows++;
            }

            sr.Dispose();
            return (rows, columns);
        }

        private static IEnumerable<ValueInfo<T>> GetChoicesEnumerated<T>(
            IEnumerable<T> choices, IEnumerable<string>? messages = null)
        {
            var count = choices.Count();
            var order = new List<ValueInfo<T>>(count);
            var format = "&[[green]{0};&] - {1}";
            var selection_number = 1;

            if (messages == null)
            {
                foreach (var choice in choices)
                {
                    var internalInfo = new ValueInfo<T>()
                    {
                        Value = choice,
                        Information = string.Format(format, selection_number, choice),
                        Number = selection_number
                    };

                    order.Add(internalInfo);
                    selection_number++;
                }
            }
            else
            {
                if ( messages.Count() != count )
                    throw new Exception(nameof(messages) + " doesn't have the same length of " + nameof(choices));

                foreach (var num in Enumerable.Range(0, count))
                {
                    var value = choices.ElementAt(num);
                    var information = messages.ElementAt(num);

                    var internalInfo = new ValueInfo<T>()
                    {
                        Value = value,
                        Information = string.Format(format, selection_number, information),
                        Number = selection_number
                    };

                    order.Add(internalInfo);
                    selection_number++;
                }
            }

            return order;
        }

        internal static ValueInfo<T> InternalSelect<T>(
            string message, IEnumerable<T> choices, IEnumerable<string>? messages = null, bool keyselectable = false)
        {
            if (!choices.Any())
                throw new ArgumentOutOfRangeException(nameof(choices));

            var help = "Use as teclas setas UP e DOWN ou W e S para selecionar\n";
            var localMessage = keyselectable ? help + message : message;
            var internals = GetChoicesEnumerated(choices, messages);

            if (keyselectable)
            {
                CConsole.WriteLine(localMessage);
                return new Selection<T>(internals).Init();
            }
            else
            {
                var informations = internals.Select(ctx => ctx.Information);
                string selection = new StringBuilder()
                    .AppendJoin("\n", informations).ToString();

                var numbers = internals.Select(ctx => ctx.Number);
                
                int parsed = CConsole.ReadNumber(
                    localMessage + "\n" + selection + "\n",
                    numbers
                );

                var info = internals.First(ctx => ctx.Number == parsed);

                return info;
            }
        }


        internal static T InternalReadNumber<T>(string message, bool sameline = false)
        {
            var parsed = default(T);
            var type = typeof(T);

            if ( !_numericTypes.Any(t => t.Equals(type)) )
                throw new Exception("Type inserido não é um número");

            var method = type.GetMethods()
                .Where(m => m.Name == "Parse")
                .FirstOrDefault(x => x.IsStatic);

            #pragma warning disable
            while (parsed.Equals(default(T)))
            {
                var unparsed = CConsole.Read(message, "", sameline);
                try
                {
                    parsed = (T)method.Invoke(obj: null, parameters: new object[] { unparsed }) 
                        ?? throw new NullReferenceException("parsed");
                }
                catch (System.Exception)
                {
                    parsed = default(T);
                }
            }
            #pragma warning restore

            return parsed;
        }
    }
}