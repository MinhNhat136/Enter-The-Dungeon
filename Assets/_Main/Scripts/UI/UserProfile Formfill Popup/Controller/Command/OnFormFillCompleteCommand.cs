using Atomic.Models;
using RMC.Core.Architectures.Mini.Controller.Commands;

namespace Atomic.Command
{
    public class OnFormFillCompleteCommand : ICommand
    {
        private readonly UserProfileData _userProfileData;

        public UserProfileData UserProfileData
        {
            get { return _userProfileData; }
        }

        public OnFormFillCompleteCommand(UserProfileData userProfileData)
        {
            _userProfileData = userProfileData;
        }
    }
}

