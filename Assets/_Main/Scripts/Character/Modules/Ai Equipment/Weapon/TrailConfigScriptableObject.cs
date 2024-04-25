using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Equipment
{
    [CreateAssetMenu(fileName = "Trail Config", menuName = "Weapons/Ranged/Trail Config", order = 4)]
    public class TrailConfigScriptableObject : ScriptableObject, System.ICloneable
    {
        public Material Material;
        public AnimationCurve WidthCurve;
        public float Duration = 0.5f;
        public float MinVertexDistance = 0.1f;
        public Gradient Color;

        public float MissDistance = 100f;
        public float SimulationSpeed = 100f;
        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
