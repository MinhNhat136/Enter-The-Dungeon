using System;
using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    public class ForceConfigScriptableObject : MonoBehaviour, ICloneable
    {
        [field: SerializeField]
        public float ForceStrength { get; set; }

        [field: SerializeField]
        public ParticleSystem.MinMaxCurve DistanceFallOff { get; set; }

        public Vector3 GetForceStrength(Vector3 direction, float distance)
        {
            return ForceStrength * DistanceFallOff.Evaluate(distance) * direction;
        }

        public object Clone()
        {
            return this;
        }
   
    }
    
}
