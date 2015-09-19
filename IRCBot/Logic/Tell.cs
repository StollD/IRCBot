using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IRCBot
{
    // Class to store logical functions
    public partial class Logic
    {
        // Store messages for other users
        public static void Tell()
        {
            // Check if there are messages for the user
            foreach (var user in seenTell.tell)
            {
                string wildcard = "^" + Regex.Escape(user.Key).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
                if (!Regex.IsMatch(message.User.Nick, wildcard))
                    continue;

                try
                {
                    foreach (List<object> tell in seenTell.tell[user.Key])
                    {
                        // 1. name of channel
                        // 2. name of user
                        // 3. datetime
                        // 4. message
                        // 5. private / public
                        string channelName = tell[0] as string;
                        string sender = tell[1] as string;
                        DateTime time = (DateTime)tell[2];
                        string msg = tell[3] as string;
                        bool pn = (bool)tell[4];
                        string[] targets = pn ? new[] { message.User.Nick } : new[] { channel.Name };
                        SendMessage(message.User.Nick + ": \x02\u000312" + sender + "\u0003\x02 left a message for you in \x02\u000307" + channelName + "\u0003 \u000306[" + time.ToString("dd.MM.yyyy HH:mm:ss") + "]\u0003\x02: \u000310\"" + msg + "\"\u0003", targets);
                    }
                    seenTell.tell[user.Key].Clear();
                    Utils.Save(seenTell);
                }
                catch { }
            }
        }
    }
}
