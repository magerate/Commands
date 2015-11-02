using System;
using System.Diagnostics.Contracts;

namespace Cession.Commands
{
    public class Command<T1,T2,T3,T4>:Command
    {
        private T1 _arg1;
        private T2 _arg2;
        private T3 _arg3;
        private T4 _arg4;

        private Action<T1,T2,T3,T4> _executeAction;
        private Action<T1,T2,T3,T4> _undoAction;

        public Command (T1 arg1, 
                  T2 arg2, 
                  T3 arg3,
                  T4 arg4,
                  Action<T1,T2,T3,T4> executeAction, 
                  Action<T1,T2,T3,T4> undoAction)
        {
//            Contract.Requires<ArgumentNullException> (executeAction != null);
//            Contract.Requires<ArgumentNullException> (undoAction != null);
            if (null == executeAction)
                throw new ArgumentNullException ();
            if (null == undoAction)
                throw new ArgumentNullException ();
            
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;

            _executeAction = executeAction;
            _undoAction = undoAction;
        }

        public override void Execute ()
        {
            _executeAction (_arg1, _arg2, _arg3, _arg4);
        }

        public override void Undo ()
        {
            _undoAction (_arg1, _arg2, _arg3, _arg4);
        }
    }
}

