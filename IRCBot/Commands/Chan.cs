namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Roll command
        [Command("chan")]
        public static void Chan(string msg)
        {
            // Select random words from the list
            int count = random.Next(1, 6);
            string chan = "#";
            for (int i = 0; i < count; i++)
                chan += IRCBot.words.messages[random.Next(0, IRCBot.words.messages.Count)];

            // And return everything
            if (!settings.noSpam.Contains(channel.Name))
                SendMessage(chan);
        }
    }
}