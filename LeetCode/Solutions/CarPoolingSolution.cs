namespace LeetCode.Csharp.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // 1094 - https://leetcode.com/problems/car-pooling/
    public class CarPoolingSolution
    {
        class CarOper
        {
            public int Position { get; set; }
            public int Change { get; set; }
        }

        public bool CarPooling(int[][] trips, int capacity)
        {
            List<CarOper> opers = new List<CarOper>();
            foreach (int[] trip in trips)
            {
                opers.Add(new CarOper
                {
                    Position = trip[1],
                    Change = trip[0]
                });

                opers.Add(new CarOper
                {
                    Position = trip[2],
                    Change = -trip[0]
                });
            }

            int currentCap = 0;
            var opGroups = opers.GroupBy(op => op.Position).ToList();
            foreach (IGrouping<int, CarOper> oper in opGroups.OrderBy(g => g.Key))
            {
                foreach (CarOper carOper in oper)
                {
                    currentCap += carOper.Change;
                    Console.WriteLine($"{carOper.Position},{carOper.Change} - {currentCap}");
                }

                if (currentCap < 0 || currentCap > capacity)
                {
                    return false;
                }
            }

            return true;
        }
    }
}