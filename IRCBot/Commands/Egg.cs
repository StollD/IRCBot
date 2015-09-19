using System.Linq;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // egg Command
        [Command("egg")]
        public static void Egg(string msg)
        {
            // Get the attributes
            string args = Utils.Remove(msg, "egg", true);

            // If we have no additional commands
            if (args.Length == 0)
            {
                // Check for eggbot
                if (channel.Users.Where(u => u.Nick == "eggbot").Count() != 0)
                {
                    if (settings.egg.Count == 0)
                    {
                        SendMessage(message.User.Nick + ": Sorry, no egg. :(");
                    }
                    else
                    {
                        // Pick a random egg
                        string egg = settings.egg[random.Next(0, settings.egg.Count)];
                        SendMessage("eggbot: " + egg);
                    }
                }
                else
                {
                    SendMessage(message.User.Nick + ": Sorry, no eggbot. :(");
                }
            }
            else
            {
                // If we should add a egg
                if (Is(args, "add"))
                {
                    // add it!
                    string egg = Utils.Remove(args, "add");

                    // If we already know this
                    if (settings.egg.Contains(egg))
                    {
                        SendMessage(message.User.Nick + ": I already know this sort of egg.");
                    }
                    else
                    {
                        settings.egg.Add(egg);
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I added this egg to my dictionary.");
                    }
                }
                else if (Is(args, "remove"))
                {
                    // remove it!
                    string egg = Utils.Remove(args, "remove");

                    // Do we know this egg?
                    if (settings.egg.Contains(egg))
                    {
                        settings.egg.Remove(egg);
                        SendMessage(message.User.Nick + ": I forgot everything about this egg.");
                        Utils.Save(settings);
                    }
                    else
                    {
                        SendMessage(message.User.Nick + ": Sorry, I don't know anything about this egg. :(");
                    }
                }
                else if (Is(args, "list"))
                {
                    // Announce it
                    SendMessage(message.User.Nick + ": I will send you a list of my eggs!");
                    SendMessage("Here is a list of all my eggs: " + string.Join(", ", settings.egg), new[] { message.User.Nick });
                }
            }
        }
    }
}