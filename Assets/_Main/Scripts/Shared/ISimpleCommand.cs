
public interface ISimpleCommand
{
    void Execute(object parameter);
    void UnExecute(object parameter);
}
