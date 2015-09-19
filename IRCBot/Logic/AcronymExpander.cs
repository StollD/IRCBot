using System;
using System.Linq;

namespace IRCBot
{
    // Class to store logical functions
    public partial class Logic
    {
        // Save all the messages
        public static void AcronymExpander()
        {
            string msg = Utils.Remove(message.Message, new[] { settings.name + ":", settings.name });
            if (Is(msg, "What is") || Is(msg, "Explain") || msg.EndsWith("?"))
            {
                string word = Utils.Remove(msg, new[] { "What is", "?", "Explain" });
                if (settings.acronyms.ContainsKey("[" + word + "]"))
                        Commands.Acronym(settings.startChar + "acr " + word);
            }
        }
    }
}