using System.Reflection;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Action Command
        [Command("settings")]
        public static void Settings(string msg)
        {
            // Get the action
            string set = Utils.Remove(msg, "settings", true);

            // Vars
            string[] split; string fieldName, value; FieldInfo field;

            // If we set sth
            if (Is(set, "set") && admin)
            {
                // Get the field and the value
                split = Utils.Remove(set, "set").Split(' ');
                fieldName = split[0];
                value = split[1];

                // Find the Field
                field = typeof(Settings).GetField(fieldName);
                if (field == null)
                {
                    SendMessage(message.User.Nick + ": I can't find a field named \"" + fieldName + "\"!");
                }

                // Check if setabel
                if (field.FieldType != typeof(string) && field.FieldType.GetMethod("Parse", new[] { typeof(string) }) == null)
                {
                    SendMessage(message.User.Nick + ": Sorry, not settable!");
                    return;
                }

                // Set
                object val = value;
                MethodInfo m = field.FieldType.GetMethod("Parse", new[] { typeof(string) });
                if (m != null)
                {
                    val = m.Invoke(null, new[] { value });
                }
                field.SetValue(settings, val);
                SendMessage(message.User.Nick + ": I've set \"" + fieldName + "\" to \"" + value + "\"");
                return;
            }
            // Find the Field
            field = typeof(Settings).GetField(set);
            if (field == null)
            {
                SendMessage(message.User.Nick + ": I can't find a field named \"" + set + "\"!");
            }

            // Get the value and return it
            SendMessage(message.User.Nick + ": " + set + " = " + field.GetValue(settings));
        }
    }
}