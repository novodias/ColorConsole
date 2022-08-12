using ColorConsole;

// var array = new string[10];
// for (int i = 0; i < 10; i++)
//     array[i] = (i + 1).ToString();

// var n = CConsole.Selectable("Selecione:", array);
// CConsole.WriteLine(n);

var array = new string[25];
for (int i = 0; i < 25; i++)
    array[i] = (i + 1).ToString();

var n = CConsole.Selectable("Selecione:", array);
CConsole.WriteLine(n);