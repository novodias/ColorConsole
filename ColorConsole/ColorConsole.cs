using ColorConsole.Internal;

namespace ColorConsole
{
    public static partial class CConsole
    {
        public static Point2D GetCursor
        {
            get => (Console.CursorLeft, Console.CursorTop);
        }

        public static void SetCursor(int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
        }

        public static void SetCursor(Point2D point)
        {
            Console.CursorLeft = point.X;
            Console.CursorTop = point.Y;
        }

        public static void TurnOnCursor()
            => Console.CursorVisible = true;

        public static void TurnOffCursor()
            => Console.CursorVisible = false;

        /// <summary>
        /// Verifies if the console has enough space to write.
        /// If not, it prints newlines and returns the cursor
        /// in the previous position
        /// </summary>
        /// <param name="amount">Amount that needs to be printed on the console. Default = 1</param>
        /// <returns>Point2D</returns>
        public static Point2D SetUpCursor(int amount = 1)
        {
            var cursor = GetCursor;
            var limit = Console.WindowHeight;

            if (cursor.X != 0)
                cursor.X = 0;

            var sum = cursor.Y + amount;
            if (sum >= limit)
            {
                var offset = sum - limit;

                for (int i = 0; i < offset; i++)
                    Console.Write(Environment.NewLine);

                cursor.Y -= offset;
            }

            return cursor;
        }

        public static IEnumerable<Point2D> GetLines(int amount)
        {
            Point2D[] positions = new Point2D[amount];
            Point2D cursor = GetCursor;
            var limit = Console.WindowHeight;

            if (cursor.X != 0)
                cursor.X = 0;

            var sum = cursor.Y + amount;
            if (sum < limit)
            {
                for (int i = 0; i < amount; i++)
                {
                    positions[i] = cursor;
                    cursor.Y++;
                }
            }
            else if (sum >= limit)
            {
                var offset = sum - limit;

                for (int i = 0; i < amount; i++)
                {
                    var y = cursor.Y - offset + i;
                    
                    if ( i < offset )
                        Console.Write(Environment.NewLine);

                    positions[i] = (cursor.X, y);
                }
            }

            return positions;
        }

        public static void Write(string message)
            => Out.Write(message);

        public static void Write(string message, params object[] args)
            => Out.Write(message, args);

        public static void WriteLine()
            => Out.Write(Environment.NewLine);

        public static void WriteLine(string message)
            => Out.WriteLine(message);

        public static void WriteLine(string message, params object[] args)
            => Out.WriteLine(message, args);

        /// <summary>
        /// Se apertar Y ou Enter, retorna true
        /// </summary>
        /// <returns></returns>
        public static bool YesOrNo()
        {
            ConsoleKey key = Console.ReadKey().Key;
            
            WriteLine();

            bool bkey = key == ConsoleKey.Y || key == ConsoleKey.Enter;
            return bkey;
        }
    }
}