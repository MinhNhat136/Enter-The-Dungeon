using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Equipment
{
    public class StraightIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 ForwardDirection { get; set; }
        public float MaxDistance { get; set; }
        public float SpreadAngle { get; set; }
        public float Radius { get; set; }

        public void Indicate()
        {
            
        }
    }
    
}
