using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IRCBot
{
    public class Settings
    {
        public string host = "irc.esper.net";
        public string name = "Qboid";
        public string pw = "";
        public string startChar = "$";
        public int maxRoll = 100;
        public int chanProbability = 10;
        public List<string> channels = new List<string>();
        public List<string> admins = new List<string>();
        public List<string> tea = new List<string>();
        public List<string> egg = new List<string>();
        public List<string> muted = new List<string>();
        public Dictionary<string, string> acronyms = new Dictionary<string, string>();
        public Dictionary<string, Dictionary<string, string>> memos = new Dictionary<string, Dictionary<string, string>>();
        public List<string> checkpointLikes = new List<string>();
        public List<string> noSpam = new List<string>();
    }

    public class Words
    {
        public List<string> messages = new List<string>();
    }

    public class Seen_Tell
    {
        public Dictionary<string, List<object>> seen = new Dictionary<string, List<object>>();
        public Dictionary<string, List<List<object>>> tell = new Dictionary<string, List<List<object>>>();
    }

    public class Alias
    {
        public Dictionary<string, List<string>> alias = new Dictionary<string, List<string>>();
    }
}