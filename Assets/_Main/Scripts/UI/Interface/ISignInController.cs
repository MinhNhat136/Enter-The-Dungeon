using System.ComponentModel;

public interface ISignInController
{
    void StartSignInProcess();
    void OnSignInSuccess();
    void OnSignInFailed();
}
