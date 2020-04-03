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
            var solution = new MinRemoveToMakeValidSolution();
            Console.WriteLine(solution.MinRemoveToMakeValid("(a)"));
            Console.WriteLine(solution.MinRemoveToMakeValid("lee(t(c)o)de)"));
            Console.WriteLine(solution.MinRemoveToMakeValid("a)b(c)d"));
            Console.WriteLine(solution.MinRemoveToMakeValid("))(("));
            Console.WriteLine(solution.MinRemoveToMakeValid("(a(b(c)d)"));
            Console.WriteLine(solution.MinRemoveToMakeValid("())()((("));
        }
    }
}