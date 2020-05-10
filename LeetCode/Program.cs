using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace LeetCode.Csharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions;
    using LeetCode.Csharp.Solutions2;
    using Newtonsoft.Json;

    internal class Program
    {
        public static void Main()
        {
            for (int i = 0; i <= 100; i++)
            {
                Console.WriteLine($"i-{i}, {BinaryFindMaxLessVal(i)}");
            }
        }
        
        private static int BinaryFindMaxLessVal(int num) {
            int l = 0;
            int r = num;
            while (l <= r)
            {
                int m = (l + r) / 2;

               // Console.WriteLine($"l{l},r{r},m{m}");
                if (m * m <= num && (m + 1) * (m + 1) > num) return m;
                if (m * m > num)
                {
                    r = m ;
                }
                else
                {
                    l = m + 1;
                }
            }

            return -1;
        }
    }
}