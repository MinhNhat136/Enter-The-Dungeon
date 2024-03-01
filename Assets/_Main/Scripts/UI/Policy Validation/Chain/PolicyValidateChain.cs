using Atomic.Controllers;
using Atomic.Core.Interface;
using RMC.Core.Architectures.Mini.Context;

namespace Atomic.Chain
{
    public class PolicyValidateChain : BaseChain
    {
        public override IChain SetContext(IContext context)
        {
            _context = context;
            _context.CommandManager.AddCommandListener<AcceptedPolicyRulesCommand>(Command_OnAcceptedPolicyRule);
            _context.CommandManager.AddCommandListener<PolicyValidateCompletionCommand>(Command_OnPolicyValidateCompetion);
            return this;
        }

        private void Command_OnAcceptedPolicyRule(AcceptedPolicyRulesCommand command)
        {
            _nextChain?.Handle();
            OnDestroyChain();
        }

        private void Command_OnPolicyValidateCompetion(PolicyValidateCompletionCommand command)
        {
            if (command.IsAccepted)
            {
                _nextChain?.Handle();
                OnDestroyChain();
            }
        }

        public override void OnDestroyChain()
        {
            _context.CommandManager.RemoveCommandListener<AcceptedPolicyRulesCommand>(Command_OnAcceptedPolicyRule);
            _context.CommandManager.RemoveCommandListener<PolicyValidateCompletionCommand>(Command_OnPolicyValidateCompetion);
        }

        public override void Handle()
        {
            _context.CommandManager.InvokeCommand(new StartPolicyValidateProgressCommand());

        }
    }
}