namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        [Command("say")]
        public static void Say(string msg)
        {
            // Get the action
            string text = Utils.Remove(msg, "say", true);
            SendMessage(text);
        }
    }
}