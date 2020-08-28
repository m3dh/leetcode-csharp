namespace LeetCode.Csharp.Solutions2
{
    using System;
    using System.Linq;
    using LeetCode.Csharp.Common;

    class Arrays3
    {
        private class MinMeetingRoomNode : IHeapNode
        {
            public readonly int EndTime;

            public MinMeetingRoomNode(int endTime)
            {
                this.EndTime = endTime;
            }

            public int GetValue()
            {
                return -this.EndTime;
            }
        }

        // https://leetcode.com/problems/meeting-rooms-ii/
        public int MinMeetingRooms(int[][] intervals)
        {
            // REVIEW: Idea - 维护一个堆，里面是现在正在开的会，每来一个新会就把已经开完的会弹出

            intervals = intervals.OrderBy(i => i[0]).ThenBy(i => -i[1]).ToArray();
            MaxHeap<MinMeetingRoomNode> roomMinHeap = new MaxHeap<MinMeetingRoomNode>(intervals.Length);

            int max = 0;
            foreach (int[] interval in intervals)
            {
                int startTime = interval[0];
                while (roomMinHeap.Count > 0 && roomMinHeap.GetMax().EndTime <= startTime)
                {
                    roomMinHeap.RemoveMax();
                }

                roomMinHeap.Insert(new MinMeetingRoomNode(interval[1]));
                max = Math.Max(max, roomMinHeap.Count);
            }

            return max;
        }
    }
}
