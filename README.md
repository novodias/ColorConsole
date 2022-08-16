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
"[c red]The background is red;\n" +
"[c blue, black]The background is blue and this text is black;C";

CConsole.Write(text + "\n");
CConsole.WriteLine(text);
```

### Selecting a value from an array
``` C#
using ColorConsole;
using ColorConsole.Extensions;

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
p = CConsole.Select<Person>("Select a person", persons);
```

### Show download progress
``` C#
using ColorConsole;

...
var source = await response.Content.ReadAsStreamAsync();
var destination = File.Create("path/to/file");

if (source.Length.HasValue)
{
    var length = source.Length.Value;
    var progressInfo = new ProgressInfo(length, "Downloading");
    await source.CopyToAsyncWithProgress(destination, progressInfo);
}
else
    await source.CopyToAsync(destination);
```
