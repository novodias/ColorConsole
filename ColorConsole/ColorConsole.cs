namespace ColorConsole
{
    public static partial class CConsole
    {
        private static readonly Dictionary<string, ConsoleColor> _searchColor = new()
        {
            { "black", ConsoleColor.Black },
            { "blue", ConsoleColor.Blue },
            { "cyan", ConsoleColor.Cyan },
            { "darkblue", ConsoleColor.DarkBlue },
            { "darkcyan", ConsoleColor.DarkCyan },
            { "darkgray", ConsoleColor.DarkGray },
            { "darkgreen", ConsoleColor.DarkGreen },
            { "darkmagenta", ConsoleColor.DarkMagenta },
            { "darkred", ConsoleColor.DarkRed },
            { "darkyellow", ConsoleColor.DarkYellow },
            { "gray", ConsoleColor.Gray },
            { "green", ConsoleColor.Green },
            { "magenta", ConsoleColor.Magenta },
            { "red", ConsoleColor.Red },
            { "white", ConsoleColor.White },
            { "yellow", ConsoleColor.Yellow },
        };

        public static void Write(string message)
        {
            bool shouldWrite = true;
            string colorString = string.Empty;

            for (int i = 0; i < message.Length; i++)
            {
                var ch = message[i];

                // Ao encontrar o '[', vai desativar o shouldWrite e não escrever o char
                // e vai formar a cor que foi digitada
                if (ch.Equals('['))
                {
                    shouldWrite = false;
                    // colorString += ch;
                }
                else if (ch.Equals(']'))
                {
                    shouldWrite = true;

                    if (colorString.Contains("console "))
                    {
                        colorString = colorString.Remove(0, "console ".Length);
                        Console.BackgroundColor = _searchColor[colorString];
                    }
                    else
                        Console.ForegroundColor = _searchColor[colorString];
                    
                    colorString = string.Empty;
                }
                // Caso encontre o '&', escreve a próxima letra e então pula ela no for loop
                else if (ch.Equals('&'))
                {
                    Console.Write(message[i + 1]);
                    i++;
                }
                else if ( !shouldWrite && !ch.Equals(']') )
                    colorString += ch;
                else if (ch.Equals(';'))
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    if ( !(i + 1 == message.Length) )
                        if (message[i + 1] == 'C')
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            i++;
                        }

                    // Console.ResetColor();
                }
                else if ( shouldWrite )
                    Console.Write(ch);
            }

            Console.ResetColor();
        }

        public static void WriteLine()
            => Console.Write("\n");

        public static void WriteLine(string message)
        {
            Write(message);
            Console.Write("\n");
        }

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