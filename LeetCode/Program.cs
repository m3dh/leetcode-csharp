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
            var obj = new Binary();
            obj.Run();
        }

        public static int Find(int[] arr)
        {
            // Find the first 1.
            if (arr[arr.Length - 1] == 1)
            {
                int l = 0;
                int r = arr.Length - 1;
                while (l <= r)
                {
                    int mid = (l + r) / 2;
                    if (arr[mid] == 1)
                    {
                        if (mid == 0 || arr[mid - 1] == 0) return mid;
                        else
                        {
                            r = mid - 1; // arr[mid - 1] -> 1
                        }
                    }
                    else
                    {
                        l = mid + 1;
                    }
                    
                    // Console.WriteLine($"L:{l}, R:{r}");
                }
            }
            
            return -1;
        }
    }
}