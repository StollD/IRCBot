using System;
using System.Linq;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Color translator
        [Command("color")]
        public static void Color(string msg)
        {
            try
            {
                // Get the color
                string colorString = Utils.Remove(msg, "color", true);

                // Get the type of the input
                string[] split = colorString.Split('[');
                Colors color = Colors.Error;
                color = Enum.TryParse(split.First(), true, out color) ? color : Colors.Error;
                colorString = split.Last();

                // Split it
                string[] col = colorString.Split(']');
                string[] colors = col[0].Split(',');
                colorString = col[1];

                // Error
                if (color == Colors.Error)
                {
                    SendMessage(message.User.Nick + ": This color is invalid!");
                    return;
                }

                /* Adjust the last string
                if (color == Colors.Hex) colors[0] = colors[0].Remove(7);
                if (color == Colors.Unity) colors[colors.Length - 1] = colors.Last().Trim().Remove(1);
                if (color == Colors.Unity32) colors[colors.Length - 1] = colors.Last().Trim().Remove(3);
                */

                // Get the Target
                while (!colorString.StartsWith("->") && !colorString.StartsWith("to"))
                {
                    colorString = colorString.Remove(0, 1);
                }

                // Get the target
                Colors target = Colors.Error;

                if (Is(colorString, "->") || Is(colorString, "to"))
                {
                    colorString = Utils.Remove(colorString, new string(colorString.Take(2).ToArray()));
                    target = (Colors)Enum.Parse(typeof(Colors), colorString, true);

                    // Transform everything
                    float[] c = new float[4];

                    if (color == Colors.Hex)
                    {
                        c = HexToUnity(colors[0]);
                    }
                    else if (color == Colors.Unity32)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Log(colors[i]);
                            c[i] = float.Parse(colors[i]) / 255f;
                        }
                    }
                    else if (color == Colors.Unity)
                    {
                        for (int i = 0; i < 4; i++)
                            c[i] = float.Parse(colors[i]);
                    }

                    // Target
                    string t = "";

                    if (target == Colors.Hex)
                    {
                        t = ToHex(c);
                    }
                    else if (target == Colors.Unity32)
                    {
                        float[] c2 = new float[4];
                        for (int i = 0; i < 4; i++)
                            c2[i] = c[i] * 255f;
                        t = string.Join(",", c2);
                    }
                    else if (target == Colors.Unity)
                    {
                        t = string.Join(",", c);
                    }

                    // Return it
                    SendMessage(message.User.Nick + ": " + t);
                }
                else
                {
                    SendMessage(message.User.Nick + ": I can't parse the target format!");
                }
            }
            catch
            {
                SendMessage(message.User.Nick + ": Something went wrong!");
            }
        }

        private static float[] HexToUnity(string hexString)
		{
			return new float[] { Convert.ToInt32(hexString.Substring(1, 2), 16) / 255f, Convert.ToInt32(hexString.Substring(3, 2), 16) / 255f, Convert.ToInt32(hexString.Substring(5, 2), 16) / 255f, 1f };
		}
			
        private static string ToHex(float[] c)
        {
            int num = (int)(c[0] * 255f);
            string str = num.ToString("x2");
            int num1 = (int)(c[1] * 255f);
            string str1 = num1.ToString("x2");
            int num2 = (int)(c[2] * 255f);
            return string.Concat("#", str, str1, num2.ToString("x2"));
        }
    }
}