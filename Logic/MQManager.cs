using DataStructuresMQ;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logic
{
    public class MQManager
    {
        MyHashTable<string, MySLLQueue<Node<Message>>> HashCol { get; set; }
        MyDLLQueue<Message> QueueCol { get; set; }
        public MQManager()
        {
            HashCol = new MyHashTable<string, MySLLQueue<Node<Message>>>(50);
            QueueCol = new MyDLLQueue<Message>();
        }
        /// <summary>
        /// Add message to group by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public void SendMessage(string key, string message)
        {
            var newMessage = new Message(message, key);
            QueueCol.Enqueue(newMessage);
            if (!HashCol.ContainsKey(key)) HashCol.Add(key, new MySLLQueue<Node<Message>>());
            HashCol[key].Enqueue(QueueCol.GetEnd);
        }
        /// <summary>
        /// Gets Oldest Message in a group with given key.
        /// </summary>
        /// <param name="key">GroupKey</param>
        /// <param name="message">Message</param>
        /// <returns></returns>
        public bool GetOldest(string key, out Node<Message> message)
        {
            if (!HashCol.ContainsKey(key))
            {
                message = default;
                return false;
            }
            return HashCol[key].Peek(out message);
        }
        public bool GetOldest(string key, out Message message)
        {
            if (!HashCol.ContainsKey(key))
            {
                message = default;
                return false;
            }
            var success = HashCol[key].Peek(out Node<Message> node);

            if (node != null) message = node.Value;
            else message = default;
            return success;
        }
        /// <summary>
        /// Gets the oldest Message in system.
        /// </summary>
        /// <param name="message">The oldest message</param>
        /// <returns></returns>
        public bool GetOldest(out Message message) => QueueCol.Peek(out message);
        public bool GetNewest(out Message tmp) => QueueCol.GetAt(QueueCol.Count - 1, out tmp);
        public MyDoubleLinkedList<Message> GetXNewest(int number)
        {
            var newest = new MyDoubleLinkedList<Message>();
            while (number > 0)
            {
                QueueCol.GetAt(QueueCol.Count - number, out Message message);
                number--;
                if (message != null) newest.AddFirst(message);
            }
            return newest;
        }
        public MyDoubleLinkedList<Message> GetXOldest(int number)
        {
            var oldest = new MyDoubleLinkedList<Message>();
            int i = 0;
            foreach (var message in QueueCol)
            {
                if (i++ >= number) break;
                oldest.AddLast(message);
            }
            return oldest;
        }
        public MyDoubleLinkedList<Message> GetMessagesFromGroup(string key)
        {
            var tmp = new MyDoubleLinkedList<Message>();
            foreach (var m in HashCol[key]) tmp.AddLast(m.Value);
            return tmp;
        }
        public MyDoubleLinkedList<string> GetKeys()
        {
            var tmp = new MyDoubleLinkedList<string>();
            foreach (var item in HashCol) tmp.AddLast(item.Key);
            return tmp;
        }
        public bool RemoveOldestMessage()
        {
            QueueCol.GetAt(0, out Message tmpFromQueue);
            HashCol[tmpFromQueue.Key].Peek(out Node<Message> tmpFromHash);
            if (tmpFromQueue.Equals(tmpFromHash.Value))
            {
                HashCol[tmpFromQueue.Key].Dequeue();
                QueueCol.Dequeue();
                return true;
            }
            return false;
        }
        public MyDoubleLinkedList<Message> GetOlderThan(DateTime givenDT)
        {
            var tmpList = new MyDoubleLinkedList<Message>();
            foreach (var message in QueueCol)
            {
                if (message.MessageDate.CompareTo(givenDT) < 0)
                    tmpList.AddLast(message);
            }
            return tmpList;
        }
        public MyDoubleLinkedList<Message> GetNewerThan(DateTime givenDT)
        {
            var tmpList = new MyDoubleLinkedList<Message>();
            foreach (var message in QueueCol)
            {
                if (message.MessageDate.CompareTo(givenDT) > 0)
                    tmpList.AddLast(message);
            }
            return tmpList;
        }
        /// <summary>
        /// Read gets the last message from system and removes it from system.
        /// </summary>
        public bool ReadLast(out Message tmp)
        {
            tmp = null;
            if (QueueCol.Count == 0) return false;
            QueueCol.GetAt(0, out tmp);
            RemoveOldestMessage();
            return true;
        }
        /// <summary>
        /// Read with key gets the last message from spesific group and removes it from system
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ReadLast(string key, out Message message)
        {
            HashCol[key].Peek(out Node<Message> tmpFromHash);
            if (tmpFromHash == null)
            {
                message = null;
                return false;
            }
            message = tmpFromHash.Value;
            if (tmpFromHash.Value.Equals(QueueCol.FirstOrDefault(x => x.Key == key)))
            {
                QueueCol.RemoveNode(tmpFromHash);
                HashCol[key].Dequeue();
                return true;
            }
            return false;
        }
        public bool ContainsKey(string key) => HashCol.ContainsKey(key);
        public MyDoubleLinkedList<Message> GetMessagesByWord(string word)
        {
            var selectedMessages = new MyDoubleLinkedList<Message>();
            foreach (var item in QueueCol)
            {
                var wordArray = Regex.Split(item.MessageTxt, @"\W+");
                foreach (var w in wordArray)
                    if (word.ToLower() == w.ToLower())
                    {
                        selectedMessages.AddLast(item);
                        break;
                    }
            }
            return selectedMessages;
        }
        private static MQManager instance;
        public static MQManager Instance => instance ?? (instance = new MQManager());
    }
}
