namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReorganizeStringSolution
    {
        // 767 - https://leetcode.com/problems/reorganize-string/
        public string ReorganizeString(string S)
        {
            Tuple<char, int>[] counters = S.GroupBy(ch => ch).Select(cg => Tuple.Create(cg.Key, cg.Count())).ToArray();
            char[] ret = new char[S.Length];

            // corner case: when there's a char with count of SLen + 1 / 2 (SLen is odd),
            // this character must be placed starting from index = 0, otherwise there won't
            // be any correct solutions.
            if (S.Length % 2 == 1)
            {
                for (int i = 0; i < counters.Length; i++)
                {
                    if (counters[i].Item2 == (S.Length + 1) / 2 && i != 0)
                    {
                        var tmp = counters[0];
                        counters[0] = counters[i];
                        counters[i] = tmp;
                        break;
                    }
                }
            }

            int index = 0;
            for (int i = 0; i < counters.Length; i++)
            {
                if (counters[i].Item2 > (S.Length + 1) / 2) return "";
                
                for (int j = 0; j < counters[i].Item2; j++)
                {
                    // Console.WriteLine($"Placing {counters[i].Item1} at {index}");
                    
                    ret[index] = counters[i].Item1;
                    index += 2;

                    if (index >= ret.Length) index = 1;
                }
            }
            
            return new string(ret) ;
        }
    }
}