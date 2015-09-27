using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Code example
        private static string classCode = "";

        // Math Command
        [MultipleCommand("eval", "e")]
        public static void Evaluator(string msg, bool state)
        {
            // Get the eval
            string eval = Utils.Remove(msg, state ? "eval" : "e", true);

            // Create the class string, if it's not there
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Functions/");
            if (!File.Exists(Directory.GetCurrentDirectory() + "/Functions/main"))
            {
                string c =
@"using System;
//using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class IRCBotEval
{
    public object Eval()
    {
        return <###0###>;
    }

<###1###>
}";
                File.WriteAllText(Directory.GetCurrentDirectory() + "/Functions/main", c);
            }
            BuildPastebins();

            // Functions
            if (Is(eval, "function"))
            {
                string act = Utils.Remove(eval, "function");
                
                // Add
                if (Is(act, "add"))
                {
                    // Admin
                    if (!admin)
                    {
                        SendMessage(message.User.Nick + ": Only admins can add functions! Please ping one of the admins ($admin) or send him a private message.");
                        return;
                    }

                    string paste = Utils.Remove(act, "add");

                    // Gist or Pastebin?
                    bool pastebin = !paste.Contains("@");

                    // If pastebin
                    string funct = "";
                    if (pastebin)
                    {
                        WebRequest request = WebRequest.Create("http://pastebin.com/raw.php?i=" + paste);
                        Stream data = request.GetResponse().GetResponseStream();
                        using (StreamReader sr = new StreamReader(data))
                            funct = sr.ReadToEnd();
                    }
                    else
                    {
                        string[] gist = paste.Split('@');
                        WebRequest request = WebRequest.Create("https://gist.githubusercontent.com/" + gist[0] + "/" + gist[1] + "/raw");
                        Stream data = request.GetResponse().GetResponseStream();
                        using (StreamReader sr = new StreamReader(data))
                            funct = sr.ReadToEnd();
                    }

                    // Write the file
                    File.WriteAllText(Directory.GetCurrentDirectory() + "/Functions/" + paste, funct);
                    BuildPastebins();
                    SendMessage(message.User.Nick + ": I added this function into my library.");
                }
                else if (Is(act, "remove")) // Remove
                {
                    // Admin
                    if (!admin)
                    {
                        SendMessage(message.User.Nick + ": Only admins can remove functions! Please ping one of the admins ($admin) or send him a private message.");
                        return;
                    }

                    string paste = Utils.Remove(act, "remove");

                    if (File.Exists(Directory.GetCurrentDirectory() + "/Functions/" + paste))
                    {
                        SendMessage(message.User.Nick + ": I removed this function from my library.");
                        File.Delete(Directory.GetCurrentDirectory() + "/Functions/" + paste);
                        BuildPastebins();
                    }
                }
            }
            else
            {
                // If we have no args
                try
                {
                    // Get a CSharp Code Compiler
                    CSharpCodeProvider csharp = new CSharpCodeProvider();

                    // Configure it
                    CompilerParameters param = new CompilerParameters(new[] { "System.dll" });
                    CompilerResults result = csharp.CompileAssemblyFromSource(param, classCode.Replace("<###0###>", eval).Replace("<###1###>", ""));

                    // Execute it
                    foreach (var s in result.Errors) Log(s);
                    MethodInfo calc = result.CompiledAssembly.GetType("IRCBotEval").GetMethod("Eval");
                    SendMessage(message.User.Nick + ": " + calc.Invoke(Activator.CreateInstance(calc.DeclaringType), null));
                }
                catch
                {

                }
            }
        }
        // Rebuild the class string from the pastebins
        private static void BuildPastebins()
        {
            classCode = File.ReadAllText(Directory.GetCurrentDirectory() + "/Functions/main");
            foreach (string pastebin in Directory.GetFiles(Directory.GetCurrentDirectory() + "/Functions/"))
            {
                // Dont load the main file
                if (Path.GetFileName(pastebin) == "main")
                    continue;

                // Load the files
                classCode = classCode.Replace("<###1###>", File.ReadAllText(pastebin) + "<###1###>");
            }
        }
    }
}