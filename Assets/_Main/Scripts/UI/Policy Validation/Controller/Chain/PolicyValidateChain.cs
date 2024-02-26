using Atomic.Command;
using Atomic.Core.Interface;
using RMC.Core.Architectures.Mini.Context;

namespace Atomic.Chain
{
    public class PolicyValidateChain : BaseChain
    {
        public override IChain SetContext(IContext context)
        {
            _context = context;
            _context.CommandManager.AddCommandListener<AcceptedPolicyRulesCommand>( _ => 
            {
                _nextChain?.Handle();
            });
            _context.CommandManager.AddCommandListener<PolicyValidateCompletionCommand>((v) =>
            {
                if (v.IsAccepted)
                {
                    _nextChain?.Handle();
                }
            });
            return this;
        }

        public override void Handle()
        {
            _context.CommandManager.InvokeCommand(new StartPolicyValidateProgressCommand());

        }
    }
}