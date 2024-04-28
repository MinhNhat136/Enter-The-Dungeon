using UnityEngine;

namespace Atomic.Equipment
{
    public class SpreadIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        public Vector3 Position { get; set; }
        public Vector3 LaunchPosition { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Vector3 ForwardDirection { get; set; }
        public float LaunchDistance { get; set; }
        public float MaxDistance { get; set; }
        public float SpreadAngle { get; set; }
        public float MaxRadius { get; set; }

        public void Set()
        {
            throw new System.NotImplementedException();
        }

        public void Indicate()
        {
            
        }

        public void Activate(float delayTime = 0)
        {
            throw new System.NotImplementedException();
        }


        public void DeActivate()
        {
            throw new System.NotImplementedException();
        }
    }
}