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

        [Button]
        public void Indicate()
        {
            OnDrawGizmos();
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

        public void OnDrawGizmos()
        {
            Handles.color = new Color(0, 1, 0, 0.3f);
            Handles.DrawSolidArc(transform.position, transform.up, transform.forward, 60, 10);
        }
    }
}