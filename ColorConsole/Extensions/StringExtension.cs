namespace ColorConsole.Extensions;

public static class StringExtension
{
    public static string WithSpaces(this string text)
    {
        var count = Math.Max(text.Length, Console.WindowWidth) - Math.Min(text.Length, Console.WindowWidth);
        return text + new string(' ', count);
    }
}