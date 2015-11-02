using System;
using System.Diagnostics.Contracts;

namespace Cession.Commands
{
    public class Command<T>:Command
    {
        private T _arg;
        private Action<T> _executeAction;
        private Action<T> _undoAction;

        public Command (T arg,
                  Action<T> executeAction, 
                  Action<T> undoAction)
        {
//            Contract.Requires<ArgumentNullException> (executeAction != null);
//            Contract.Requires<ArgumentNullException> (undoAction != null);
            if (null == executeAction)
                throw new ArgumentNullException ();
            if (null == undoAction)
                throw new ArgumentNullException ();
            _arg = arg;
            _executeAction = executeAction;
            _undoAction = undoAction;
        }

        public override void Undo ()
        {
            _undoAction (_arg);
        }

        public override void Execute ()
        {
            _executeAction (_arg);
        }
    }
}

