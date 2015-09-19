using System.IO;

namespace IRCBot
{
    public static class StreamWriterExtensions
    {
        public static void WriteLineF(this StreamWriter stream, string s)
        {
            stream.WriteLine(s);
            stream.Flush();
        }
    }
}