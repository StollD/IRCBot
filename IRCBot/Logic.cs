using System.Linq;
using System.Reflection;

namespace IRCBot
{
    // Class to store logical functions
    public partial class Logic : BaseUtils
    {
        public static void Run()
        {
            foreach (MethodInfo info in typeof(Logic).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name != "Run"))
                info.Invoke(null, null);
        }
    }
}