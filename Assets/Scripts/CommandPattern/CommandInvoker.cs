using System.Collections.Generic;

namespace CommandPattern
{
    public static class CommandInvoker
    {
        private static readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private static readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
        
        public static void ExecuteCommand(ICommand command) 
        {
            bool success = command.Execute();
            if (success) 
            {
                _undoStack.Push(command);
                _redoStack.Clear(); 
            }
        }
        
        public static void UndoCommand() 
        {
            if (_undoStack.Count == 0) return;
            ICommand command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }
        
        public static void RedoCommand() 
        {
            if (_redoStack.Count == 0) return;
            ICommand command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }
}