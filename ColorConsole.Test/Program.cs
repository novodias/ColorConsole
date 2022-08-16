using ColorConsole;
using ColorConsole.Progress;
using ColorConsole.Extensions;

string[] options =
{
    "Progress Test",
    "Progress Chrome Download Test",
    "List Progress",
    "Clean Console",
    "Exit"
};

bool exit = false;

while (!exit)
{
    var n = CConsole.SelectableNumber("Tests: ", options);
    
    switch (n)
    {
        case 1:
            ProgressTest();
            break;
        case 2:
            await ProgressChromeTest();
            break;
        case 3:
            ListProgressTest();
            break;
        case 4:
            for (int i = 0; i < Console.WindowHeight; i++)
                Console.Write(Environment.NewLine);
            break;
        case 5:
            exit = true;
            break;
    }
}

static void ProgressTest()
{
    CConsole.TurnOffCursor();

    long length = 250000000;
    var info = new ProgressInfo(length, "[green]PROGRESS TESTE;");

    while (!info.IsCompleted)
    {
        var value = info.Length + 81920;

        info.Report(value);
    }

    CConsole.TurnOnCursor();
    CConsole.WriteLine();
}

static async Task ProgressChromeTest()
{
    CConsole.TurnOffCursor();

    try
    {    
        var chromeurl = "https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb";

        using var client = new HttpClient();
        using var response = await client.GetAsync(chromeurl, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        var length2 = response.Content.Headers.ContentLength;

        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "googlechrome.deb");
        using WatchStream file = File.Create(path);
        using var source = await response.Content.ReadAsStreamAsync();

        if (length2.HasValue)
        {
            var info2 = new ProgressInfo(length2.Value, "PROGRESS TEST DOWNLOADING CHROME");
            await source.CopyToAsyncWithProgress(file, info2);
            Console.Write(Environment.NewLine);
        }
        else
            throw new Exception();
    }
    finally
    {
        CConsole.TurnOnCursor();
    }
}

static void ListProgressTest()
{
    var n = CConsole.SelectableNumber("TESTS LIST PROGRESS:", "NO PARAMETER AMOUNT", "WITH PARAMETER AMOUNT", "GET LINES TEST");

    CConsole.TurnOffCursor();

    ProgressInfoCollection listp;
    long length = 250000000;
    if (n == 1)
    {
        CConsole.WriteLine("NO PARAMETER AMOUNT: 5");
        listp = new();

        for (int i = 0; i < 5; i++)
            listp.Add(length, $"TEST {i + 1}");
    }
    else if (n == 2)
    {
        CConsole.WriteLine("WITH PARAMETER AMOUNT: 5");
        listp = new(5);

        for (int i = 0; i < 5; i++)
            listp.Add(length, $"TEST {i + 1}");
    }
    else if (n == 3)
    {
        CConsole.WriteLine("GET LINES TEST: 5");
        var pos = CConsole.GetLines(5).ToArray();
        listp = new();

        for (int i = 0; i < 5; i++)
            listp.Add(length, pos[i], $"TEST {i + 1}");
    }
    else
        return;

    int index = 0;
    while (!listp.IsCompleted)
    {
        listp.Update(index, 81920);

        index++;
        if (index == listp.Count) index = 0;
    }

    CConsole.TurnOnCursor();

    Console.WriteLine();
}