namespace LeetCode.Csharp.Solutions2
{
    using LeetCode.Csharp.Common;

    public class LinkedLists
    {
        // https://leetcode.com/problems/merge-k-sorted-lists/
        public ListNode MergeKLists(ListNode[] lists)
        {
            MaxHeap<ListHeapNode> h = new MaxHeap<ListHeapNode>(lists.Length * 2);
            foreach (ListNode node in lists)
            {
                h.Insert(new ListHeapNode { Node = node });
            }

            ListNode head = null;
            ListNode curr = null;
            while (h.Count > 0)
            {
                ListHeapNode min = h.GetMax();
                h.RemoveMax();

                if (min.Node.next != null)
                {
                    h.Insert(new ListHeapNode { Node = min.Node.next });
                }

                if (head == null)
                {
                    head = min.Node;
                }

                if (curr == null)
                {
                    curr = head;
                }
                else
                {
                    curr.next = min.Node;
                    curr = curr.next;
                }
            }

            return head;
        }

        private class ListHeapNode : IHeapNode
        {
            public ListNode Node { get; set; }

            public int GetValue()
            {
                // Make it a min heap.
                return -this.Node.val;
            }
        }

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
        
        public class Node {
            public int val;
            public Node next;
            public Node random;
    
            public Node(int _val) {
                val = _val;
                next = null;
                random = null;
            }
        }
        
        // https://leetcode.com/problems/copy-list-with-random-pointer/
        public Node CopyRandomList(Node head) {
            // REVIEW: 超级巧妙的方法！
            Node node = head;
            while (node != null)
            {
                Node oldNext = node.next;
                node.next = new Node(node.val);
                node.next.next = oldNext;
                node = oldNext;
            }

            node = head;
            while (node != null)
            {
                node.next.random = node.random?.next;
                node = node.next.next;
            }

            node = head;
            head = node.next;
            while (node != null)
            {
                Node oldNext = node.next.next;
                node.next.next = node.next.next == null ? null : node.next.next.next;
                node = oldNext;
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