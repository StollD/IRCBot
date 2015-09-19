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
        // When a user was seen
        public static void Seen()
        {
            // If there is no list,create one
            if (!seenTell.seen.ContainsKey(message.User.Nick))
                seenTell.seen.Add(message.User.Nick, new List<object>());
            seenTell.seen[message.User.Nick] = new List<object>
            {
                channel.Name,
                DateTime.Now,
                message.Message
            };
            Utils.Save(seenTell);
        }
    }
}
