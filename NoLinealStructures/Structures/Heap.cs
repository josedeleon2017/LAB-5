using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoLinealStructures.Interfaces;
using NoLinealStructures.Structures;

namespace NoLinealStructures.Structures 
{
    public class Heap<T> : Interfaces.IHeapStructure<T>
    {
        public Node<T> Root { get; set; }
        public static int Count = 0;
        public Delegate GetPriorityValue;
        public Delegate Comparer;

        public void Add(T value)
        {
            Node<T> node = new Node<T>(value);
            if (Root == null && Count == 0)
            {
                Root = node;
                Count++;
            }
            // int s = (int)GetPriorityValue.DynamicInvoke(value);
            else
            {
                if (Count == 1)
                {
                    Root.Left = node;
                    Count++;
                    if ((int)GetPriorityValue.DynamicInvoke(node.Value) < (int)GetPriorityValue.DynamicInvoke(Root.Value))
                    {
                        SwapNodeValue(Root, node);
                    }
                }
                else if (Count == 2)
                {
                    Root.Right = node;
                    Count++;
                    if ((int)GetPriorityValue.DynamicInvoke(node.Value) < (int)GetPriorityValue.DynamicInvoke(Root.Value))
                    {
                        SwapNodeValue(Root, node);
                    }
                }
                else
                {
                    string address = GetAddress(Count + 1);
                    Node<T> NodeTemp = Root;
                    for (int i = 0; i < address.Length-1; i++)
                    {
                        if (address[i] == '0')
                        {
                            NodeTemp = NodeTemp.Left;
                        }
                        else if (address[i] == '1')
                        {
                            NodeTemp = NodeTemp.Right;
                        }
                    }
                    if (address[address.Length - 1] == '0')
                    {
                        NodeTemp.Left = node;
                        Count++;
                        Node<T> Parent = GetParent(NodeTemp.Left);
                        Node<T> Next = Parent.Left;
                        while(Parent != null)
                        {
                            if ((int)GetPriorityValue.DynamicInvoke(Next.Value) < (int)GetPriorityValue.DynamicInvoke(Parent.Value))
                            {
                                SwapNodeValue(Parent, Next);
                                Next = Parent;
                                Parent = GetParent(Parent);
                            }
                            else
                            {
                                Parent = null;
                            }
                        }
                    }
                    else if (address[address.Length - 1] == '1')
                    {
                        NodeTemp.Right = node;
                        Count++;
                        Node<T> Parent = GetParent(NodeTemp.Right);
                        Node<T> Next = Parent.Right;
                        while (Parent != null)
                        {
                            if ((int)GetPriorityValue.DynamicInvoke(Next.Value) < (int)GetPriorityValue.DynamicInvoke(Parent.Value))
                            {
                                SwapNodeValue(Parent, Next);
                            }
                            Next = Parent;
                            Parent = GetParent(Parent);
                        }
                    }
                    
                }
            }
        }

        public int Find(T value)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoot(T value)
        {
            throw new NotImplementedException();
        }

        private void ReSort()
        {
            throw new NotImplementedException();
        }
        private string GetAddress(int node)
        {
            string address = Convert.ToString(node, 2);
            return address.Substring(1, address.Length - 1);
        }
        private void SwapNodeValue(Node<T> Parent, Node<T> Next)
        {
            Node<T> temp = new Node<T>(Parent.Value);
            Parent.Value = Next.Value;
            Next.Value = temp.Value;
        }
        private Node<T> GetParent(Node<T> Next)
        {
            if (Count == 0 || Count == 1)
            {
                return null;
            }
            else if (Count == 2 || Count == 3)
            {
                return Root;
            }
            else
            {
                for (int i = 3; i <= Count; i++)
                {
                    string address = GetAddress(i);
                    Node<T> Node = Root;
                    for (int j = 0; j < address.Length; j++)
                    {
                        if ((int)Comparer.DynamicInvoke(Next.Value, Node.Left.Value) == 0 || (int)Comparer.DynamicInvoke(Next.Value, Node.Right.Value) == 0)
                        {
                            return Node;
                        }
                        else if (address[j] == '0')
                        {
                            Node = Node.Left;
                        }
                        else if(address[j] == '1')
                        {
                            Node = Node.Right;
                        }
                    }
                }
            }
            return null;
        }

    }
}
