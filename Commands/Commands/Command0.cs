using System;
using System.Diagnostics.Contracts;

namespace Cession.Commands
{
    internal class Command0:Command
    {
        private Action _executeAction;
        private Action _undoAction;

        public Command0 (Action executeAction, Action undoAction)
        {
//            Contract.Requires<ArgumentNullException> (executeAction != null);
//            Contract.Requires<ArgumentNullException> (undoAction != null);
            if (null == executeAction)
                throw new ArgumentNullException ();
            if (null == undoAction)
                throw new ArgumentNullException ();
            
            _executeAction = executeAction;
            _undoAction = undoAction;
        }

        public override void Execute ()
        {
            _executeAction ();
        }

        public override void Undo ()
        {
            _undoAction ();
        }
    }
}

