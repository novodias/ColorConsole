using ColorConsole;
using ColorConsole.Progress;
using ColorConsole.Extensions;

CConsole.SetErrorEmpty("[c blue, white]ERROR EMPTY;C");

string[] options =
{
    "Test",
    "Test Context",
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
            Test();
            break;
        case 2:
            TestContext();
            break;
        case 3:
            ProgressTest();
            break;
        case 4:
            await ProgressChromeTest();
            break;
        case 5:
            ListProgressTest();
            break;
        case 6:
            for (int i = 0; i < Console.WindowHeight; i++)
                Console.Write(Environment.NewLine);
            break;
        case 7:
            exit = true;
            break;
    }
}

static void Test()
{
    CConsole.Write("WRITE TEST ");
    CConsole.Write("[red]WRITE RED TEST; ");
    CConsole.Write("[c blue]WRITE BG BLUE TEST;C ");
    CConsole.Write("[c white, red]WRITE BG WHITE AND TEXT RED TEST;C");
    Console.WriteLine();

    Console.WriteLine();

    CConsole.WriteLine("WRITELINE TEST ");
    CConsole.WriteLine("[red]WRITELINE RED TEST; ");
    CConsole.WriteLine("[c blue]WRITELINE BG BLUE TEST;C ");
    CConsole.WriteLine("[c white, red]WRITELINE BG WHITE AND TEXT RED TEST;C");
    Console.WriteLine();

    Console.WriteLine();

    var read = CConsole.Read("READ TEST: ");
    CConsole.WriteLine(read);
    Console.WriteLine();

    read = CConsole.Read("READ TEST SAMELINE: ", true);
    CConsole.WriteLine(read);
    Console.WriteLine();

    read = CConsole.Read("READ TEST SAMELINE ERROR: ", true);
    CConsole.WriteLine(read);

    Console.WriteLine();
    Console.WriteLine();

    var readNumber = CConsole.ReadNumber<int>("READNUMBER TEST: ");
    CConsole.WriteLine(readNumber.ToString());
    Console.WriteLine();

    readNumber = CConsole.ReadNumber<int>("READNUMBER TEST SAMELINE: ", true);
    CConsole.WriteLine(readNumber.ToString());
    Console.WriteLine();

    readNumber = CConsole.ReadNumber<int>("READNUMBER TEST SAMELINE ERROR: ", true, "NOT A NUMBER");
    CConsole.WriteLine(readNumber.ToString());
    Console.WriteLine();

    Console.WriteLine();

    int[] array = { 1, 3, 5 };

    var readNumberArr = CConsole.ReadNumber("READNUMBER ARRAY TEST (1 3 5): ", array);
    CConsole.WriteLine(readNumberArr.ToString());
    Console.WriteLine();

    readNumberArr = CConsole.ReadNumber("READNUMBER ARRAY TEST SAMELINE (1 3 5): ", array, true);
    CConsole.WriteLine(readNumberArr.ToString());
    Console.WriteLine();

    readNumberArr = CConsole.ReadNumber("READNUMBER ARRAY TEST SAMELINE ERROR (1 3 5): ", array, true, "NUMBER NOT IN THE ARRAY");
    CConsole.WriteLine(readNumberArr.ToString());
    Console.WriteLine();

    Console.WriteLine();

    (int min, int max) between = (1, 10);

    var readNumberBetween = CConsole.ReadNumber("READNUMBER BETWEEN TEST (1, 10): ", between);
    CConsole.WriteLine(readNumberBetween.ToString());
    Console.WriteLine();

    readNumberBetween = CConsole.ReadNumber("READNUMBER BETWEEN TEST SAMELINE (1, 10): ", between, true);
    CConsole.WriteLine(readNumberBetween.ToString());
    Console.WriteLine();
}

static void TestContext()
{
    const string error = "[c white, red]ERROR;C";

    var readNumber = CConsole.ReadNumber<int>("CONTEXT IN - SAMELINE FALSE TEST: ", false, error);
    CConsole.WriteLine(readNumber.ToString());
    Console.WriteLine();

    readNumber = CConsole.ReadNumber<int>("CONTEXT IN - SAMELINE TRUE TEST: ", true, error);
    CConsole.WriteLine(readNumber.ToString());
    Console.WriteLine();
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
    var n = CConsole.SelectableNumber("TESTS LIST PROGRESS:", 
    "NO PARAMETER AMOUNT", 
    "WITH PARAMETER AMOUNT", 
    "GET LINES TEST");

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