namespace LeetCode.Csharp.Solutions2
{
    using LeetCode.Csharp.Common;

    public class LinkedLists
    {
        // 143 - https://leetcode.com/problems/reorder-list/
        public void ReorderList(ListNode head) {
            int cnt = this.Count(head);
            if (cnt <= 1) return;
        
            int splitPos = (cnt+1)/2;
            ListNode node = head;
            for(int i=0;i<splitPos-1;i++) {
                node = node.next;
            }
        
            ListNode secondHalf = node.next;
            node.next = null;
            secondHalf = Reverse(secondHalf);
        
            node = head;
            while(node != null) {
                ListNode next = node.next;
                node.next = secondHalf;
                if(secondHalf!=null) {
                    ListNode secondNext = secondHalf.next;
                    secondHalf.next = next;
                    secondHalf=secondNext;
                }
            
                node = next;
            }
        }
        
        // 92 - https://leetcode.com/problems/reverse-linked-list-ii/
        public ListNode ReverseBetween(ListNode head, int m, int n)
        {
            // REVIEW: 边界情况处理
            if (head == null || head.next == null || m == n) return head;
            ListNode node = head;
            
            if (m == 1)
            {
                // There's no first section.
                ListNode reverse = node;
                for (int i = 0; i < n - m; i++) node = node.next;
                ListNode th = node.next; // Start third section.
                node.next = null; // For reverse.
                
                ListNode newRv = this.Reverse(reverse);
                reverse.next = th;
                head = newRv;
            }
            else
            {
                for (int i = 0; i < m - 2; i++) node = node.next;
                ListNode preSh = node; // Node before second section.
                ListNode sh = node.next; // Starting second section.
                
                // Do one more move.
                for (int i = 0; i <= n - m; i++) node = node.next;
                ListNode th = node.next; // Start third section.
                node.next = null; // For reverse.

                ListNode newSh = this.Reverse(sh); // 4->3->2->null
                preSh.next = newSh; // 1->4->3->2->null
                sh.next = th;
            }

            return head;
        }

        private ListNode Reverse(ListNode head) {
            ListNode prev = null;
            while (head != null ) {
                ListNode next = head.next;
                head.next = prev;
                prev = head;
                head = next;
            }
        
            return prev;
        }
    
        private int Count(ListNode head) {
            ListNode node = head;
            int count = 0;
            while (node != null) {
                count++;
                node = node.next;
            }
            return count;
        }
    }
}