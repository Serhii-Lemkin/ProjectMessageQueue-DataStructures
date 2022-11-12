using Logic;
using System;
using System.Text;
using System.Windows;

namespace UIMessageQueue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MQManager mqm = MQManager.Instance;
        int minutesDiff = 0;
        public MainWindow() => InitializeComponent();

        private void SendMessageClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewKey.Text) || !string.IsNullOrWhiteSpace(NewKey.Text) ||
                !string.IsNullOrEmpty(NewMessage.Text) || !string.IsNullOrWhiteSpace(NewMessage.Text))
            {
                if (!mqm.ContainsKey(NewKey.Text))
                {
                    MessageBoxResult result = MessageBox.Show("Key does not exist, would you like to add one?", "Add New Key", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            mqm.SendMessage(NewKey.Text, NewMessage.Text);
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
                else
                {
                    mqm.SendMessage(NewKey.Text, NewMessage.Text);
                    MessageBox.Show($"Message Sent Successfully to Group ({NewKey.Text})");
                }
            }
        }

        private void PeekOldestMessageClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PeekKeyTxt.Text) && string.IsNullOrWhiteSpace(PeekKeyTxt.Text))
            {
                mqm.GetOldest(out Message message);
                if (message != null) MessageBox.Show(message.ToString());
                else MessageBox.Show("No item to show");
            }
            else
            {
                mqm.GetOldest(PeekKeyTxt.Text, out Message message);
                if (message != null) MessageBox.Show(message.ToString());
                else MessageBox.Show("No item to show");
            }
        }

        private void PeekResentMessageClick(object sender, RoutedEventArgs e)
        {
            mqm.GetNewest(out Message message);
            if (message != null) MessageBox.Show(message.ToString());
            else MessageBox.Show("No item to show");
        }

        private void SearchByWordClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchByWordTxt.Text)
                && !string.IsNullOrWhiteSpace(SearchByWordTxt.Text))
            {
                var tmpList = mqm.GetMessagesByWord(SearchByWordTxt.Text);
                if (tmpList.Count == 0) { MessageBox.Show("No item to show"); return; }
                StringBuilder sb = new StringBuilder();
                foreach (var tmp in tmpList)
                {
                    sb.Append(tmp.ToString());
                    sb.Append("\n");
                    sb.Append("\n");
                }
                MessageBox.Show(sb.ToString());
            }
        }

        private void ReadOldestClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ReadByKeyTxt.Text)
                        || string.IsNullOrWhiteSpace(ReadByKeyTxt.Text))
            {
                mqm.ReadLast(out Message message);
                if (message != null) MessageBox.Show(message.ToString());
                else MessageBox.Show("No items to show");
            }
            else if (!mqm.ContainsKey(ReadByKeyTxt.Text))
            {
                MessageBox.Show("Key does not exist.");
            }
            else
            {
                mqm.ReadLast(ReadByKeyTxt.Text, out Message message);
                if (message != null) MessageBox.Show(message.ToString());
                else MessageBox.Show("No items to show");
            }
        }

        private void GetKeysClick(object sender, RoutedEventArgs e)
        {
            var tmpList = mqm.GetKeys();
            if (tmpList.Count == 0) { MessageBox.Show("No keys available"); return; }
            StringBuilder sb = new StringBuilder();
            foreach (var tmp in tmpList)
            {
                sb.Append(tmp.ToString());
                sb.Append("\n");
            }
            MessageBox.Show(sb.ToString());
        }

        private void GetBeforeDateClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(HoursTB.Text, out int hours) || hours >= 24) return;
            if (!int.TryParse(MinutesTB.Text, out int minutes) || hours >= 60) return;
            DateTime curr = Date.SelectedDate.Value.AddHours(hours).AddMinutes(minutes);
            var tmpList = mqm.GetOlderThan(curr);
            StringBuilder sb = new StringBuilder();
            foreach (var tmp in tmpList)
            {
                sb.Append(tmp.ToString());
                sb.Append("\n");
            }
            MessageBox.Show(sb.ToString());
        }

        private void GetAfterDateClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(HoursTB.Text, out int hours) || hours >= 24) return;
            if (!int.TryParse(MinutesTB.Text, out int minutes) || hours >= 60) return;
            DateTime curr = Date.SelectedDate.Value.AddHours(hours).AddMinutes(minutes);
            var tmpList = mqm.GetNewerThan(curr);
            StringBuilder sb = new StringBuilder();
            foreach (var tmp in tmpList)
            {
                sb.Append(tmp.ToString());
                sb.Append("\n");
            }
            MessageBox.Show(sb.ToString());
        }

        private void GetXOldestClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(GetXOldestTXT.Text, out int num)) return;
            var tmpList = mqm.GetXOldest(num);
            if (tmpList.Count == 0) { MessageBox.Show("No Messages"); return; }
            StringBuilder sb = new StringBuilder();
            foreach (var tmp in tmpList)
            {
                sb.Append(tmp.ToString());
                sb.Append("\n");
            }
            MessageBox.Show(sb.ToString());
        }

        private void GetXNewestClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(GetXNewestTXT.Text, out int num)) return;
            StringBuilder sb = new StringBuilder();
            var tmpList = mqm.GetXNewest(num);
            if (tmpList.Count == 0) { MessageBox.Show("No Messages"); return; }
            if (tmpList.Count == 0)
            {
                MessageBox.Show("No Messages");
                return;
            }
            foreach (var tmp in tmpList)
            {
                sb.Append(tmp.ToString());
                sb.Append("\n");
            }
            MessageBox.Show(sb.ToString());
        }

        private void PopulateClick(object sender, RoutedEventArgs e)
        {
            SendWithDateDiff("Second", "second Message 1 Red");
            SendWithDateDiff("first", "first Message 1");
            SendWithDateDiff("first", "first Message 2 red");
            SendWithDateDiff("first", "first Message 6");
            SendWithDateDiff("Second", "second Message 2");
            SendWithDateDiff("Second", "second Message 3");
            SendWithDateDiff("Third", "third Message 1");
            SendWithDateDiff("Third", "third Message 2 Red");
            SendWithDateDiff("Fourth", "Forth Message 1");
            SendWithDateDiff("Fourth", "Forth Message 2 Blue");
            SendWithDateDiff("Fourth", "Forth Message 3");
            SendWithDateDiff("Fourth", "Forth Message 4");
            SendWithDateDiff("Fifth", "Fifth Message");
            SendWithDateDiff("Sixth", "Sixth Message 1");
            SendWithDateDiff("Sixth", "Sixth Message 2");
            SendWithDateDiff("Sixth", "Sixth Message 3 blue");
            SendWithDateDiff("Seventh", "Seventh Message 1 green");
            SendWithDateDiff("Seventh", "Seventh Message 2");
            pwdButton.IsEnabled = false;
        }
        void SendWithDateDiff(string key, string message)
        {
            mqm.SendMessage(key, message);
            mqm.GetNewest(out Message tmp);
            tmp.MessageDate = new DateTime(2022, 5, 27, 18, 30, 0).AddMinutes(minutesDiff);
            minutesDiff += 5;
        }
    }
}
