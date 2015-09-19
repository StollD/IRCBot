namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // We all like checkpoint, so we have to show that!
        [Command("like")]
        public static void Likecheckpoint(string msg)
        {
            // Get the attributes
            string args = Utils.Remove(msg, "like", true);

            if (args.Length == 0)
            {
                if (!channel.Users.Contains("checkpoint"))
                {
                    SendMessage(message.User.Nick + ": Sorry, no checkpoint.");
                    return;
                }
                SendMessage("checkpoint: " + settings.checkpointLikes[random.Next(0, settings.checkpointLikes.Count)]);
            }
            else if (Is(args, "add"))
            {
                string add = Utils.Remove(args, "add");
                settings.checkpointLikes.Add(add);
                Utils.Save(settings);
                SendMessage(message.User.Nick + ": I added this message to my list.");
            }
            else if (Is(args, "remove"))
            {
                string remove = Utils.Remove(args, "remove");
                settings.checkpointLikes.Remove(remove);
                Utils.Save(settings);
                SendMessage(message.User.Nick + ": I removed this message to my list.");
            }
            else if (Is(args, "list"))
            {
                // Announce it
                SendMessage(message.User.Nick + ": I will send you a list of messages for checkpoint!");
                SendMessage("Here is a list of all my messages for checkpoint: " + string.Join(", ", settings.checkpointLikes), new[] { message.User.Nick });
            }
        }
    }
}