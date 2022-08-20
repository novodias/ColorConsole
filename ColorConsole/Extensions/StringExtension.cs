namespace ColorConsole.Extensions;

public static class StringExtension
{
    public static string WithSpaces(this string text)
    {
        static int Count(int length) {
            return Math.Max(length, Console.WindowWidth) - Math.Min(length, Console.WindowWidth);
        }

        int count;
        if ( text.Length > Console.WindowWidth )
        {
            int start = 0;
            int end = Console.WindowWidth;
            int rows = text.GetRectangle().rows;
            
            string[] lines = new string[rows];
            for (int i = 0; i < rows; i++)
            {
                lines[i] = text.Substring(start, end);
                start = end;
                end += end;
                
                if ( end > text.Length )
                {
                    var end2 = end;
                    end -= text.Length;
                    end = end2 + end;
                }
            }

            var lText = lines.Last();
            count = Count(lText.Length);
            return text + new string(' ', count);
        }

        count = Count(text.Length);
        return text + new string(' ', count);
    }

    public static (int rows, int columns) GetRectangle(this string text)
    {
        int rows = 0, columns = 0;

        using var sr = new StringReader(text);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Length > columns)
                columns = line.Length;

            rows++;
        }

        return (rows, columns);
    }
}