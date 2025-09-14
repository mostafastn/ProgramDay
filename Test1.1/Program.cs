using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter matrix in JSON-like format:");
        // خواندن کل ورودی به صورت چند خط
        List<string> lines = new List<string>();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            lines.Add(line.Trim());
            if (line.Contains("]]")) break; // انتهای ماتریس
        }

        // حذف براکت‌های اضافی و تبدیل به ماتریس
        List<int[]> rows = new List<int[]>();
        foreach (var l in lines)
        {
            string clean = l.Replace("[", "")
                            .Replace("]", "")
                            .Replace(",", " ")
                            .Trim();
            if (string.IsNullOrWhiteSpace(clean)) continue;
            int[] row = clean.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(int.Parse)
                             .ToArray();
            rows.Add(row);
        }

        int n = rows.Count;
        int[,] matrix = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                matrix[i, j] = rows[i][j];

        int mafiaCount = n / 3;
        string result = FindMafia(matrix, n, mafiaCount);
        Console.WriteLine("Result (1=Citizen, 0=Mafia): " + result);
    }

    static string FindMafia(int[,] matrix, int n, int mafiaCount)
    {
        foreach (var mafiaSet in GetCombinations(Enumerable.Range(0, n).ToList(), mafiaCount))
        {
            bool ok = true;
            for (int i = 0; i < n && ok; i++)
            {
                bool isMafia = mafiaSet.Contains(i);
                for (int j = 0; j < n && ok; j++)
                {
                    if (i == j) continue;
                    int opinion = matrix[i, j];
                    bool realIsMafia = mafiaSet.Contains(j);

                    if (!isMafia) // شهروند راستگو
                    {
                        if (!CheckCitizenOpinion(opinion, realIsMafia))
                            ok = false;
                    }
                    else // مافیا دروغگو
                    {
                        if (!CheckMafiaOpinion(opinion, realIsMafia))
                            ok = false;
                    }
                }
            }

            if (ok)
                return string.Join("", Enumerable.Range(0, n).Select(x => mafiaSet.Contains(x) ? '0' : '1'));
        }
        return "No solution";
    }

    static bool CheckCitizenOpinion(int opinion, bool targetIsMafia)
    {
        if (opinion == 1 && targetIsMafia) return false;
        if (opinion == -1 && !targetIsMafia) return false;
        return true;
    }

    static bool CheckMafiaOpinion(int opinion, bool targetIsMafia)
    {
        if (opinion == 1 && !targetIsMafia) return false;
        if (opinion == -1 && targetIsMafia) return false;
        return true;
    }

    static IEnumerable<List<T>> GetCombinations<T>(List<T> list, int length)
    {
        if (length == 0) yield return new List<T>();
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                foreach (var tail in GetCombinations(list.Skip(i + 1).ToList(), length - 1))
                {
                    var res = new List<T> { list[i] };
                    res.AddRange(tail);
                    yield return res;
                }
            }
        }
    }
}
