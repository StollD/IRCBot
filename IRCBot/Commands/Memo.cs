using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Memos
        [Command("memo")]
        public static void Memo(string msg)
        {
            // Get the attributes
            string args = msg.Replace(settings.startChar + "memo", "").Trim();
            string channelname = channel.Name;

            // If a channel was defined
            if (Is(args, "@"))
            {
                channelname = args.Remove(0, 1).Split(' ').First();
                args = args.Replace("@" + channelname, "").Trim();
                channelname = "#" + channelname;
            }

            // If we have no additional commands
            if (args.Length == 0)
            {
                int count = 0;
                if (settings.memos.ContainsKey(channelname))
                    count = settings.memos[channelname].Count;

                // Response
                SendMessage(message.User.Nick + ": Currently, I have " + count + " memos stored for this channel.");
            }
            else
            {
                // If we should add a memo
                if (Is(args, "add"))
                {
                    // add it!
                    string memo = args.Replace("add", "").Trim();

                    // Get the identifier
                    string ident = Regex.Match(memo, "\\[.+\\]").Value;

                    // Error out
                    if (string.IsNullOrEmpty(ident))
                    {
                        SendMessage(message.User.Nick + ": Invalid key!");
                        return;
                    }

                    // Get the text
                    string text = memo.Replace(ident, "").Trim();

                    // Create the Dict, if it's not there
                    if (!settings.memos.ContainsKey(channelname))
                        settings.memos[channelname] = new Dictionary<string, string>();

                    // If we already know this
                    if (settings.memos[channelname].ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is already registered!");
                    }
                    else
                    {
                        settings.memos[channelname].Add(ident, "\x02" + message.User.Nick + "\x02 said on \x02[" + DateTime.UtcNow.ToString() + "]\x02 in \x02" + channelname + "\x02: " + text);
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I added this memo to my dictionary.");
                    }
                }
                else if (Is(args, "remove"))
                {
                    // remove it!
                    string ident = "[" + args.Replace("remove", "").Trim() + "]";

                    // Create the Dict, if it's not there
                    if (!settings.memos.ContainsKey(channelname))
                        settings.memos[channelname] = new Dictionary<string, string>();

                    // If we don't know this
                    if (!settings.memos[channelname].ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is not registered!");
                    }
                    else
                    {
                        settings.memos[channelname].Remove(ident);
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I removed this memo from my dictionary.");
                    }
                }
                else if (Is(args, "update"))
                {
                    // update it!
                    string memo = args.Replace("update", "").Trim();

                    // Get the identifier
                    string ident = Regex.Match(memo, "\\[.+\\]").Value;

                    // Get the text
                    string text = memo.Replace(ident, "").Trim();

                    // Create the Dict, if it's not there
                    if (!settings.memos.ContainsKey(channelname))
                        settings.memos[channelname] = new Dictionary<string, string>();

                    // If we don't know this
                    if (!settings.memos[channelname].ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is not registered!");
                    }
                    else
                    {
                        settings.memos[channelname][ident] = "\x02" + message.User.Nick + "\x02 said on \x02[" + DateTime.UtcNow.ToString() + "]\x02 in \x02" + channelname + "\x02: " + text;
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I updated this memo.");
                    }
                }
                else if (Is(args, "list"))
                {
                    // Announce it
                    // Create the Dict, if it's not there
                    if (!settings.memos.ContainsKey(channelname))
                        settings.memos[channelname] = new Dictionary<string, string>();

                    SendMessage(message.User.Nick + ": I will send you a list of my memos for this channel!");
                    SendMessage("Here is a list of all my memos for " + channelname + ": " + string.Join(", ", settings.memos[channelname].Keys), new[] { message.User.Nick });
                }
                else
                {
                    // Get the ident
                    string ident = "[" + args.Trim() + "]";

                    // Create the Dict, if it's not there
                    if (!settings.memos.ContainsKey(channelname))
                        settings.memos[channelname] = new Dictionary<string, string>();

                    // If we don't know this
                    if (!settings.memos[channelname].ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is not registered!");
                    }
                    else
                    {
                        SendMessage(settings.memos[channelname][ident]);
                    }
                }
            }
        }
    }
}