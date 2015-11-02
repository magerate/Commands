using System;
using System.Collections.Generic;

namespace Cession.Commands
{
    public class MacroCommand:Command
    {
        private List<Command> _commands = new List<Command> ();

        public MacroCommand ()
        {
        }

        public MacroCommand (IEnumerable<Command> commands)
        {
            _commands.AddRange (commands);
        }

        public MacroCommand (Command command)
        {
            _commands.Add (command);
        }

        public void AddCommand (Command command)
        {
            _commands.Add (command);
        }

        public void AddRange (IEnumerable<Command> commands)
        {
            _commands.AddRange (commands);
        }

        public override void Execute ()
        {
            foreach (var command in _commands)
            {
                command.Execute ();
            }
        }

        public override void Undo ()
        {
            for (int i = _commands.Count - 1; i >= 0; i--)
            {
                _commands [i].Undo ();
            }
        }
    }
}

