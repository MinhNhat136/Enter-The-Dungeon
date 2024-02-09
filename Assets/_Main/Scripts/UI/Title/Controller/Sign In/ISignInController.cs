using Atomic.UI;
using System.ComponentModel;

public interface ISignInController
{
    void StartSignInProcess();
    void OnSignedInComplete(UserData userData, bool wasSuccessful);
}
