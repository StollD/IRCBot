using System;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Roll command
        [Command("roll")]
        public static void Roll(string msg)
        {
            // Get the attributes
            string args = Utils.Remove(msg, "roll", true);

            // If we have no additional commands
            if (args.Length == 0)
                args = "1d6";

            // Get the numbers
            int x, y;
            string[] numbers = args.Split('d');

            // Parse them
            if (numbers.Length != 2) return;
            if (!int.TryParse(numbers[0], out x) || !int.TryParse(numbers[1], out y)) return;
            x = Math.Min(Math.Max(x, 0), settings.maxRoll); y = Math.Min(Math.Max(y, 0), settings.maxRoll);

            // Roll
            int[] rolls = new int[x];
            Random r = new Random();
            for (int i = 0; i < x; i++)
                rolls[i] = r.Next(1, y + 1);

            // And return everything
            SendMessage(message.User.Nick + ": " + string.Join(", ", rolls));
        }
    }
}