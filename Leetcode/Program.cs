namespace Leetcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //PriorityQueue<int,int> heap = new PriorityQueue<int, int>();
            


            HashSet<string> set = new HashSet<string>();
            ListNode a = new ListNode(1);
            ListNode b = new ListNode(2);
            ListNode c = new ListNode(3);
            ListNode d = new ListNode(2);
            ListNode e = new ListNode(2);
            a.next = b;
            b.next = c;
            c.next = d;
            d.next = e;
            e.next = null;
            Console.WriteLine(IsPalindrome2(a));
        }
        static ListNode CopyRandomList(ListNode head)
        {
            if (head == null) return null;
            ListNode curr = head;
            //一定要用一个 dict 不然就找不到对应关系了
            Dictionary<ListNode, ListNode> map = new Dictionary<ListNode, ListNode>();
            //先复制 节点
            while (curr != null)
            {
                map.Add(curr, new ListNode(curr.val));
                curr = curr.next;
            }

            curr = head;
            while (curr != null)
            {
                //注意这里去map中找 不能 new 了
                //一定要注意 rand 可能为null
                map[curr].next = curr.next != null ? map[curr.next] : null;
                map[curr].rand = curr.rand != null ? map[curr.rand] : null;


                curr = curr.next;
            }

            return map[head];
        }
        static ListNode CopyRandomList2(ListNode head)
        {
            if (head == null) return null;

            ListNode curr = head;
            //利用 链表的位置关系 将 Dict 省去了
            while (curr != null)
            {
                ListNode newCurr = new ListNode(curr.val);
                ListNode next = curr.next;
                curr.next = newCurr;
                newCurr.next = next;

                //移动指针
                curr = next;
            }

            curr = head;
            while (curr != null)
            {
                //链表类的题目 要记得先记录 不然后面改变了 就不对了
                ListNode next = curr.next.next;
                curr.next.next = curr.next.next.next;
                curr.next.rand = curr.rand?.next;

                //一定记得不能破坏老链表的 结构
                curr.next = next;
                curr = curr.next;
            }
            return head.next;
        }
        static ListNode Partition(ListNode head, int x)
        {
            ListNode sh = null;
            ListNode st = null;
            ListNode eh = null;
            ListNode et = null;
            ListNode bh = null;
            ListNode bt = null;

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
        static bool IsPalindrome(ListNode head)
        {
            Stack<ListNode> stack = new Stack<ListNode>();
            ListNode curr = head;
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
        static bool IsPalindrome2(ListNode head)
        {
            //捣乱的先 return
            if (head == null || head.next == null)
            {
                return true;
            }

            //快慢指针 找到中点
            ListNode right = head.next;
            ListNode cur = head;
            //链表 快慢指针就这么写 直接记住
            while (cur.next != null && cur.next.next != null)
            {
                right = right.next;
                cur = cur.next.next;
            }

            Stack<ListNode> stack = new Stack<ListNode>();
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
    public class ListNode
    {
        public int val;
        public ListNode next;

        //有些题有一个随机指针
        public ListNode rand;
        public ListNode(int val)
        {
            this.val = val;
        }
    }
}
