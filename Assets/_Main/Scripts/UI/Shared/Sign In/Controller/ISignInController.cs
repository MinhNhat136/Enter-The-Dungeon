using Atomic.Command;
using Atomic.Models;

public interface ISignInController
{
    void StartSignInProcess(OnGuestSignInCommand command);
    void OnSignedInComplete(UserProfileData userData, bool wasSuccessful);
}
