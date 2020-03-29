namespace LeetCode.Csharp
{
    using System;
    using LeetCode.Csharp.Solutions;

    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("LeetCode solutions in Csharp.");
            var solution = new MinWindowSolution();
            Console.WriteLine(solution.MinWindow("a", "aa"));
        }
    }
}