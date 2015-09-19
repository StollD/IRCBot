namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Admin command
        [Command("admin", admin = true)]
        public static void Admin(string msg)
        {
            // Get the attributes
            string args = Utils.Remove(msg, "admin", true);

            // If we have no additional commands
            if (args.Length == 0)
            {
                SendMessage(message.User.Nick + ": Currently I know the following admins: " + string.Join(", ", settings.admins));
            }
            else
            {
                // If we should add an admin
                if (Is(args, "add"))
                {
                    // add it!
                    string adminUser = Utils.Remove(args, "add");

                    // If the user exists
                    if (!client.Users.Contains(adminUser))
                    {
                        SendMessage(message.User.Nick + ": Sorry, I can't find this username. :(");
                        return;
                    }

                    // Callback
                    client.WhoIs(adminUser, who =>
                    {
                        // Get the user-ident
                        string ident = who.LoggedInAs;

                        // If the ident is invalid
                        if (string.IsNullOrEmpty(ident))
                        {
                            SendMessage(message.User.Nick + ": Sorry, " + adminUser + "'s identifier seems to be invalid!");
                            return;
                        }

                        // If we already know this
                        if (settings.admins.Contains(ident))
                        {
                            SendMessage(message.User.Nick + ": This user has already got admin-access!");
                        }
                        else
                        {
                            settings.admins.Add(ident);
                            Utils.Save(settings);
                            SendMessage(message.User.Nick + ": I gave " + adminUser + " admin-access!");
                        }
                    });
                }
                else if (Is(args, "remove"))
                {
                    // remove it!
                    string adminUser = Utils.Remove(args, "remove");

                    // If the user exists
                    if (!client.Users.Contains(adminUser))
                    {
                        SendMessage(message.User.Nick + ": Sorry, I can't find this username. :(");
                        return;
                    }
                    // Callback
                    client.WhoIs(adminUser, who =>
                    {
                        // Get the user-ident
                        string ident = who.LoggedInAs;

                        // Do we know this user
                        if (settings.admins.Contains(ident))
                        {
                            settings.admins.Remove(ident);
                            Utils.Save(settings);
                            SendMessage(message.User.Nick + ": I removed " + adminUser + "'s admin-access!");
                        }
                        else
                        {
                            SendMessage(message.User.Nick + ": Currently, this user hasn't got admin-access!");
                        }
                    });
                }
            }
        }
    }
}