using Atomic.Models;
using RMC.Core.Architectures.Mini.Controller.Commands;

namespace Atomic.Command
{
    public class SignInValidateCompletionCommand : ICommand
    {
        //  Properties ------------------------------------
        public UserProfileData UserData { get { return _userData; } }
        public bool WasSuccess { get { return _wasSuccess; } }

        //  Fields ----------------------------------------
        private readonly UserProfileData _userData;
        private readonly bool _wasSuccess;

        //  Initialization  -------------------------------
        public SignInValidateCompletionCommand(UserProfileData userData, bool wasSuccess)
        {
            _userData = userData;
            _wasSuccess = wasSuccess;
        }
    }
}

