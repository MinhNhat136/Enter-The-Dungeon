using JetBrains.Annotations;
using UnityEngine;

namespace Atomic.Equipment
{
    public interface ITrajectoryIndicator
    {
        // Position and Rotation ----------------------------------------
        public virtual ITrajectoryIndicator SetPosition(Vector3 position)
        {
            return this; 
        }
        
        public virtual ITrajectoryIndicator SetLaunchPosition(Vector3 launchPosition)
        {
            return this;
        }

        public ITrajectoryIndicator SetTarget([CanBeNull] Transform targetPosition)
        {
            return this;
        }

        public virtual ITrajectoryIndicator SetForwardDirection(Vector3 forwardDirection)
        {
            return this;
        }
        
        // Distance ----------------------------------------
        public virtual ITrajectoryIndicator SetLaunchDistance(float distance)
        {
            return this;
        }
        
        public virtual ITrajectoryIndicator SetMaxDistance(float value)
        {
            return this;
        }
        
        // Angle ----------------------------------------
        public virtual ITrajectoryIndicator SetSpreadAngle(float spreadAngle)
        {
            return this;
        }

        public void Set();
        public void Indicate();
        public void Activate(float delayTime = 0);
        public void DeActivate(); 
    }
}
