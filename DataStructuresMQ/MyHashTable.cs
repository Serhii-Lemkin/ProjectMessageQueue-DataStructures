using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataStructuresMQ
{
    public class MyHashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        MyDoubleLinkedList<Data>[] hashArray;
        public int ItemsCount = 0;
        public TValue this[TKey key]
        {
            get { return GetValue(key); }
            
        }
        public MyHashTable(int capacity = 1024) => hashArray = new MyDoubleLinkedList<Data>[capacity];
        public void Add(TKey key, TValue value)
        {
            int ind = KeyToIndex(key);
            if (hashArray[ind] == null) hashArray[ind] = new MyDoubleLinkedList<Data>();
            else if (ContainsKey(key))
                throw new ArgumentException($"An item with the same key: {key} has already been added.");
            hashArray[ind].AddLast(new Data(key, value));
            ItemsCount++;
            if (ItemsCount > hashArray.Length) ReHash();
        }
        public MyDoubleLinkedList<TKey> GetKeys()
        {
            var tmp = new MyDoubleLinkedList<TKey>();
            foreach (var item in hashArray)
                if (item != null)
                    foreach (var subItem in item)
                        tmp.AddLast(subItem.key);
            return tmp;
        }
        public void Remove(TKey key)
        {
            int ind = KeyToIndex(key);
            if (hashArray[ind] != null)
            {
                int index = 0;
                foreach (var data in hashArray[ind])
                {
                    if (data.key.Equals(key))
                    {
                        hashArray[ind].RemoveAt(index);
                        if (hashArray[ind].Count == 0) hashArray = null;
                        return;
                    }
                    index++;
                }
            }
        }
        private void ReHash()
        {
            var tmp = hashArray;
            hashArray = new MyDoubleLinkedList<Data>[hashArray.Length * 2];
            ItemsCount = 0;

            foreach (var list in tmp)
                if (list != null)
                    foreach (Data keyValueItem in list)
                        Add(keyValueItem.key, keyValueItem.value);
        }
        public double CalcAverLoad() => hashArray.Where(lst => lst != null).Average(lst => lst.Count);
        public TValue GetValue(TKey key)
        {
            int ind = KeyToIndex(key);
            Data keyValue;
            if (hashArray[ind] != null)
            {
                keyValue = hashArray[ind].FirstOrDefault(item => item.key.Equals(key));
                if (keyValue != null) return keyValue.value;
            }
            throw new KeyNotFoundException($"No such key: {key}");
        }
        public bool ContainsKey(TKey key)
        {
            int ind = KeyToIndex(key);
            if (hashArray[ind] == null) return false;
            return hashArray[ind].Any(item => item.key.Equals(key));
        }
        private int KeyToIndex(TKey key)
            => Math.Abs((key.GetHashCode() + hashArray.Length) % hashArray.Length);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var item in hashArray)
                if (item != null)
                    foreach (var subItem in item)
                        yield return new KeyValuePair<TKey, TValue>(subItem.key, subItem.value);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        class Data
        {
            public TKey key;
            public TValue value;
            public Data(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
