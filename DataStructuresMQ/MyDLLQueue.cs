using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresMQ
{
    public class MyDLLQueue<T> : IEnumerable<T>
    {
        MyDoubleLinkedList<T> queue = new MyDoubleLinkedList<T> ();
        public Node<T> GetEnd => queue.End;
        public int Count => queue.Count;
        public void Enqueue(T value) => queue.AddLast(value);
        public bool Peek(out T value) => queue.GetAt(0, out value);
        public bool GetAt(int ind, out T value) => queue.GetAt(ind, out value);
        public void Dequeue() => queue.RemoveFirst();
        public void RemoveNode(Node<T> nodeToDelete) => queue.RemoveNode(nodeToDelete);
        public IEnumerator<T> GetEnumerator() => queue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
