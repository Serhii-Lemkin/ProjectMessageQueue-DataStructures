using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresMQ
{
    internal class MySingleLinkedList<T> : IEnumerable
    {
        
        Node<T> start;
        Node<T> end;
        public int Count { get; private set; }
        public void AddFirst(T value) 
        {
            Node<T> n = new Node<T>(value);
            n.next = start;
            start = n;
            if (end == null) end = n;
            Count++;
        }

        internal bool RemoveLast()
        {
            if (end == null) return false;
            if (start == end)
            {
                end = start = null;
                return true;
            }
            Count--;
            Node<T> tmp = start;
            Node<T> preTmp = start;
            while (tmp.next != null)
            {
                preTmp = tmp;
                tmp = tmp.next;
            }
            end = preTmp;
            end.next = null;
            return true;
        }
        public void AddLast(T value)  
        {
            if (start == null) { AddFirst(value); return; }
            Node<T> n = new Node<T>(value);
            end.next = n;
            end = n;
            Count++;
        }
        public bool RemoveFirst() 
        {
            if (start == null) return false;
            else
            {
                start = start.next;
                if (start == null) end = start;
                Count--;
                return true;
            }
        }
        public bool GetAt(int index, out T item) 
        {
            item = default; 
            if (index < 0 || index >= Count) return false;

            Node<T> tmp = start;
            for (int i = 0; i < index; i++) tmp = tmp.next; 
            item = tmp.Value;
            return true;
        }
        public override string ToString()
        {
            StringBuilder allValues = new StringBuilder();

            Node<T> tmp = start;
            while (tmp != null)
            {
                allValues.Append(tmp.Value + " ");
                tmp = tmp.next;
            }
            return allValues.ToString();
        }
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = start;
            while (current != null)
            {
                yield return current.Value;
                current = current.next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
