using RMC.Core.Architectures.Mini.Controller.Commands;

namespace Atomic.UI
{
    public class SignInCompletedCommand : ICommand
    {
        //  Properties ------------------------------------
        public UserData UserData { get { return _userData; } }
        public bool WasSuccess { get { return _wasSuccess; } }

        //  Fields ----------------------------------------
        private readonly UserData _userData;
        private readonly bool _wasSuccess;

        //  Initialization  -------------------------------
        public SignInCompletedCommand(UserData userData, bool wasSuccess)
        {
            _userData = userData;
            _wasSuccess = wasSuccess;
        }
    }
}

