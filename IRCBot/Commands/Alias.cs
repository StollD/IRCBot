/*using System.Collections.Generic;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Alias Command
        [Command("alias")]
        public static void Alias(string msg)
        {
            // Get the action
            string uname = Utils.Remove(msg, "alias", true);

            // Get all the aliases
            List<string> aliases = new List<string>();
            if (alias.alias.ContainsKey(uname))
            {
                aliases = alias.alias[uname];
            }
            else
            {
                foreach (KeyValuePair<string, List<string>> kvP in alias.alias)
                {
                    if (kvP.Value.Contains(uname))
                    {
                        aliases = kvP.Value;
                        aliases.Remove(uname);
                        aliases.Insert(0, kvP.Key);
                        break;
                    }
                }
            }

            // Output
            if (aliases.Count == 0)
                SendMessage(message.User.Nick + ": I have no aliases stored for \x02" + uname + "\x02!");
            else
                SendMessage(message.User.Nick + ": I have the following aliases stored for \x02" + uname + "\x02: " + string.Join(", ", aliases));
        }
    }
}*/