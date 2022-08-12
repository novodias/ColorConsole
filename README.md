# ColorConsole

## How it works:

### Print text and background with color
``` C#
using ColorConsole;

// The '[', ']', ';' and '&' are ignored, to write them to the console,
// add before the char you wish to write a '&'.
// Example: "&& &[ &] &;"

// The char ';' tells to reset the color to the default,
// add a 'C' to reset the background and the foreground.

string text = 
"[green]This text is green;\n" +
"[console blue][black]This text is black and the background is blue;C"

CConsole.Write(text + "\n");
CConsole.WriteLine(text);
```

### Selecting a value from an array
``` C#
using ColorConsole;

struct Person 
{
    public string Name;
    public int Age;
    public string FavoriteColor;
    
    public Person(string name, int age, string color)
    {
        Name = name;
        Age = age;
        FavoriteColor = color;
    }
}

var persons = new Person[3] 
{ 
    new Person("Fulano", 32, "Blue"),
    new Person("Ciclano", 46, "Red"),
    new Person("Sicrano", 23, "Green")
};

Person p;
p = CConsole.Selectable<Person>("Select a person", persons);
p = CConsole.Select<Person>("Select a perosn", persons);
```

### Show download progress
``` C#
using ColorConsole;

static HttpClient Client = new();

using var response = await Client.SendAsync(HttpMethod.Get, "url/to/file")
response.EnsureSuccessStatusCode();

using var source = await response.Content.ReadAsStreamAsync();
using var destination = File.Create("path/to/file");

// Still very early, the plan is to automate this part.
if (source.Length.HasValue)
{
    var cursor = Console.GetCursorPosition();
    var length = source.Length.Value;
    var progressInfo = new ProgressInfo(cursor, length, "Downloading");
    await source.CopyToAsyncWithProgress(destination, progressInfo);
}
else
    await source.CopyToAsync(destination);
```
