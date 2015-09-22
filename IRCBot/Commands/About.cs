using System.Text.RegularExpressions;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        public static int[] version = new[] { 0, 0, 0, 2 }; 

        // Infos about the bot
        [Command("about")]
        public static void About(string msg)
        {
            SendMessage(message.User.Nick + ": Qboid IRC-Bot - A helpful Utility bot for IRC (" + string.Join(".", version) + "). Created by Thomas (Thomas P. | @ThomasKerman). Source: http://github.com/ThomasKerman/IRCBot/");
        }
    }
}