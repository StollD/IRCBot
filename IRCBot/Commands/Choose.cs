using System.Linq;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Choose command
        [Command("choose")]
        public static void Choose(string msg)
        {
            string choose = Utils.Remove(msg, "choose", true);
            string[] options = choose.Split('|');
            string result = options[random.Next(0, options.Length)];
            if (options.Contains("KSP")) result = "KSP";
            if (options.Contains("Kerbal")) result = "Kerbal";
            if (options.Contains("Kerbal Space Program")) result = "Kerbal Space Program";
            if (options.Contains("KerbalSpaceProgram")) result = "KerbalSpaceProgram";
            SendMessage(message.User.Nick + ": Your options are: " + string.Join(", ", options) + ". My choice: " + result + ".");
        }
    }
}