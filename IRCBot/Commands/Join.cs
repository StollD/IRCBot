namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Join / Leave command
        [MultipleCommand("join", "leave", admin = true)]
        public static void Join(string msg, bool state)
        {
            // Get the new channel
            string channelName = Utils.Remove(msg, (state ? "join" : "leave"), true);

            // Join
            if (state)
            {
                // If we are already joined
                if (client.Channels.Contains(channelName))
                {
                    SendMessage(message.User.Nick + ": I'm aready active in this channel.");
                }
                else
                {
                    LogSpecial("Joining " + channelName);
                    client.JoinChannel(channelName);
                    settings.channels.Add(channelName);
                    Utils.Save(settings);
                    SendMessage(message.User.Nick + ": I've joined " + channelName + ".");
                }
            }
            else
            {
                // Leave
                // If a channel is specified
                if (channelName.Length > 0)
                {
                    // If we are already joined
                    if (client.Channels.Contains(channelName))
                    {
                        LogSpecial("Leaving " + channelName);
                        client.PartChannel(channelName);
                        settings.channels.Remove(channelName);
                        Utils.Save(settings);
                        SendMessage(message.User.Nick + ": I left " + channelName + ".");
                    }
                    else
                    {
                        SendMessage(message.User.Nick + ": I'm not active in this channel!");
                    }
                }
                else
                {
                    SendMessage(message.User.Nick + ": I will leave " + channel.Name + ".");
                    LogSpecial("Leaving " + channel.Name);
                    settings.channels.Remove(channel.Name);
                    Utils.Save(settings);
                    client.PartChannel(channel.Name);
                }
            }
        }
    }
}