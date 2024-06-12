using UnityEngine;
namespace Atomic.Equipment
{
    public interface ITrajectoryIndicator : IEnergyConsumer<ITrajectoryIndicator>
    {
        public float DelayActivateTime { get; set; }
        
        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            return this; 
        }
        
        public ITrajectoryIndicator SetStartPosition(Transform startPosition)
        {
            return this;
        }
        
        public ITrajectoryIndicator SetEndPosition(Vector3 endPosition)
        {
            return this;
        }
        
        public virtual void Initialize()
        {
            
        }
        
        public void Indicate();
        public void Activate();
        public void DeActivate(); 
    }
}
