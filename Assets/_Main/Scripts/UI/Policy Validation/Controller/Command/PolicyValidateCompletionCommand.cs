using RMC.Core.Architectures.Mini.Controller.Commands;

public class PolicyValidateCompletionCommand : ICommand
{
    private readonly bool _isAccepted;
    
    public bool IsAccepted
    {
        get { return _isAccepted; }
    }

    public PolicyValidateCompletionCommand(bool isAccepted)
    {
        _isAccepted = isAccepted;
    }
}
