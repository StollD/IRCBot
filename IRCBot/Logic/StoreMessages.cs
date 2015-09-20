using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IRCBot
{
    // Class to store logical functions
    public partial class Logic
    {
        // Strip Chars
        private static string[] stripChars = { " ", ":", ",", ".", "?", "!", "§", "$", "%", "&", "/", "(", ")", "=", "´", "`", "*", "+", "~", "#", "'", "-", "_", "<", ">", "|", "²", "³", "{", "[", "]", "}", @"\", "^", "°" };

        // Save all the messages
        public static void StoreMessages()
        {
            try
            {
                IRCBot.words.messages.AddRange(message.Message.Split(stripChars, StringSplitOptions.RemoveEmptyEntries).Where(s => !Is(s, "http") && !Is(s, "https")));
                Utils.Save(words);
            }
            catch { }
        }
    }
}