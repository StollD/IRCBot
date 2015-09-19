namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Action Command
        [MultipleCommand("save", "load")]
        public static void Save(string msg, bool state)
        {
            // Get the action
            if (state)
                Utils.Save(settings);
            else
                Utils.Load<Settings>();
        }
    }
}