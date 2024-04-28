using JetBrains.Annotations;
using UnityEngine;

namespace Atomic.Equipment
{
    public interface ITrajectoryIndicator
    {
        public float DelayActivateTime { get; set; }
        
        // Position and Rotation ----------------------------------------
        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            return this; 
        }
        
        public ITrajectoryIndicator SetLaunchPosition(Vector3 launchPosition)
        {
            return this;
        }

        public ITrajectoryIndicator SetTarget([CanBeNull] Transform targetPosition)
        {
            return this;
        }

        public ITrajectoryIndicator SetForwardDirection(Vector3 forwardDirection)
        {
            return this;
        }
        
        // Distance ----------------------------------------
        public ITrajectoryIndicator SetLaunchDistance(float distance)
        {
            return this;
        }
        
        public ITrajectoryIndicator SetMaxDistance(float value)
        {
            return this;
        }
        
        // Angle ----------------------------------------
        public ITrajectoryIndicator SetSpreadAngle(float spreadAngle)
        {
            return this;
        }

        public void Set();
        public void Indicate();
        public void Activate();
        public void DeActivate(); 
    }
}
