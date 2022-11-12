namespace DataStructuresMQ
{
    public class Node<T>
    {
        public T Value { get; set; }
        internal Node<T> next { get; set; }
        internal Node<T> previous { get; set; }
        public Node(T value) => this.Value = value;
    }
}