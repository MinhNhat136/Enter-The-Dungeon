

using Atomic.Controllers;

namespace Atomic.Chain
{
    public class NetworkValidateChain : BaseChain
    {
        public override void Handle()
        {
            _context.CommandManager.InvokeCommand(new StartValidateNetworkConnectionCommand());
            _nextChain?.Handle();
        }
    }
}

