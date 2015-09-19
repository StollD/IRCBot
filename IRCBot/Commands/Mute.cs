using ChatSharp;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Mute command
        [MultipleCommand("mute", "unmute", admin = true)]
        public static void Mute(string msg, bool state)
        {
            // Get the args
            string args = Utils.Remove(msg, (state ? "mute" : "unmute"), true);

            // If there are no args
            if (args.Length == 0)
            {
                // Mute / Unmute the bot on the current channel
                if (state && !settings.muted.Contains(channel.Name))
                    settings.muted.Add(channel.Name);
                else if (!state && settings.muted.Contains(channel.Name))
                    settings.muted.Remove(channel.Name);
            }
            else if (Is(args, "global"))
            {
                // Totally mute the bot
                foreach (IrcChannel c in client.Channels)
                {
                    // Mute / Unmute the bot on all channels
                    if (state && !settings.muted.Contains(c.Name))
                        settings.muted.Add(c.Name);
                    else if (!state && settings.muted.Contains(c.Name))
                        settings.muted.Remove(c.Name);
                }
            }
            else
            {
                // Get the channels
                string[] channels = args.Split(' ');

                // Mute the bot on the channels
                foreach (string name in channels)
                {
                    if (state && !settings.muted.Contains(name))
                        settings.muted.Add(name);
                    else if (!state && settings.muted.Contains(name))
                        settings.muted.Remove(name);
                }
            }
            Utils.Save(settings);
            SendMessage(message.User.Nick + ": " + (state ? "I'll stop talking now." : "I'll talk again now."));
        }
    }
}