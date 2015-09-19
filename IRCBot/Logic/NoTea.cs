namespace IRCBot
{
    // Class to store logical functions
    public partial class Logic
    {
        // Handle teabots "No tea."
        public static void NoTea()
        {
            // Teabot's "No tea" has to be special
            if (Is(message.User.Nick, "teabot") && Is(message.Message, "No tea."))
                SendMessage(":(");
        }
    }
}