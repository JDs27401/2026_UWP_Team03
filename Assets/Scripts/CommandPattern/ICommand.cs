namespace CommandPattern
{
    public interface ICommand
    {
        bool Execute();
        void Undo();
    }    
}
