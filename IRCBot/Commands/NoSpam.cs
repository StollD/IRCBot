namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Action Command
        [Command("nospam")]
        public static void NoSpam(string msg)
        {
            // Get the action
            string action = Utils.Remove(msg, "nospam", true);

            // Add a channel
            if (Is(action, "add"))
            {
                string channel = Utils.Remove(action, "add");
                if (settings.noSpam.Contains(channel))
                {
                    SendMessage(message.User.Nick + ": This channel is already added!");
                }
                else
                {
                    SendMessage(message.User.Nick + ": I won't spam in this channel anymore.");
                    settings.noSpam.Add(channel);
                    Utils.Save(settings);
                }
                return;
            }

            // Remove a channel
            if (Is(action, "remove"))
            {
                string channel = Utils.Remove(action, "remove");
                if (settings.noSpam.Contains(channel))
                {
                    SendMessage(message.User.Nick + ": I removed this channel from my blacklist.");
                    settings.noSpam.Remove(channel);
                    Utils.Save(settings);
                }
                else
                {
                    SendMessage(message.User.Nick + ": My blacklist doesn't contain this channel!");
                }
                return;
            }

            // List the channels
            SendMessage(message.User.Nick + ": " + string.Join(", ", settings.noSpam));
        }
    }
}