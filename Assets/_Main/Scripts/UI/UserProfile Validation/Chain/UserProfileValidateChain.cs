using Atomic.Controllers;
using Atomic.Core.Interface;
using RMC.Core.Architectures.Mini.Context;

namespace Atomic.Chain
{
    public class UserProfileValidateChain : BaseChain
    {
        public override void Handle()
        {
            _context.CommandManager.InvokeCommand(new UserProfileValidateCommand());
        }

        public override IChain SetContext(IContext context)
        {
            _context = context;
            _context.CommandManager.AddCommandListener<UserProfileValidateCompletionCommand>(Command_OnUserProfileValidateCompletion);
            return this;
        }

        private void Command_OnUserProfileValidateCompletion(UserProfileValidateCompletionCommand command)
        {
            _nextChain?.Handle();
            OnDestroyChain();
        }

        public override void OnDestroyChain()
        {
            _context.CommandManager.RemoveCommandListener<UserProfileValidateCompletionCommand>(Command_OnUserProfileValidateCompletion);
        }
    }
}