using System;

namespace Cession.Commands
{
    public  abstract partial class Command
    {
        protected Command ()
        {
        }

        public abstract void Execute ();
        public abstract void Undo ();
        public void Redo()
        {
            Execute ();
        }
    }
}

