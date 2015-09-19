namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Action Command
        [Command("action")]
        public static void Action(string msg)
        {
            // Get the action
            string action = Utils.Remove(msg, "action", true);
            client.SendAction(action, new[] { channel.Name });
        }
    }
}