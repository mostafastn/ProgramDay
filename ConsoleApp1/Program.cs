// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


Console.Write("Matrix size (n): ");
int n = int.Parse(Console.ReadLine());

Console.WriteLine($"Please enter {n * n} numbers (tank owners):");
string[] input = Console.ReadLine().Split(',', ' ', StringSplitOptions.RemoveEmptyEntries);

int[,] matrix = new int[n, n];
for (int i = 0; i < n * n; i++)
    matrix[i / n, i % n] = int.Parse(input[i]);

int moves = 0;

// الگوریتم ساده: هر سطر را مرتب می‌کنیم
for (int i = 0; i < n; i++)
{
    int[] row = new int[n];
    for (int j = 0; j < n; j++)
        row[j] = matrix[i, j];

    Array.Sort(row);

    for (int j = 0; j < n; j++)
    {
        if (matrix[i, j] != row[j])
            moves++;
        matrix[i, j] = row[j];
    }
}

Console.WriteLine($"\nNumber of moves: {moves}\n");
Console.WriteLine("Final matrix:");
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
        Console.Write(matrix[i, j] + "\t");
    Console.WriteLine();
}