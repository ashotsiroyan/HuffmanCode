using DataTypes;
using System.Collections.Generic;

namespace HuffmanCode
{
    public class HuffmanTree
    {
        private BinaryTreeNode<char> tree;
        private Dictionary<char, int> frequencies;

        public HuffmanTree()
        {
            frequencies = new Dictionary<char, int>();
        }

        public void Create(string source)
        {
            PriorityQueue<BinaryTreeNode<char>> queue = new PriorityQueue<BinaryTreeNode<char>>();

            for (int i = 0; i < source.Length; i++)
            {
                if (!frequencies.ContainsKey(source[i]))
                {
                    frequencies.Add(source[i], 0);
                }

                frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in frequencies)
            {
                queue.Insert(new PriorityQueueNode<BinaryTreeNode<char>>(new BinaryTreeNode<char>(symbol.Key), symbol.Value));
            }

            while (queue.Size > 1)
            {
                PriorityQueueNode<BinaryTreeNode<char>> node1 = queue.ExtractMin();
                PriorityQueueNode<BinaryTreeNode<char>> node2 = queue.ExtractMin();
                int sum = node1.Priority + node2.Priority;

                BinaryTreeNode<char> treeNode = new BinaryTreeNode<char>(node1.Data, node2.Data);
                PriorityQueueNode<BinaryTreeNode<char>> node = new PriorityQueueNode<BinaryTreeNode<char>>(treeNode, sum);

                queue.Insert(node);
            }

            tree = queue.ExtractMin().Data;
        }

        public bool[] Encode(string source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = Traverse(Tree, source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            return encodedSource.ToArray();
        }

        public string Decode(bool[] bits)
        {
            BinaryTreeNode<char> current = tree;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Data;
                    current = tree;
                }
            }

            return decoded;
        }

        public bool IsLeaf(BinaryTreeNode<char> node)
        {
            return (node.Left == null && node.Right == null);
        }

        public BinaryTreeNode<char> Tree { get { return tree; } }

        private List<bool> Traverse(BinaryTreeNode<char> node, char symbol, List<bool> data)
        {
            if (node.Right == null && node.Left == null)
            {
                if (symbol.Equals(node.Data))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (node.Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Traverse(node.Left, symbol, leftPath);
                }

                if (node.Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);

                    right = Traverse(node.Right, symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
