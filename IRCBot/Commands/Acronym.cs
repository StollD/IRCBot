using System.Text.RegularExpressions;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Acronym Expander
        [Command("acr")]
        public static void Acronym(string msg)
        {
            // Get the attributes
            string args = Utils.Remove(msg, "acr", true);

            // If we have no additional commands
            if (args.Length == 0)
            {
                // Response
                SendMessage(message.User.Nick + ": Currently, I have " + settings.acronyms.Count + " acronyms stored.");
            }
            else
            {
                // If we should add an acronym
                if (Is(args, "add"))
                {
                    // add it!
                    string acr = Utils.Remove(args, "add");

                    // Get the identifier
                    string ident = Regex.Match(acr, "\\[.+\\]").Value;

                    // Error out
                    if (string.IsNullOrEmpty(ident))
                    {
                        SendMessage(message.User.Nick + ": Invalid key!");
                        return;
                    }

                    // Get the text
                    string text = Utils.Remove(acr, ident);

                    // If we already know this
                    if (settings.acronyms.ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is already registered!");
                    }
                    else
                    {
                        settings.acronyms.Add(ident, text);
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I added this acronym to my dictionary.");
                    }
                }
                else if (Is(args, "remove"))
                {
                    // remove it!
                    string ident = "[" + Utils.Remove(args, "remove") + "]";

                    // If we don't know this
                    if (!settings.acronyms.ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is not registered!");
                    }
                    else
                    {
                        settings.acronyms.Remove(ident);
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I removed this acronym from my dictionary.");
                    }
                }
                else if (Is(args, "update"))
                {
                    // update it!
                    string acr = Utils.Remove(args, "update");

                    // Get the identifier
                    string ident = Regex.Match(acr, "\\[.+\\]").Value;

                    // Get the text
                    string text = Utils.Remove(acr, ident);

                    // If we don't know this
                    if (!settings.acronyms.ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is not registered!");
                    }
                    else
                    {
                        settings.acronyms[ident] = text;
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I updated this acronym.");
                    }
                }
                else if (Is(args, "list"))
                {
                    // Announce it
                    SendMessage(message.User.Nick + ": I will send you a list of my acronyms!");
                    SendMessage("Here is a list of all my acronyms: " + string.Join(", ", settings.acronyms.Keys), new[] { message.User.Nick });
                }
                else
                {
                    // Get the ident
                    string ident = "[" + args.Trim() + "]";

                    // If we don't know this
                    if (!settings.acronyms.ContainsKey(ident))
                    {
                        SendMessage(message.User.Nick + ": This key is not registered!");
                    }
                    else
                    {
                        SendMessage(message.User.Nick + ": " + ident + " => " + settings.acronyms[ident]);
                    }
                }
            }
        }
    }
}