namespace LeetCode.Csharp
{
    using System;
    using LeetCode.Csharp.Solutions2;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var solution = new FindDuplicatesSolution();
            var ret = solution.FindDuplicates(new[] {4, 3, 2, 7, 8, 2, 3, 1});
            Console.WriteLine(string.Join(",", ret));
        }

        private static void PrintDuplications(int[] array)
        {
            // Scan 1: Swap
            for (int i = 0; i < array.Length; i++)
            {
                // Ensures for any member number x in array, array[x] = x.
                // So in the second scan, if found any array[y] <> y, array[y] is a duplication.
                if (array[array[i]] != array[i])
                {
                    int tmp = array[i];
                    array[i] = array[tmp];
                    array[tmp] = tmp;
                }
            }

            // Console.WriteLine($"Debug: {string.Join(",", array)}");

            // Scan 2: Find duplications
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != i)
                {
                    Console.WriteLine($"Duplication: {array[i]}");
                }
            }
        }
    }
}