using Atomic.Core.Interface;

namespace Atomic.Character
{
    public interface ITargetingController : IInitializableWithBaseModel<BaseAgent>
    {
        public string TargetLayer { get; set; }
        public float DistanceWeight { get; set; }
        public float AngleWeight { get; set; }
        public float AgeWeight { get; set; }
        public int MaxNumberTarget { get; set; }
        public void FindTarget();
        public void UpdateTarget();
        public void EvaluateTargetScores();
        
        
    }
}
