using System;
using Sirenix.OdinInspector;
using UnityEditor;
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

        public float DelayActivateTime { get; set; }
        public float EnergyValue { get; set; }
        public float MinEnergyValue { get; set; }
        public float MaxEnergyValue { get; set; }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Indicate()
        {
            throw new NotImplementedException();
        }


        public void Activate()
        {
            throw new NotImplementedException();
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