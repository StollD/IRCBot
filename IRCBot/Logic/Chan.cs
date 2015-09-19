namespace IRCBot
{
    // Class to store logical functions
    public partial class Logic
    {
        // Spam the channel
        public static void Chan()
        {
            if (random.Next(100) < settings.chanProbability)
            {
                Commands.Chan(settings.startChar + "$chan");
            }
        }
    }
}