using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Tell
        [Command("tell")]
        public static void Tell(string msg)
        {
            // Get the attributes
            string args = Utils.Remove(msg, "tell", true);

            // Get the username etc
            int i = 0;
            for (i = 0; i < args.Length; i++)
                if (args[i] == ' ')
                    break;
            string name = args.Remove(i);
            args = args.Substring(i).Trim();

            // Private
            bool pn = Is(args, "private");

            // Message
            string mess = Utils.Remove(args, "private");

            // Check if the list exists
            if (!seenTell.tell.ContainsKey(name))
                seenTell.tell.Add(name, new List<List<object>>());
            seenTell.tell[name].Add(new List<object>() { channel.Name, message.User.Nick, DateTime.Now, mess, pn });
            SendMessage(message.User.Nick + ": I'll redirect this as soon as he/she/it is around.");
            Utils.Save(seenTell);
        }
    }
}