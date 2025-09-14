using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("Please enter points in one line (example: [0,0], [4,2], [6,6], [0,5], [9,9]):");
        string input = Console.ReadLine();

        try
        {
            var pts = ParsePoints(input);
            if (pts.Count < 2)
            {
                Console.WriteLine("Error: At least 2 points are required.");
                return;
            }

            var visited = new HashSet<string>();
            for (int i = 0; i < pts.Count - 1; i++)
            {
                AddLineBresenham(pts[i].Item1, pts[i].Item2, pts[i + 1].Item1, pts[i + 1].Item2, visited);
            }

            Console.WriteLine("Total unique cells visited: " + visited.Count);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Input processing error: " + ex.Message);
        }
    }

    static List<Tuple<int, int>> ParsePoints(string s)
    {
        var nums = new List<int>();
        foreach (Match m in Regex.Matches(s, @"-?\d+"))
        {
            nums.Add(int.Parse(m.Value));
        }
        if (nums.Count % 2 != 0) throw new Exception("Uneven number of coordinates.");
        var pts = new List<Tuple<int, int>>();
        for (int i = 0; i < nums.Count; i += 2)
        {
            pts.Add(Tuple.Create(nums[i], nums[i + 1]));
        }
        return pts;
    }

    static void AddLineBresenham(int x0, int y0, int x1, int y1, HashSet<string> set)
    {
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;

        int x = x0;
        int y = y0;
        set.Add($"{x},{y}");

        if (dx >= dy)
        {
            int err = 2 * dy - dx;
            for (int i = 0; i < dx; i++)
            {
                x += sx;
                if (err > 0)
                {
                    y += sy;
                    err += 2 * (dy - dx);
                }
                else
                {
                    err += 2 * dy;
                }
                set.Add($"{x},{y}");
            }
        }
        else
        {
            int err = 2 * dx - dy;
            for (int i = 0; i < dy; i++)
            {
                y += sy;
                if (err > 0)
                {
                    x += sx;
                    err += 2 * (dx - dy);
                }
                else
                {
                    err += 2 * dx;
                }
                set.Add($"{x},{y}");
            }
        }
    }
}
