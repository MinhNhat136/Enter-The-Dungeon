using Atomic.Models;

public interface ISignInController
{
    void StartSignInProcess();
    void OnSignedInComplete(UserProfileData userData, bool wasSuccessful);
}
