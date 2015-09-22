using ChatSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace IRCBot
{
    // Utility acessors for the bot
    public class BaseUtils
    {
        // Fetch the settings
        protected static Settings settings
        {
            get { return IRCBot.settings; }
            set { IRCBot.settings = value; }
        }

        // Fetch the words
        protected static Words words
        {
            get { return IRCBot.words; }
            set { IRCBot.words = value; }
        }

        // Fetch the words
        protected static Seen_Tell seenTell
        {
            get { return IRCBot.seenTell; }
            set { IRCBot.seenTell = value; }
        }

        // Fetch the alialses
        protected static Alias alias
        {
            get { return IRCBot.alias; }
            set { IRCBot.alias = value; }
        }

        // Fetch the channel
        protected static IrcChannel channel
        {
            get { return IRCBot.channel; }
            set { IRCBot.channel = value; }
        }

        // Fetch the client
        protected static IrcClient client
        {
            get { return IRCBot.client; }
            set { IRCBot.client = value; }
        }

        // Fetch the admin state
        protected static bool admin
        {
            get { return IRCBot.admin; }
            set { IRCBot.admin = value; }
        }

        // Fetch the message
        protected static PrivateMessage message
        {
            get { return IRCBot.message; }
            set { IRCBot.message = value; }
        }

        // Is-Comarison
        public static bool Is(string text, string cmd)
		{
			return text.StartsWith(cmd);
		}

        // Random
        public static Random random;

        // Logging //

        public static void Log(object o)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
            Writer().WriteLineF("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
        }

        public static void LogSpecial(object o)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
            Writer().WriteLineF("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void LogQboid(object o)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
            Writer().WriteLineF("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void LogAdmin(object o)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
            Writer().WriteLineF("[" + DateTime.UtcNow.ToLongTimeString() + "] " + o);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        // Channel Messages
        public static void SendMessage(string msg, [Optional] string[] targets)
        {
            if (targets == null)
            {
                foreach (string s in Split(msg, 460))
                {
                    channel.SendMessage(s);
                    LogQboid(s);
                }
            }
            else if (targets != null)
            {
                foreach (string s in Split(msg, 460))
                {
                    client.SendMessage(s, targets);
                    LogQboid(s);
                }
            }
        }

        private static string[] Split(string s, int length)
        {
            List<string> splits = new List<string>();
            while (s.Length > 0)
            {
                string split = new string(s.Take(Math.Min(length, s.Length)).ToArray());
                s = s.Remove(0, Math.Min(length, s.Length));
                splits.Add(split);
            }
            return splits.ToArray();
        }

        private static StreamWriter _writer;

        public static StreamWriter Writer()
        {
            if (_writer == null)
            {
                _writer = new StreamWriter(Directory.GetCurrentDirectory() + "/log.txt", false);
                _writer.WriteLineF("[Start] ============ " + DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToLongTimeString() + " ============");
            }
            return _writer;
        }
    }

    // Utility class
    public class Utils : BaseUtils
    {
        // Remove texts from a string
        public static string Remove(string input, string del, bool stripCommandChar = false)
        {
            if (stripCommandChar)
                return input.Replace(settings.startChar + del, "").Trim();
            else
                return input.Replace(del, "").Trim();
        }

        // Remove texts from a string
        public static string Remove(string input, string[] del, bool stripCommandChar = false)
        {
            foreach (string d in del)
            {
                if (stripCommandChar)
                    input = input.Replace(settings.startChar + d, "").Trim();
                else
                    input = input.Replace(d, "").Trim();
            }
            return input;
        }

        // Load the settings
        public static T Load<T>() where T : new()
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(Directory.GetCurrentDirectory() + "/Settings/" + typeof(T).Name.ToLower() + ".json"),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    FloatParseHandling = FloatParseHandling.Double,
                    NullValueHandling = NullValueHandling.Include,
                    ObjectCreationHandling = ObjectCreationHandling.Auto
                });
        }

        // Save the settings
        public static void Save<T>(T data) where T : new()
        {
            File.WriteAllText(
                Directory.GetCurrentDirectory() + "/Settings/" + typeof(T).Name.ToLower() + ".json",
                JsonConvert.SerializeObject(data, typeof(T), new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    FloatParseHandling = FloatParseHandling.Double,
                    NullValueHandling = NullValueHandling.Include,
                    ObjectCreationHandling = ObjectCreationHandling.Auto
                }));
        }

        public static void Save<T>(ref T data) where T : new()
        {
            if (data == null)
                data = new T();
            Save(data);
        }

        // Parse Datetime
        public static DateTime ParseTime(string time)
        {
            int[] date = new[] { 1, 1, 1, 0, 0, 0 };
            string[] split = time.Split('-');
            for (int i = 0; i < split.Length; i++)
                date[i] = Int32.Parse(split[i]);
            return new DateTime(date[0], date[1], date[2], date[3], date[4], date[5]);
        }
    }
}