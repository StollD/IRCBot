﻿using ChatSharp;
using ChatSharp.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace IRCBot
{
    public class IRCBot
    {
        // Instance
        public static IRCBot Instance { get; protected set; }

        // The connection to the IRC Server
        public static IrcClient client;

        // Is the bot active?
        public static bool isActive = false;

        // The Channel that recieved the last message
        public static IrcChannel channel;

        // Is the latest sender an admin?
        public static bool admin;

        // The latest message
        public static PrivateMessage message;
        public static string whoIs;

        // The persistent data
        public static Settings settings;
        public static Words words;
        public static Seen_Tell seenTell;
        public static Alias alias;

        // Create the Bot
        public static void Main(string[] args)
        {
            Instance = new IRCBot();
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "stop")
                    Environment.Exit(0);
                else
                {
                    string[] arr = input.Remove(0, 1).Split(':');
                    BaseUtils.SendMessage(arr[1], new[] { arr[0] });
                }
            }
        }

        // Start the bot
        public IRCBot()
        {
            // Load the settings
            if (File.Exists(Directory.GetCurrentDirectory() + "/Settings/settings.json"))
                settings = Utils.Load<Settings>();
            else
                Utils.Save(ref settings);

            // Load the words
            if (File.Exists(Directory.GetCurrentDirectory() + "/Settings/words.json"))
                words = Utils.Load<Words>();
            else
                Utils.Save(ref words);

            // Load the seen_tells'
            if (File.Exists(Directory.GetCurrentDirectory() + "/Settings/seen_tell.json"))
                seenTell = Utils.Load<Seen_Tell>();
            else
                Utils.Save(ref seenTell);

            // Load the aliase
            if (File.Exists(Directory.GetCurrentDirectory() + "/Settings/alias.json"))
                alias = Utils.Load<Alias>();
            else
                Utils.Save(ref alias);

            // Create the randomizer
            BaseUtils.random = new Random();

            // Startup
            BaseUtils.LogSpecial(settings.name + " - a friendly IRC bot!");

            // Connection
            client = new IrcClient(settings.host, new IrcUser(settings.name, settings.name));
            client.ConnectionComplete += (s, e) =>
            {
                client.SendMessage("identify " + settings.pw, new[] { "NickServ" });
                settings.channels.ForEach(c => client.JoinChannel(c));
            };
            client.ChannelMessageRecieved += Client_ChannelMessageRecieved;
            client.UserKicked += Client_UserKicked;
            client.NickChanged += Client_NickChanged;
            client.ConnectAsync();
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                client.Quit();
                BaseUtils.Writer().WriteLine("[Start] ============ " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ============");
                BaseUtils.Writer().Close();
            };

            // Stop GC
            GC.KeepAlive(this);
        }

        private void Client_NickChanged(object sender, NickChangedEventArgs e)
        {
            BaseUtils.Log(e.OldNick + " - " + e.NewNick);
            if (alias.alias.ContainsKey(e.OldNick))
            {
                if (!alias.alias[e.OldNick].Contains(e.NewNick))
                    alias.alias[e.OldNick].Add(e.NewNick);
            }
            else
            {
                foreach (KeyValuePair<string, List<string>> kvP in alias.alias)
                {
                    if (kvP.Value.Contains(e.OldNick))
                    {
                        if (kvP.Key == e.NewNick)
                            continue;
                        else if (kvP.Value.Contains(e.NewNick))
                            continue;
                        else
                            alias.alias[kvP.Key].Add(e.NewNick);
                        break;
                    }
                    else
                    {
                        alias.alias.Add(e.OldNick, new List<string>() { e.NewNick });
                        break;
                    }
                }
                if (alias.alias.Count == 0)
                    alias.alias.Add(e.OldNick, new List<string>() { e.NewNick });

            }
            Utils.Save(alias);
        }

        private void Client_UserKicked(object sender, KickEventArgs e)
        {
            if (e.Kicked.Nick != settings.name)
                return;

            BaseUtils.LogSpecial(e.Kicker.Nick + " kicked me from " + e.Channel.Name + "! Reason: " + e.Reason);
            client.PartChannel(e.Channel.Name);
            settings.channels.Remove(e.Channel.Name);
            Utils.Save(settings);
        }

        private static void Client_ChannelMessageRecieved(object sender, PrivateMessageEventArgs e)
        {
            try
            {
                // Is the sender an admin?
                client.WhoIs(e.PrivateMessage.User.Nick, who =>
                {
                    // Get the response
                    admin = settings.admins.Contains(who.LoggedInAs);
                    whoIs = who.LoggedInAs;

                    // Set the private message
                    message = e.PrivateMessage;

                    // Find the current channel
                    channel = client.Channels[e.PrivateMessage.Source];

                    // Log-Spam
                    if (!admin)
                        BaseUtils.Log(e.PrivateMessage.User.Nick + " in " + e.PrivateMessage.Source + ": " + e.PrivateMessage.Message);
                    else
                        BaseUtils.LogAdmin(e.PrivateMessage.User.Nick + " in " + e.PrivateMessage.Source + ": " + e.PrivateMessage.Message);

                    // Get the message
                    string msg = e.PrivateMessage.Message.Trim();

                    // If it should be a command
                    if (e.PrivateMessage.IsChannelMessage && msg.StartsWith(settings.startChar))
                    {
                        // Mute
                        if (settings.muted.Contains(channel.Name) && !BaseUtils.Is(msg, settings.startChar + "unmute"))
                            return;

                        // Get all the methods
                        MethodInfo[] methods = Assembly.GetAssembly(typeof(IRCBot)).GetTypes().SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static)).ToArray() as MethodInfo[];

                        string start = settings.startChar;

                        // Loop through them
                        foreach (MethodInfo method in methods)
                        {
                            // Check the Commands
                            foreach (Command c in method.GetCustomAttributes(typeof(Command), false))
                            {
                                // Check if the command is correct
                                if (BaseUtils.Is(msg, start + c.command))
                                {
                                    if (c.admin && !admin)
                                        break;
                                    method.Invoke(null, new[] { msg });
                                }
                            }

                            // Check the multipleCommands
                            foreach (MultipleCommand mc in method.GetCustomAttributes(typeof(MultipleCommand), false))
                            {
                                // Check if the command is correct
                                if (BaseUtils.Is(msg, start + mc.commandT) || BaseUtils.Is(msg, start + mc.commandF))
                                {
                                    if (mc.admin && !admin)
                                        break;
                                    method.Invoke(null, new object[] { msg, BaseUtils.Is(msg, start + mc.commandT) });
                                }
                            }
                        }
                    }
                    else
                    {
                        Logic.Run();
                    }
                });
            }
            catch { }
            whoIs = "";
        }
    }
}