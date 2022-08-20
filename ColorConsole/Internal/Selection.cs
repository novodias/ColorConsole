namespace ColorConsole.Internal;
internal class Selection<T>
{
    private readonly Book _book;

    internal Selection(IEnumerable<ValueInfo<T>> listValueInfo)
        => _book = CreateBook(listValueInfo.ToList());

    public ValueInfo<T> Init()
    {
        void getKey(ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.W)
                _book.MoveBack();

            if (key == ConsoleKey.DownArrow || key == ConsoleKey.S)
                _book.MoveNext();
        }

        Console.CursorVisible = false;
        DrawText();

        ConsoleKey key;
        while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
        {
            getKey(key);
            DrawText();
        }

        Console.Write(Environment.NewLine);
        Console.CursorVisible = true;

        return _book.CurrentLine.ValueInfo;
    }

    private void DrawText()
    {
        var page = _book.CurrentPage;
        var last = _book.LastPage;

        if (last is not null)
        {
            var space = new string(' ', Console.WindowWidth);
            foreach (var line in last)
            {
                Console.SetCursorPosition(0, line.Top);
                Console.Write(space);
            }
        }

        foreach (var line in page)
        {
            var text = line.ValueInfo.Information;
            Console.SetCursorPosition(0, line.Top);

            if (_book.CurrentLine.Equals(line))
                CConsole.Write("[console darkgray]" + text + ";C");
            else
                CConsole.Write(text);
        }
    }

    private static Book CreateBook(IList<ValueInfo<T>> listValueInfo)
    {
        const int linesPerPage = 10;
        var count = listValueInfo.Count;
        var lineCount = count > linesPerPage ? linesPerPage : count;

        var cursor = CConsole.GetLine(lineCount);

        // var cursorTop = Console.CursorTop;

        // var limit = Console.WindowHeight;
        // if ( cursorTop + lineCount > limit )
        // {
        //     var offset = cursorTop + lineCount - limit;

        //     for (int i = 0; i < offset; i++)
        //         Console.Write(Environment.NewLine);

        //     cursorTop -= offset;
        // }

        var book = new Book();
        var top = cursor.Y;

        for (int i = 0; i < count;)
        {
            var lines = new List<Line>();

            for (int j = 0; j < lineCount; j++)
            {
                if (i == count)
                    break;

                var valueinfo = listValueInfo[i];

                lines.Add(new Line()
                {
                    Top = top++,
                    ValueInfo = valueinfo
                });

                i++;
            }

            book.AddPage(lines);
            top = cursor.Y;
        }

        return book;
    }

    private class Book
    {
        /// <summary>
        /// The 'book'
        /// </summary>
        public IDictionary<int, IList<Line>> Pages { get; private set; }

        /// <summary>
        /// Returns the last page, returns null if the user didn't use
        /// </summary>
        public IList<Line>? LastPage { get; private set; } = null;

        /// <summary>
        /// Returns the current page
        /// </summary>
        public IList<Line> CurrentPage => this.Pages[_page];

        /// <summary>
        /// Returns the current line
        /// </summary>
        public Line CurrentLine => this.CurrentPage[_line];

        /// <summary>
        /// Page count
        /// </summary>
        public int PageCount { get => Pages.Count; }

        /// <summary>
        /// Line count of the current page
        /// </summary>
        public int LineCount { get => CurrentPage.Count; }

        private int _line = 0;
        private int _page = 0;

        public Book()
        {
            Pages = new Dictionary<int, IList<Line>>();
        }

        public void AddPage(IList<Line> lines)
            => Pages.Add(PageCount, lines);

        public void MoveNext()
        {
            // The order this executes totally matters
            LastPage = CurrentPage;
            _line++;

            if (_line == LineCount)
            {
                _line = 0;
                _page++;
            }

            if (_page == PageCount)
                _page = 0;
        }

        public void MoveBack()
        {
            // The order this executes totally matters
            LastPage = CurrentPage;
            _line--;

            if (_line == -1)
            {
                _page--;

                if (_page == -1)
                    _page = PageCount - 1;

                _line = LineCount - 1;
            }
        }
    }

    private struct Line
    {
        public int Top { get; init; }
        public ValueInfo<T> ValueInfo { get; init; }
    }
}