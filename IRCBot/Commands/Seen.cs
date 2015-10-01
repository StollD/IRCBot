using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Tell
        [Command("seen")]
        public static void Seen(string msg)
        {
            // Get the name
            string name = Utils.Remove(msg, "seen", true);

            // Sort by date
            KeyValuePair<string, List<object>> lastUser = new KeyValuePair<string, List<object>>();

            // Check the lists
            foreach (var user in seenTell.seen)
            {
                string wildcard = "^" + Regex.Escape(name).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
                if (!Regex.IsMatch(user.Key, wildcard))
                    continue;

                // Null check
                if (lastUser.Value == null)
                    lastUser = user;

                // Sort by date
                if (((DateTime)user.Value[1]) > ((DateTime)lastUser.Value[1]))
                    lastUser = user;
            }

            if (lastUser.Value != null)
            { 
                // Return the answer
                string channelName = lastUser.Value[0] as string;
                DateTime time = (DateTime)lastUser.Value[1];
                string mess = lastUser.Value[2] as string;
                SendMessage(
                    message.User.Nick + 
                    ": I last saw \x02\u000312" +
                    lastUser.Key + "\u0003\x02 on \x02\u000306[" + 
                    time.ToString("dd.MM.yyyy HH:mm:ss") + 
                    "]\u0003\x02 in \x02\u000307" + 
                    channelName +
                    "\u0003\x02 saying: \u000310\"" + mess + "\"\u0003");
                return;
            }

            SendMessage(message.User.Nick + ": I haven't seen this user yet.");
        }
    }
}