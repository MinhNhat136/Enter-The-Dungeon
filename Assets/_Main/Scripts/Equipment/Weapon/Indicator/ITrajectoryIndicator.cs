using UnityEngine;
namespace Atomic.Equipment
{
    public interface ITrajectoryIndicator : IEnergyConsumer
    {
        public float DelayActivateTime { get; set; }
        
        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            return this; 
        }
        
        public ITrajectoryIndicator SetLaunchTransform(Transform launchTransform)
        {
            return this;
        }

        public ITrajectoryIndicator SetTarget(Vector3 targetPosition)
        {
            return this;
        }

        public ITrajectoryIndicator SetForwardDirection(Transform forwardDirection)
        {
            return this;
        }
        
        public virtual void Set()
        {
            
        }
        
        public void Indicate();
        public void Activate();
        public void DeActivate(); 
    }
}
