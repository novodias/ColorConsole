# ColorConsole

## How it works:
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
    new Person("Fulano", "32", "Blue"),
    new Person("Ciclano", "46", "Red"),
    new Person("Sicrano", "23", "Green")
};
  
var p = CConsole.Selectable<Person>("Select a person", persons);
```
