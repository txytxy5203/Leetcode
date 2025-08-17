namespace Leetcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TreeNode a = new TreeNode(1);
            TreeNode b = new TreeNode(2);
            TreeNode c = new TreeNode(3);
            TreeNode d = new TreeNode(4);
            TreeNode e = new TreeNode(5);
            TreeNode f = new TreeNode(6);
            TreeNode g = new TreeNode(7);
            a.left = b; a.right = c;
            b.left = d; b.right = e;
            d.left = null; d.right = null;
            e.left = null; e.right = null;
            c.left = f; c.right = g;
            f.left = null; f.right = null;
            g.left = null; g.right = null;
            //TreeNode a5 = new TreeNode(5);
            //TreeNode a3 = new TreeNode(3);
            //TreeNode a7 = new TreeNode(7);
            //TreeNode a2 = new TreeNode(2);
            //TreeNode a4 = new TreeNode(4);
            //TreeNode a6 = new TreeNode(6);
            //TreeNode a8 = new TreeNode(8);
            //TreeNode a1 = new TreeNode(1);
            //a5.left = a3; a5.right = a7;
            //a3.left = a2; a3.right = a4;
            //a7.left = a6; a7.right = a8;
            //a2.left = a1; a2.right = null;
            //a4.left = null; a4.right = null;
            //a6.left = null; a6.right = null;
            //a8.left = null; a8.right = null;
            //a1.left = null; a1.right = null;


            Console.WriteLine(LowestCommonAncestor2(a, d, e).val);


        }
        #region 二叉树
        static TreeNode GetSuccessorNode(TreeNode node)
        {

        }
        static TreeNode LowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q)
        {
            // https://leetcode.cn/problems/lowest-common-ancestor-of-a-binary-tree/description/
            // 一般的直观解法
            // 先遍历
            Dictionary<TreeNode, TreeNode> dic = new Dictionary<TreeNode, TreeNode>();
            dic.Add(root, root);
            LowestCommonAncestorRecur(root, dic);

            HashSet<TreeNode> ancestor = new HashSet<TreeNode>();
            TreeNode curr = p;
            // 记得把自己也先添加进去
            ancestor.Add(curr);
            while (dic[curr] != curr)
            {
                ancestor.Add(dic[curr]);
                curr = dic[curr];
            }

            curr = q;
            while (!ancestor.Contains(curr))
            {
                curr = dic[curr];
            }
            return curr;
        }
        static void LowestCommonAncestorRecur(TreeNode node, Dictionary<TreeNode, TreeNode> dict)
        {
            if (node == null)
                return;
            if (node.left != null)
                dict.Add(node.left, node);
            if (node.right != null)
                dict.Add(node.right, node);
            LowestCommonAncestorRecur(node.left, dict);
            LowestCommonAncestorRecur(node.right, dict);
        }
        static TreeNode LowestCommonAncestor2(TreeNode root, TreeNode p, TreeNode q)
        {
            // 所以说递归本质上就是把问题分成更小的相似的问题  从root节点去向下找  信息会从底下返回上来
            // 实际上实现了一个“双向树”  如果一个节点能够知道自己的父节点那么这题就好办了
            // 基础情况
            if (root == null || root == p || root == q) 
                return root;
            TreeNode left = LowestCommonAncestor2(root.left, p, q);
            TreeNode right = LowestCommonAncestor2(root.right, p, q);

            if (left != null && right != null)
                return root;
            // 这一句要好好理解  自己写一下就明白了所有情况
            return left != null ? left : right; 
        }
        static bool IsBalanced(TreeNode root)
        {
            // https://leetcode.cn/problems/balanced-binary-tree/description/
            // 左神的套路递归写法  每个Node都向自己的左右孩子要信息
            // 递归仔细体会一下也就那样嘛  加油别怕
            return IsBalancedRecur(root).isBalanced;
        }
        static (bool isBalanced, int depth) IsBalancedRecur(TreeNode root)
        {
            // 这里的递归套路一定要仔细体会一下
            if (root == null)
                return new(true, 0);
            (bool isBalancedLeft, int depthLeft) = IsBalancedRecur(root.left);
            (bool isBalancedRight, int depthRight) = IsBalancedRecur(root.right);
            return (isBalancedLeft && isBalancedRight && Math.Abs(depthRight - depthLeft) <= 1,
                Math.Max(depthLeft, depthRight) + 1);
        }
        static bool IsFullTree(TreeNode root)
        {
            // 自己想的方法 简单直观
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(root);

            // 总结点数
            int totalNum = 1;
            // 深度  这里初始化为 -1
            int depth = -1;
            while (stack.Count > 0)
            {
                int curLayerNum = stack.Count;
                depth++;
                for (int i = 0; i < curLayerNum; i++)
                {
                    TreeNode node = stack.Pop();
                    if (node.left != null)
                    {
                        stack.Push(node.left);
                        totalNum++;
                    }
                    if (node.right != null)
                    {
                        stack.Push(node.right);
                        totalNum++;
                    }
                }
            }
            if (totalNum == Math.Pow(2, depth) - 1)
                return true;
            return false;
        }
        static bool IsCompleteTree(TreeNode root)
        {
            // https://leetcode.cn/problems/check-completeness-of-a-binary-tree/description/
            // 依旧是层序遍历的思想  每一次弹出时和上一次弹出比较
            if (root == null) return true;
            Queue<(TreeNode, int)> queue = new Queue<(TreeNode, int)>();

            int currId = 0;
            int preNodeId = -1;

            queue.Enqueue((root, currId));
            while (queue.Count > 0)
            {
                // 一次性把一层的节点全部弹出  这样就做到了层序遍历
                int curLayerCount = queue.Count;

                for (int i = 0; i < curLayerCount; i++)
                {
                    TreeNode node;
                    (node, currId) = queue.Dequeue();
                    // 每一次弹出的node Id 和上一次的Node Id比较
                    if (currId != preNodeId + 1)
                        return false;
                    preNodeId = currId;

                    if (node.left != null)
                    {
                        queue.Enqueue((node.left, currId * 2 + 1));
                    }
                    if (node.right != null)
                    {
                        queue.Enqueue((node.right, currId * 2 + 2));
                    }
                }
            }
            return true;
        }
        static bool IsCompleteTree2(TreeNode root)
        {
            // 左神的解法      
            if (root == null) return true;
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            bool checkLeaf = false;
            while (queue.Count > 0)
            {
                TreeNode node;
                node = queue.Dequeue();

                //if (checkLeaf)
                //{
                //    if (node.left != null || node.right != null)
                //        return false;
                //}

                //if (node.left == null && node.right != null)
                //    return false;
                // 上面的简写就变成这样了
                // 1 遇到第一个子节点不全的节点后  如果有不是叶节点的节点 则return false
                // 2 有右孩子但是没有左孩子的 则return false
                if ((checkLeaf && (node.left != null || node.right != null))
                    || (node.left == null && node.right != null))
                {
                    return false;
                }

                if (node.left != null)
                {
                    queue.Enqueue(node.left);
                }
                if (node.right != null)
                {
                    queue.Enqueue(node.right);
                }
                else
                {
                    checkLeaf = true;
                }
            }
            return true;
        }
        static bool IsValidBST(TreeNode root)
        {
            // https://leetcode.cn/problems/validate-binary-search-tree/description/
            // 傻白甜写法
            IList<int> list = new List<int>();
            //中序遍历的时候已经生成了list 检查list即可
            inOrderRecur(root, list);
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (!(list[i] < list[i + 1]))
                    return false;
            }
            return true;
        }
        static bool IsValidBST2(TreeNode root)
        {
            // LeetCode题解中的解法   好好看看 仔细体会一下递归的思想
            return IsValidBST2(root, long.MinValue, long.MaxValue);
        }
        static bool IsValidBST2(TreeNode node, long lower, long upper)
        {
            if (node == null)
                return true;

            if (node.val < lower || node.val > upper)
                return false;
            return IsValidBST2(node.left, lower, node.val) && IsValidBST2(node.right, node.val, upper);
        }
        static bool IsValidBST3(TreeNode root)
        {
            // 中序遍历 不使用递归
            // 这个代码要好好理解一下
            if (root == null) return true;
            Stack<TreeNode> stack = new Stack<TreeNode>();

            long temp = long.MinValue;
            while (stack.Count > 0 || root != null)
            {
                if (root != null)
                {
                    stack.Push(root);
                    root = root.left;
                }
                else
                {
                    root = stack.Pop();
                    if (root.val > temp)
                    {
                        // 一定要注意这里的顺序
                        // 先比较再赋值
                        temp = root.val;
                        root = root.right;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static bool IsValidBST4(TreeNode root)
        {
            // 左神的套路递归写法
            return IsValidBST4Recur(root).isValid;
        }
        static (bool isValid, long min, long max) IsValidBST4Recur(TreeNode root)
        {
            // 节点为空时  传最大最小值要注意
            if (root == null)
                return (true, long.MaxValue, long.MinValue);
            (bool left, long leftMin, long leftMax) = IsValidBST4Recur(root.left);
            (bool right, long rightMin, long rightMax) = IsValidBST4Recur(root.right);


            // 左右子树的最大最小值逻辑一定要理清楚!!!
            return (left && right
                    && root.val > leftMax
                    && root.val < rightMin,
                    Math.Min(root.val, leftMin),
                    Math.Max(root.val, rightMax));
        }
        static int WidthOfBinaryTree(TreeNode root)
        {
            // https://leetcode.cn/problems/maximum-width-of-binary-tree/description/
            // 队列中不仅要包含它的TreeNode 还要有它的编号
            // 最后通过编号来计算宽度
            if (root == null) return 0;
            Queue<(TreeNode, int)> queue = new Queue<(TreeNode, int)>();

            int currId = 0;
            int max = 1;

            queue.Enqueue((root, currId));
            while (queue.Count > 0)
            {
                // 一次性把一层的节点全部弹出  这样就做到了层序遍历
                int temp = queue.Count;
                // 记录每一层的第一个和最后一个节点Id
                int first = 0;
                int last = 0;
                for (int i = 0; i < temp; i++)
                {
                    TreeNode node;
                    (node, currId) = queue.Dequeue();
                    if (i == 0)
                        first = currId;
                    if (i == temp - 1)
                        last = currId;

                    if (node.left != null)
                    {
                        queue.Enqueue((node.left, currId * 2 + 1));
                    }
                    if (node.right != null)
                    {
                        queue.Enqueue((node.right, currId * 2 + 2));
                    }
                }
                max = Math.Max(max, last - first + 1);
            }
            return max;
        }
        static IList<IList<int>> LevelOrder(TreeNode root)
        {
            // https://leetcode.cn/problems/binary-tree-level-order-traversal/description/
            // 二叉树的层序遍历 也叫 宽度遍历   BFS   leetcode上面的题解讲得很好
            // https://leetcode.cn/problems/binary-tree-level-order-traversal/solutions/244853/bfs-de-shi-yong-chang-jing-zong-jie-ceng-xu-bian-l/
            if (root == null) return new List<IList<int>>();
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            IList<IList<int>> res = new List<IList<int>>();

            while (queue.Count > 0)
            {
                // 一次性把一层的节点 全部弹出  这样就做到了层序遍历
                int temp = queue.Count;
                IList<int> list = new List<int>();
                for (int i = 0; i < temp; i++)
                {
                    TreeNode node = queue.Dequeue();
                    list.Add(node.val);

                    if (node.left != null)
                    {
                        queue.Enqueue(node.left);
                    }
                    if (node.right != null)
                    {
                        queue.Enqueue(node.right);
                    }
                }
                res.Add(list);
            }
            return res;
        }
        static IList<int> InorderTraversal(TreeNode root)
        {
            IList<int> result = new List<int>();
            preOrderRecur(root, result);
            return result;
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
        static void preOrderRecur(TreeNode head)
        {
            //前序遍历 在第一次进入节点时 执行逻辑
            if (head == null)
                return;
            preOrderRecur(head.left);
            preOrderRecur(head.right);
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

        static void preOrderUnRecur(TreeNode head)
        {
            //前序遍历 不使用递归的方式
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
        static void inOrderUnRecur(TreeNode head)
        {
            // 中序遍历 不使用递归
            // 这个代码要好好理解一下
            // 先让左边的进栈
            if (head == null) return;
            Stack<TreeNode> stack = new Stack<TreeNode>();

            while (stack.Count > 0 || head != null)
            {
                if (head != null)
                {
                    stack.Push(head);
                    head = head.left;
                }
                else
                {
                    head = stack.Pop();
                    Console.WriteLine(head.val);
                    head = head.right;
                }
            }
        }

        static void posOrderUnRecur(TreeNode head)
        {
            //后续遍历  不使用递归的方式
            if (head == null) return;
            Stack<TreeNode> stack = new Stack<TreeNode>();
            Stack<TreeNode> printStack = new Stack<TreeNode>();
            stack.Push(head);
            while (stack.Count > 0)
            {
                TreeNode node = stack.Pop();
                printStack.Push(node);
                if (node.left != null)
                    stack.Push(node.left);
                if (node.right != null)
                    stack.Push(node.right);
            }
            while (printStack.Count > 0)
            {
                Console.WriteLine(printStack.Pop().val);
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
                if (currA.next != null)
                {
                    currA = currA.next;
                    lengthA++;
                }
                if (currB.next != null)
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
                while (offset > 0)
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
    public class TreeNode2
    {
        // 能够知道自己的父节点
        public int val;
        public TreeNode2 left;
        public TreeNode2 right;
        public TreeNode2 parent;
        public TreeNode2(int _val)
        {
            val = _val;
        }
    }
}