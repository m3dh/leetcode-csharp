namespace LeetCode.Csharp.Solutions
{
    using System;
    using LeetCode.Csharp.Common;

    public class HasCycleSolution
    {
        // 142 - https://leetcode.com/problems/linked-list-cycle-ii/
        public ListNode DetectCycle(ListNode head)
        {
            // Answer: When slow meets fast, we have:
            //     s_steps = before_loop + n * loop_size + to_meet.
            //     f_steps = before_loop + m * loop_size + to_meet.
            //     s_steps * 2 = f_steps;
            // so:
            //     f_steps = s_steps + (m - n) * loop_size.
            //     s_steps = (m - n) * loop_size;
            //
            // Hence s_steps + before_loop = before_loop + (m - n) * loop_size;
            
            ListNode slow = head;
            ListNode fast = head;
            while (slow != null && fast != null)
            {
                slow = slow.next;
                fast = fast.next?.next;
                if (slow == fast && slow != null)
                {
                    ListNode hPtr2 = head;
                    while (hPtr2 != slow)
                    {
                        hPtr2 = hPtr2.next;
                        slow = slow.next;
                    }

                    return hPtr2;
                }
            }
            
            return null;
        }

        // 141 - https://leetcode.com/problems/linked-list-cycle/
        public bool HasCycle(ListNode head)
        {
            ListNode slow = head;
            ListNode fast = head;
            while (slow != null && fast != null)
            {
                slow = slow.next;
                fast = fast.next?.next;
                if (slow == fast && slow != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
