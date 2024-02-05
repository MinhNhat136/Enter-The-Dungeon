using System.ComponentModel;

public interface ISignInController
{
    void Initialize(ICommand command, ISignInService service);
    void OnSignIn();
    void SignedIn();
}
