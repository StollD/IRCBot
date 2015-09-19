using System.Linq;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Tea Command
        [Command("tea")]
        public static void Tea(string msg)
        {
            // Get the attributes
            string args = Utils.Remove(msg, "tea", true);

            // If we have no additional commands
            if (args.Length == 0)
            {
                // Check for Teabot
                if (channel.Users.Where(u => u.Nick == "teabot").Count() != 0)
                {
                    if (settings.tea.Count == 0)
                    {
                        SendMessage(message.User.Nick + ": Sorry, no tea. :(");
                    }
                    else
                    {
                        // Pick a random tea
                        string tea = settings.tea[random.Next(0, settings.tea.Count)];
                        SendMessage("teabot: " + tea);
                    }
                }
                else
                {
                    SendMessage(message.User.Nick + ": Sorry, no teabot. :(");
                }
            }
            else
            {
                // If we should add a tea
                if (Is(args, "add"))
                {
                    // add it!
                    string tea = Utils.Remove(args, "add");

                    // If we already know this
                    if (settings.tea.Contains(tea))
                    {
                        SendMessage(message.User.Nick + ": I already know this sort of tea.");
                    }
                    else
                    {
                        settings.tea.Add(tea);
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I added this tea to my dictionary.");
                    }
                }
                else if (Is(args, "remove"))
                {
                    // remove it!
                    string tea = Utils.Remove(args, "remove");

                    // Do we know this tea?
                    if (settings.tea.Contains(tea))
                    {
                        settings.tea.Remove(tea);
                        SendMessage(message.User.Nick + ": I forgot everything about this tea.");
                        Utils.Save(settings);
                    }
                    else
                    {
                        SendMessage(message.User.Nick + ": Sorry, I don't know anything about this tea. :(");
                    }
                }
                else if (Is(args, "list"))
                {
                    // Announce it
                    SendMessage(message.User.Nick + ": I will send you a list of my teas!");
                    SendMessage("Here is a list of all my teas: " + string.Join(", ", settings.tea), new[] { message.User.Nick });
                }
            }
        }
    }
}