using UnityEngine;

namespace Atomic.Equipment
{
    public interface ITrajectoryIndicator
    {
        public float DelayActivateTime { get; set; }
        public float IndicateValue { get; set; }

        public ITrajectoryIndicator SetDistanceWeight(float distanceWeight)
        {
            return this;
        }

        public ITrajectoryIndicator SetTimeWeight(float timeWeight)
        {
            return this;
        }

        public ITrajectoryIndicator SetScaleWeight(float scaleWeight)
        {
            return this;
        }

        public ITrajectoryIndicator SetRadiusWeight(float radiusWeight)
        {
            return this;
        }
        
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

        public ITrajectoryIndicator SetForwardDirection(Vector3 forwardDirection)
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
