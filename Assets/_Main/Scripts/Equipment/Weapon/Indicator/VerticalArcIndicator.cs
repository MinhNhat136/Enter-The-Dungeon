using UnityEngine;

namespace Atomic.Equipment
{
    public class VerticalArcIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        public Vector3 Position { get; set; }
        public Vector3 LaunchPosition { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Vector3 ForwardDirection { get; set; }
        public float MaxDistance { get; set; }
        public float SpreadAngle { get; set; }
        public float MaxRadius { get; set; }
        
        [SerializeField]
        private Transform aoeIndicator;

        [SerializeField]
        private Transform aoeOriginalIndicator;

        [SerializeField]
        private LineRenderer line;

        [SerializeField]
        private float angle;

        [SerializeField]
        private float gravity;
        
        public void Indicate()
        {
            
        }

        public void TurnOn()
        {
            
        }

        public void TurnOff()
        {
            
        }
    }
}