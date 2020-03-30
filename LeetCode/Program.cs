namespace LeetCode.Csharp
{
    using System;
    using LeetCode.Csharp.Solutions2;

    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("LeetCode solutions in Csharp.");
            var solution = new CalculatorSolution();
            Console.WriteLine(solution.Calculate("2*3+5 / 2"));
        }
    }
}