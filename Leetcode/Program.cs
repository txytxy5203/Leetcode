using System.Collections.Generic;

namespace Leetcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //PriorityQueue<int,int> heap = new PriorityQueue<int, int>();
            //HashSet<string> set = new HashSet<string>();
            TreeNode a = new TreeNode(1);
            TreeNode b = new TreeNode(2);
            TreeNode c = new TreeNode(3);
            TreeNode d = new TreeNode(4);
            TreeNode e = new TreeNode(5);
            TreeNode f = new TreeNode(6);
            TreeNode g = new TreeNode(7);
            a.left = b; a.right = c;
            b.left = d; b.right = e;
            d.left = null;d.right = null;
            e.left = null;e.right = null;
            c.left = f; c.right = g;
            f.left = null;f.right = null;
            g.left = null;g.right = null;

            preOrderUnRecur(a);
        }
        #region 二叉树
        static IList<int> InorderTraversal(TreeNode root)
        {
            IList<int> result = new List<int>();
            preOrderRecur(root, result);
            return result;
        }
        static void inOrderRecur(TreeNode head, IList<int> list)
        {
            //中序遍历  在第二次进入节点的时候 执行逻辑
            if (head == null)
                return;
            inOrderRecur(head.left, list);
            list.Add(head.val);
            inOrderRecur(head.right, list);
        }
        static void preOrderRecur(TreeNode head, IList<int> list)
        {
            //前序遍历 在第一次进入节点时 执行逻辑
            if (head == null)
                return;
            list.Add(head.val);
            preOrderRecur(head.left, list);
            preOrderRecur(head.right, list);
        }
        static void preOrderUnRecur(TreeNode head)
        {
            //不使用递归的方式
            if (head == null) return;
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(head);
            while (stack.Count > 0)
            {
                TreeNode node = stack.Pop();
                Console.WriteLine(node.val);
                if (node.right != null)
                    stack.Push(node.right);
                if (node.left != null)
                    stack.Push(node.left);
            }
        }
        #endregion

        static ListNode GetIntersectionNode(ListNode headA, ListNode headB)
        {
            // 对齐的思想 因为没有环的存在
            // 下次试一下 HashSet
            if (headA == null || headB == null) 
                return null;

            ListNode currA = headA;
            ListNode currB = headB;
            int lengthA = 1;
            int lengthB = 1;
            while (true)
            {
                if(currA.next != null)
                {
                    currA = currA.next;
                    lengthA++;
                }
                if(currB.next != null)
                {
                    currB = currB.next;
                    lengthB++;
                }
                if (currA.next == null && currB.next == null)
                    break;
            }
                      
            if (currA != currB) 
                return null;


            currA = headA;
            currB = headB;
            if (lengthA >= lengthB)
            {
                int offset = lengthA - lengthB;
                while(offset > 0)
                {
                    currA = currA.next;
                    offset--;
                }
                while (currA != currB)
                {
                    currA = currA.next;
                    currB = currB.next;
                }
                return currA;
            }
            else 
            {
                int offset = lengthB - lengthA;
                while (offset > 0)
                {
                    currB = currB.next;
                    offset--;
                }
                while (currA != currB)
                {
                    currA = currA.next;
                    currB = currB.next;
                }
                return currA;
            }

        }
        static ListNode DetectCycle(ListNode head)
        {
            HashSet<ListNode> visited = new HashSet<ListNode>();

            while (head != null)
            {
                if (visited.Contains(head))
                {
                    return head;
                }
                else
                {
                    visited.Add(head);
                }
                head = head.next;
            }
            return null;
        }
        static ListNode DetectCycle2(ListNode head)
        {
            if (head == null || head.next == null) return null;

            ListNode slow = head;
            ListNode fast = head;
            while (fast != null)
            {
                slow = slow.next;
                fast = fast.next?.next;

                //slow 和 fast 指针重合后 fast回到 head  下一次他们再重合就是 环的入口
                if (slow == fast)
                {
                    fast = head;
                    while (slow != fast)
                    {
                        slow = slow.next;
                        fast = fast.next;
                    }
                    return slow;
                }
            }
            return null;
        }
        static bool HasCycle(ListNode head)
        {
            HashSet<ListNode> visited = new HashSet<ListNode>();

            while (head != null)
            {
                if (visited.Contains(head))
                {
                    return true;
                }
                else
                {
                    visited.Add(head);
                }
                head = head.next;
            }
            return false;
        }
        static bool HasCycle2(ListNode head)
        {
            //使用快慢指针做
            //还是先 排除特殊情况
            if (head == null || head.next == null) return false;

            ListNode slow = head;
            ListNode fast = head;
            while (fast != null)
            {
                slow = slow.next;
                fast = fast.next?.next;
                if (slow == fast)
                {
                    return true;
                }
            }
            return false;
        }
        static Node CopyRandomList(Node head)
        {
            if (head == null) return null;
            Node curr = head;
            //一定要用一个 dict 不然就找不到对应关系了
            Dictionary<Node, Node> map = new Dictionary<Node, Node>();
            //先复制 节点
            while (curr != null)
            {
                map.Add(curr, new Node(curr.val));
                curr = curr.next;
            }

            curr = head;
            while (curr != null)
            {
                //注意这里去map中找 不能 new 了
                //一定要注意 random 可能为null
                map[curr].next = curr.next != null ? map[curr.next] : null;
                map[curr].random = curr.random != null ? map[curr.random] : null;


                curr = curr.next;
            }

            return map[head];
        }
        static Node CopyRandomList2(Node head)
        {
            if (head == null) return null;

            Node curr = head;
            //利用 链表的位置关系 将 Dict 省去了
            //1 -> 1' -> 2 -> 2' -> 3 -> 3' 
            while (curr != null)
            {
                Node newCurr = new Node(curr.val);
                Node next = curr.next;
                curr.next = newCurr;
                newCurr.next = next;

                //移动指针
                curr = next;
            }

            //先处理random指针 因为不能破坏之前的有序结构
            curr = head;
            Node newHead = head.next;
            while (curr != null)
            {
                //原链表的下一个Node
                Node next = curr.next.next;

                curr.next.random = curr.random?.next;
                curr = next;
            }

            //split
            curr = head;
            while (curr != null)
            {
                Node next = curr.next.next;

                curr.next.next = next?.next;
                curr.next = next;

                curr = next;
            }
            return newHead;
        }
        static Node Partition(Node head, int x)
        {
            Node sh = null;
            Node st = null;
            Node eh = null;
            Node et = null;
            Node bh = null;
            Node bt = null;

            while (head != null)
            {
                if (head.val < x)
                {
                    if (sh == null)
                    {
                        sh = head;
                        st = head;
                    }
                    else
                    {
                        st.next = head;
                        st = head;
                    }
                }
                else if (head.val > x)
                {
                    if (eh == null)
                    {
                        eh = head;
                        et = head;
                    }
                    else
                    {
                        et.next = head;
                        et = head;
                    }
                }
                else
                {
                    if (bh == null)
                    {
                        bh = head;
                        bt = head;
                    }
                    else
                    {
                        bh.next = head;
                        bh = head;
                    }
                }
                head = head.next;
            }

            //各区域相连接
            //if (sh == null)
            //{
            //    if (eh == null)
            //    {
            //        return bh;
            //    }
            //    else 
            //    {
            //        if (bh != null)
            //            et.next = bh;
            //        return eh;                   
            //    }
            //}
            //else
            //{
            //    if (eh == null)
            //    {
            //        if (bh != null)
            //            st.next = bh;
            //        return sh;

            //    }
            //    else
            //    {
            //        if (bh != null)
            //            et.next = bh;                  
            //        st.next = eh;                       
            //        return sh;                   
            //    }
            //}
            if (sh == null && eh == null)
            {
                return bh;
            }
            else if (sh == null && eh != null)
            {
                if (bh != null)
                    et.next = bh;
                return eh;
            }
            else if (sh != null && eh == null)
            {
                if (bh != null)
                    st.next = bh;
                return sh;
            }
            else
            {
                if (bh != null)
                    et.next = bh;
                st.next = eh;
                return sh;
            }

        }


        // need n extra space
        static bool IsPalindrome(Node head)
        {
            Stack<Node> stack = new Stack<Node>();
            Node curr = head;
            while (curr != null)
            {
                stack.Push(curr);
                curr = curr.next;
            }
            while (head != null)
            {
                if (head.val != stack.Pop().val)
                {
                    return false;
                }
                head = head.next;
            }
            return true;
        }
        // need n/2 extra space
        static bool IsPalindrome2(Node head)
        {
            //捣乱的先 return
            if (head == null || head.next == null)
            {
                return true;
            }

            //快慢指针 找到中点
            Node right = head.next;
            Node cur = head;
            //链表 快慢指针就这么写 直接记住
            while (cur.next != null && cur.next.next != null)
            {
                right = right.next;
                cur = cur.next.next;
            }

            Stack<Node> stack = new Stack<Node>();
            while (right != null)
            {
                stack.Push(right);
                right = right.next;
            }

            while (stack.Count != 0)
            {
                if (stack.Pop().val != head.val)
                {
                    return false;
                }
                head = head.next;
            }
            return true;
        }
        #region 堆排序
        static void HeapInsert(int[] arr, int index)
        {
            while (arr[index] > arr[(index - 1) / 2])
            {
                Swap(arr, index, (index - 1) / 2);
                index = (index - 1) / 2;
            }
        }
        static void Heapify(int[] arr, int index, int heapSize)
        {
            //太优雅了
            while (true)
            {
                int left = index * 2 + 1;
                int right = index * 2 + 2;
                int largest = index;

                if (left < heapSize && arr[left] > arr[largest]) largest = left;
                if (right < heapSize && arr[right] > arr[largest]) largest = right;

                if (largest == index) break;        // 父节点已最大，结束

                Swap(arr, index, largest);          // 交换
                index = largest;                    // 继续下沉
            }
        }
        static void HeapSort(int[] arr)
        {
            if (arr == null || arr.Length < 2) return;

            for (int i = 0; i < arr.Length; i++)
            {
                HeapInsert(arr, i);
            }

            #region 一次性给所有的数字 则可以这样写 要快一点 但是复杂度没有变
            //for (int i = arr.Length - 1; i > 0 ; i++)
            //{
            //    Heapify(arr, i, arr.Length);
            //}
            #endregion

            int heapSize = arr.Length;

            heapSize--;
            Swap(arr, 0, heapSize);
            while (heapSize > 0)
            {
                Heapify(arr, 0, heapSize);
                heapSize--;
                Swap(arr, 0, heapSize);
            }
        }
        #endregion
        static void Swap(int[] arr, int i, int j)
        {
            //要保证 i != j
            if (i == j) return;
            arr[i] = arr[i] ^ arr[j];
            arr[j] = arr[i] ^ arr[j];
            arr[i] = arr[i] ^ arr[j];
        }
    }
    /// <summary>
    /// Definition for singly-linked list.
    /// </summary>
    public class Node
    {
        public int val;
        public Node next;

        //有些题有一个随机指针
        public Node random;
        public Node(int val)
        {
            this.val = val;
        }
    }
    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }

    // Definition for a binary tree node.
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }
}