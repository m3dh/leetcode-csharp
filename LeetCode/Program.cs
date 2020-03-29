namespace LeetCode.Csharp
{
    using System;
    using LeetCode.Csharp.Solutions;

    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("LeetCode solutions in Csharp.");
            var solution = new IsNStraightHandSolution();
            Console.WriteLine(solution.IsNStraightHand(new []{1,2,3,6,2,3,4,7,8}, 3));
        }
    }
}