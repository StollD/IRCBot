using System;

namespace IRCBot
{
    // Class to store Commands
    public partial class Commands : BaseUtils { }

    // Class to recognize Command-Functions
    public class Command : Attribute
    {
        public string command = "";
        public bool admin = false;

        public Command(string command)
        {
            this.command = command;
        }
    }

    // Class to manage different states
    public class MultipleCommand : Attribute
    {
        public string commandT, commandF;
        public bool admin = false;

        public MultipleCommand(string commandT, string commandF)
        {
            this.commandF = commandF; this.commandT = commandT;
        }
    }
}