/*using System;
using System.Collections.Generic;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Smile Command
        [Command("smiley")]
        public static void Smile(string msg)
        {
            // Dictionary with Smileys
            Dictionary<string, string> smileys = new Dictionary<string, string>()
            {
                { ":)", "smile" },
                { ":(", "sad" },
                { ":P", "tongue" },
                { ";)", "wink" },
                { "8)", "cool" },
                { ":D", "biggrin" },
                { ";(", "crying" },
                { "^^", "squint" },
                { ":rolleyes:", "rolleyes" },
                { ":huh:", "huh" },
                { ":S", "unsure" },
                { ":love:", "love" },
                { ":seriously:", "seriously" },
                { ":thumbdown:", "thumbdown" },
                { ":thumbup:", "thumbup" },
                { ":thumbsup:", "thumbsup" },
                { ":|", "mellow" },
                { "8o", "w00t" },
                { ":pinch:", "pinch" },
                { ":sleeping:", "sleeping" },
                { ":wacko:", "wacko" },
                { ";P", "winktongue" },
                { ":]", "smile" },
                { ":party:", "Party" },
                { ":whistling:", "whistling" },
                { ":crylaugh:", "crylaugh" },
                { ":facepalm:", "facepalm" },
                { ":cursing:", "cursing" },
                { ":really:", "really" },
                { ":badplan:", "BadPlan" },
                { ":wink:", "Wink" },
                { ":evil:", "Teufel" },
                { ":holy:", "Holy" },
                { ":/", "mellow" },
                { ":'D", "crylaugh" },
                { ":O", "w00t" },
                { "<3", "love" },
                { ">.<", "pinch" },
                { ">.>", "pinch" },
                { ">_<", "pinch" },
                { ">_>", "pinch" },
                { "o.O", "seriously" },
                { "O.o", "seriously" },
                { "o_O", "seriously" },
                { "O_o", "seriously" },
                { "o.0", "seriously" },
                { "0.o", "seriously" },
                { "o_0", "seriously" },
                { "0_o", "seriously" }
            };

            // Get the smiley
            string smiley = Utils.Remove(msg, "smiley", true);
            if (!smileys.ContainsKey(smiley))
                return;
            string url = "http://www.kerbalspaceprogram.de/wcf/images/smilies/" + smileys[smiley] + ".png";
            SendMessage(url);
        }
    }
}*/
