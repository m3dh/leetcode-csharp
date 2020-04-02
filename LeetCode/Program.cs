namespace LeetCode.Csharp
{
    using System;
    using System.Linq;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions;
    using LeetCode.Csharp.Solutions2;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var solution = new ReorganizeStringSolution();
            var result = solution.ReorganizeString("lovvv");
            Console.WriteLine(result);
        }
    }
}