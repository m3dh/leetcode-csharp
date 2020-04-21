namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LeetCode.Csharp.Common;

    public class Binary
    {
        // 50 - https://leetcode.com/problems/powx-n/
        public double MyPow(double x, int n)
        {
            // REVIEW: 二分法
            if (n == 0) return 1;
            if (n == Int32.MinValue) return 1.0 / (x * MyPow(x, - (n+1)));
            if (n < 0) return 1.0 / MyPow(x, -n);
            if (n % 2 == 0) {
                var half = MyPow(x, n / 2);
                return half * half;
            } else {
                return MyPow(x, n - 1) * x;
            }
        }

        // 338 - https://leetcode.com/problems/counting-bits/
        public int[] CountBits(int num)
        {
            if (num == 0) return new[]{0};
            
            int[] result = new int[num+1];
            result[0] = 0;
            result[1] = 1;
            
            for(int i=2;i<=num;i++) {
                if (i%2==1) result[i] = result[i-1]+1;
                else result[i] = result[i/2];
            }
        
            return result;
        }

        public int LeftMostColumnWithOne(BinaryMatrix binaryMatrix)
        {
            IList<int> dimensions = binaryMatrix.Dimensions();

            int leftMostCol = dimensions[1]; // A invalid column.
            for (int i = 0; i < dimensions[0]; i++)
            {
                if (binaryMatrix.Get(i, leftMostCol - 1) == 1)
                {
                    int l = 0;
                    int r = leftMostCol - 1;
                    int mid = leftMostCol;
                    while (l <= r)
                    {
                        mid = (l + r) / 2;
                        if (binaryMatrix.Get(i, mid) == 1)
                        {
                            if (mid == 0 || binaryMatrix.Get(i, mid - 1) == 0)
                            {
                                // return mid;
                                break;
                            }
                            else
                            {
                                r = mid - 1; // arr[mid - 1] -> 1
                            }
                        }
                        else
                        {
                            l = mid + 1;
                        }
                    }

                    if (mid < leftMostCol)
                    {
                        leftMostCol = mid;
                    }

                    // Cannot find any better results.
                    if (leftMostCol == 0) break;
                }
            }

            return leftMostCol < dimensions[1] ? leftMostCol : -1;
        }

        public void Run()
        {
            var mtxJson = "[[1,1,1,1,1],[0,0,0,1,1],[0,0,1,1,1],[0,0,0,0,1],[0,0,0,0,0]]";
            BinaryMatrix bm = new BinaryMatrix(mtxJson);
            Console.WriteLine(LeftMostColumnWithOne(bm));
        }
    }
}