namespace LeetCode.Csharp
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var fb=new ConcurrencySolutions.ZeroEvenOdd(10);
            var t1 = Task.Run(() => fb.Zero(i => Console.Write(i)));
            var t2 = Task.Run(() => fb.Odd(i => Console.Write(i)));
            var t3 = Task.Run(() => fb.Even(i => Console.Write(i)));
            Task.WaitAll(t1, t2, t3);
        }
    }
}