using System;
using System.Collections.Generic;

namespace Cession.Commands
{
    public class CommandManager
    {
        private Stack<Command> _undoStack;
        private Stack<Command> _redoStack;

        private Queue<Command> _commandQueue;

        public event EventHandler<CommandCommitedEventArgs> Committed;

        public event EventHandler<EventArgs> CanUndoChanged;
        public event EventHandler<EventArgs> CanRedoChanged;

        public CommandManager ()
        {
            _undoStack = new Stack<Command> ();
            _redoStack = new Stack<Command> ();

            _commandQueue = new Queue<Command> ();
        }

        public void Execute (Command command)
        {
            Push (command);
            command.Execute ();
            OnCommit (new CommandCommitedEventArgs(command,CommandCommitType.Execute));
        }

        private void OnCommit (CommandCommitedEventArgs e)
        {
            if (null != Committed)
                Committed (this, e);
        }

        private void OnUndoChange ()
        {
            if (null != CanUndoChanged)
                CanUndoChanged (this, EventArgs.Empty);
        }

        private void OnRedoChange ()
        {
            if (null != CanRedoChanged)
                CanRedoChanged (this, EventArgs.Empty);
        }


        public void Push ()
        {
            if (_commandQueue.Count == 0)
                return;

            Command command;
            if (_commandQueue.Count == 1)
            {
                command = _commandQueue.Dequeue ();
            }
            else
            {
                command = new MacroCommand (_commandQueue);
                _commandQueue.Clear ();
            }
            Push (command);
            OnCommit (new CommandCommitedEventArgs(command,CommandCommitType.Execute));
        }

        public void Push (Command command)
        {
            ClearRedoStack ();

            if (_commandQueue.Count > 0)
            {
                var mc = new MacroCommand (_commandQueue);
                mc.AddCommand (command);
                _commandQueue.Clear ();
                _undoStack.Push (mc);
            }
            else
                _undoStack.Push (command);

            CheckChanged (true, _undoStack, OnUndoChange);
        }

        private void ClearRedoStack ()
        {
            if (_redoStack.Count > 0)
            {
                _redoStack.Clear ();
                OnRedoChange ();
            }
        }

        private void ClearUndoStack()
        {
            if (_undoStack.Count > 0)
            {
                _undoStack.Clear ();
                OnUndoChange ();
            }
        }

        public void ExecuteQueue (Command command)
        {
            ClearRedoStack ();
            _commandQueue.Enqueue (command);
            command.Execute ();
        }

        public void Queue(Command command)
        {
            _commandQueue.Enqueue (command);
        }

        public void Undo ()
        {
            if (_commandQueue.Count > 0)
                throw new InvalidOperationException ("command queue should be empty when undo, you may misuse the ExecuteQueue method");

            if (_undoStack.Count <= 0)
                throw new InvalidOperationException ("undo stake is empty");

            var command = _undoStack.Pop ();
            CheckChanged (false, _undoStack, OnUndoChange);

            _redoStack.Push (command);
            CheckChanged (true, _redoStack, OnRedoChange);

            command.Undo ();
            OnCommit (new CommandCommitedEventArgs (command, CommandCommitType.Undo));
        }

        public void Redo ()
        {
            if (_redoStack.Count <= 0)
                throw new InvalidOperationException ("redo stake is empty");

            var command = _redoStack.Pop ();
            CheckChanged (false, _redoStack, OnRedoChange);

            _undoStack.Push (command);
            CheckChanged (true, _undoStack, OnUndoChange);

            command.Redo ();
            OnCommit (new CommandCommitedEventArgs (command, CommandCommitType.Redo));
        }

        private void CheckChanged (bool isPush, Stack<Command> stack, Action action)
        {
            if (isPush)
            {
                if (stack.Count == 1)
                    action ();
            }
            else
            {
                if (stack.Count == 0)
                    action ();
            }
        }

        public void Clear ()
        {
            ClearRedoStack ();
            ClearUndoStack ();
            _commandQueue.Clear ();
        }

        public bool CanUndo
        {
            get{ return _undoStack.Count > 0; }
        }

        public bool CanRedo
        {
            get{ return _redoStack.Count > 0; }
        }
    }
}

