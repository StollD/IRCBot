namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Nick
        [Command("nick", admin = true)]
        public static void Nick(string msg)
        {
            // Get the nick
            string nick = Utils.Remove(msg, "nick", true);

            // Warning
            if (client.Users.Contains(nick))
            {
                SendMessage(message.User.Nick + ": This nick is already taken!");
                return;
            }

            // Apply it
            client.Nick(nick);
            SendMessage(message.User.Nick + ": I've changed my nick to " + nick + ".");
            settings.name = nick;
            Utils.Save(settings);
        }
    }
}