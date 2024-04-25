using UnityEngine;

namespace Atomic.Equipment
{
    public interface ITrajectoryIndicator
    {
        public Vector3 Position { get; set; }
        public Vector3 LaunchPosition { get; set; }
        public Vector3 TargetPosition { get; set; } 
        public Vector3 ForwardDirection { get; set; }
        public float MaxDistance { get; set; }
        public float SpreadAngle { get; set; }
        public float MaxRadius { get; set; }

        public ITrajectoryIndicator SetPosition(Vector3 position)
        {
            Position = position;
            return this; 
        }
        
        public ITrajectoryIndicator SetStartPosition(Vector3 startPosition)
        {
            LaunchPosition = startPosition;
            return this;
        }

        public ITrajectoryIndicator SetEndPosition(Vector3 endPosition)
        {
            TargetPosition = endPosition;
            return this;
        }

        public ITrajectoryIndicator SetForwardDirection(Vector3 forwardDirection)
        {
            ForwardDirection = forwardDirection;
            return this;
        }

        public ITrajectoryIndicator SetMaxDistance(float value)
        {
            MaxDistance = value;
            return this;
        }

        public ITrajectoryIndicator SetSpreadAngle(float spreadAngle)
        {
            SpreadAngle = spreadAngle;
            return this;
        }

        public ITrajectoryIndicator SetMaxRadius(float radius)
        {
            MaxRadius = radius;
            return this; 
        }
        
        public void Indicate();
        public void TurnOn();
        public void TurnOff(); 
    }
}
