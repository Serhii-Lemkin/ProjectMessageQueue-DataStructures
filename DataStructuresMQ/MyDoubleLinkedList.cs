using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresMQ
{
    public class MyDoubleLinkedList<T> : IEnumerable<T>
    {
        Node<T> start;
        public Node<T> End { get; private set; }
        public int Count { get; protected set; }
        public MyDoubleLinkedList() => Count = 0;
        public void AddFirst(T item)
        {
            Node<T> n = new Node<T>(item);
            n.next = start;
            start = n;
            if (start.next != null) start.next.previous = start;
            if (End == null) End = n;
            Count++;
        }
        public void AddLast(T item)
        {
            if (start == null)
            {
                AddFirst(item);
                return;
            }
            End.next = new Node<T>(item);
            End.next.previous = End;
            End = End.next;
            Count++;
        }
        public bool RemoveNode(Node<T> nodeToDelete)
        {
            Count--;
            if (nodeToDelete.next == null && nodeToDelete.previous == null) { RemoveFirst(); return true; }            
            if (nodeToDelete.next == null) { RemoveLast(); return true; }
            if (nodeToDelete.previous == null) { RemoveFirst(); return true; }
            nodeToDelete.previous.next = nodeToDelete.next;
            nodeToDelete.next.previous = nodeToDelete.previous;
            return true;
        }
        public bool RemoveFirst()
        {
            if (start == null) return false;
            start = start.next;
            if (start == null) End = null;
            else start.previous = null;
            Count--;
            return true;
        }
        public bool RemoveLast()
        {
            if (End == null) return false;
            Count--;
            if (start == End)
            {
                End = start = null;
                return true;
            }
            End = End.previous;
            End.next = null;
            return true;

        }
        public bool GetAt(int index, out T item)
        {
            item = default;
            if (index >= Count || index < 0) return false;
            if (start == null) return false;
            Node<T> node = start;
            for (int i = 0; i < index; i++) node = node.next;
            item = node.Value;
            return true;
        }
        public bool RemoveAt(int index)
        {
            if (index > Count || index < 0) return false;
            if (index == 0) { RemoveFirst(); return true; }
            else if (index == Count) { RemoveLast(); return true; }
            else
            {
                Node<T> node = start;
                for (int i = 0; i < index; i++) node = node.next;
                node.previous.next = node.next;
                node.next.previous = node.previous;
                Count--;
                return true;
            }
        }
        public bool AddAt(int index, T item)
        {
            if (index > Count || index < 0) return false;
            if (index == 0) { AddFirst(item); return true; }
            else if (index == Count) { AddLast(item); return true; }
            else
            {
                Node<T> node = start;
                for (int i = 0; i < index; i++) node = node.next;
                Node<T> newNode = new Node<T>(item);
                newNode.previous = node.previous;
                newNode.previous.next = newNode;
                newNode.next = node;
                node.previous = newNode;
                Count++;
                return true;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = start;
            if (current == null) yield break;
            while (current != null)
            {
                yield return current.Value;
                current = current.next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}