using Atomic.Command;
using Atomic.Models;

public interface ISignInController
{
    void StartSignInProcess(OnGuessSignInCommand command);
    void OnSignedInComplete(UserProfileData userData, bool wasSuccessful);
}
