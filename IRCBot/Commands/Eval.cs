using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands
    {
        // Code example
        private const string classCode = "using System;\nusing System.IO;\nusing System.Linq;\nusing System.Collections;\nusing System.Collections.Generic;\npublic class IRCBotMath\n{{\n\tpublic static object Eval()\n\t{{\n\t\treturn {0};\n\t}}\n}}";

        // Math Command
        [MultipleCommand("eval", "e")]
        public static void Evaluator(string msg, bool state)
        {
            try
            {
                // Get the eval
                string eval = Utils.Remove(msg, state ? "eval" : "e", true);

                // Get a CSharp Code Compiler
                CSharpCodeProvider csharp = new CSharpCodeProvider();

                // Configure it
                CompilerParameters param = new CompilerParameters(new[] { "System.dll" });
                CompilerResults result = csharp.CompileAssemblyFromSource(param, new[] { string.Format(classCode, eval) });

                // Execute it
                MethodInfo calc = result.CompiledAssembly.GetType("IRCBotMath").GetMethod("Eval");
                SendMessage(message.User.Nick + ": " + calc.Invoke(null, null));
            }
            catch
            {

            }
        }
    }
}