using System;
using System.Diagnostics.Contracts;

namespace Cession.Commands
{
    public class Command<T1,T2>:Command
    {
        private T1 _arg1;
        private T2 _arg2;

        private Action<T1,T2> _executeAction;
        private Action<T1,T2> _undoAction;

        public Command (T1 arg1, 
                  T2 arg2, 
                  Action<T1,T2> executeAction, 
                  Action<T1,T2> undoAction)
        {
//            Contract.Requires<ArgumentNullException> (executeAction != null);
//            Contract.Requires<ArgumentNullException> (undoAction != null);
            if (null == executeAction)
                throw new ArgumentNullException ();
            if (null == undoAction)
                throw new ArgumentNullException ();
            
            _arg1 = arg1;
            _arg2 = arg2;

            _executeAction = executeAction;
            _undoAction = undoAction;
        }

        public override void Execute ()
        {
            _executeAction (_arg1, _arg2);
        }

        public override void Undo ()
        {
            _undoAction (_arg1, _arg2);
        }
    }
}

