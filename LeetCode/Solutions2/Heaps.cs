namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LeetCode.Csharp.Common;

    public class Heaps
    {
        public class HireWorker : IHeapNode
        {
            private readonly int _quality;

            public HireWorker(int quality)
            {
                this._quality = quality;
            }

            public int GetValue()
            {
                return this._quality;
            }
        }

        public double MincostToHireWorkers(int[] quality, int[] wage, int K)
        {
            // REVIEW: 注意局部性思想...
            List<Tuple<double, int>> workers = new List<Tuple<double, int>>();
            for (int i = 0; i < quality.Length; i++)
            {
                workers.Add(Tuple.Create((double)wage[i] / quality[i], quality[i]));
            }

            workers = workers.OrderBy(w => w.Item1).ToList();

            double min = double.MaxValue;
            int qualitySum = 0;
            MaxHeap<HireWorker> workerHeap = new MaxHeap<HireWorker>(quality.Length);

            for (int i = 0; i < workers.Count; i++)
            {
                workerHeap.Insert(new HireWorker(workers[i].Item2));
                qualitySum += workers[i].Item2;

                if (workerHeap.Count == K)
                {
                    min = Math.Min(min, workers[i].Item1 * qualitySum);
                    qualitySum -= workerHeap.GetMax().GetValue();
                    workerHeap.RemoveMax();
                }
            }

            return min;
        }
    }
}
