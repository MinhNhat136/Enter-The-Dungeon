using Atomic.Core.Interface;

namespace Atomic.Character
{
    public interface ITargetingController : IInitializableWithBaseModel<BaseAgent>
    {
        public void FindTarget();
        public void UpdateTarget();
        public void EvaluateTargetScores();
        
        
    }
}
