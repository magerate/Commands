using System;

namespace Cession.Commands
{
    public enum CommandCommitType
    {
        Execute,
        Undo,
        Redo,
    }

    public class CommandCommitedEventArgs:EventArgs
    {
        public Command Command{ get; set;}
        public CommandCommitType Type{ get; set;}

        public CommandCommitedEventArgs (Command command,CommandCommitType type)
        {
            Command = command;
            Type = type;
        }
    }
}

