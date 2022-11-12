using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Message
    {
        public DateTime MessageDate { get; set; }
        public string MessageTxt { get;  set; }
        public string Key { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageTxt);
            sb.Append(", \nKey = ");
            sb.Append(Key);
            sb.Append(", Date = ");
            sb.Append(MessageDate);
            return sb.ToString();
        }
        public Message(string message, string key)
        {
            MessageTxt = message;
            Key = key;
            MessageDate = DateTime.Now;
        }
    }
}
