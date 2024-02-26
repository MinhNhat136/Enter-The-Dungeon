using Atomic.Command;
using Atomic.Core.Interface;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

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
            _context.CommandManager.AddCommandListener<UserProfileValidateCompletionCommand>((p) =>
            {
                _nextChain?.Handle();
            });
            return this;
        }
    }
}