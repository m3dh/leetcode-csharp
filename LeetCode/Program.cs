namespace LeetCode.Csharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions;
    using LeetCode.Csharp.Solutions2;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class Program
    {
        public static void Main()
        {
            new Trees2().Run();
        }
    }

    public class ProductOfNumbers
    {
        private List<long> list = new List<long>();

        public ProductOfNumbers()
        {
        }

        public void Add(int num)
        {
            if (num == 0)
            {
                this.list.Clear();
            }
            else
            {
                if (this.list.Count == 0)
                {
                    this.list.Add(num);
                }
                else
                {
                    this.list.Add(num * this.list[this.list.Count - 1]);
                }
            }
        }

        public int GetProduct(int k)
        {
            if (k > this.list.Count) return 0;
            else
            {
                // k = 2, 3 items    -   l[n-1] / l[n-3]
                var pre = this.list.Count >= k + 1 ? this.list[this.list.Count - k - 1] : 1;
                return (int) (this.list[this.list.Count - 1] / pre);
            }
        }
    }
}