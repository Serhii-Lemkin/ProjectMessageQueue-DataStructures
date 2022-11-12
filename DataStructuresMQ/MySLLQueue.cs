using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresMQ
{
    public class MySLLQueue<T> : IEnumerable<T>
    {
        public int Count => SLLQueue.Count;
        MySingleLinkedList<T> SLLQueue { get; set; }
        public MySLLQueue() => SLLQueue = new MySingleLinkedList<T>();
        public void Enqueue(T value) => SLLQueue.AddLast(value);
        public bool Peek(out T value) => SLLQueue.GetAt(0, out value);
        public bool GetAt(int ind, out T value) => SLLQueue.GetAt(ind, out value);
        public bool Dequeue() => SLLQueue.RemoveFirst();
        public IEnumerator<T> GetEnumerator() => SLLQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
