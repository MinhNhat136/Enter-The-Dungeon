using Atomic.Character.Model;
using Atomic.Core.Interface;

namespace Atomic.Character.Module
{
    public interface ITargetingController : IInitializableWithBaseModel<BaseAgent>, ITickable
    {
        public void UpdateTarget();
        public void EvaluateTargetScores();
        public bool ForgetTargetConditions(AiMemoryObject memory);
    }
}
