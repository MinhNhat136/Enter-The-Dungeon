using System.Collections.Generic;
using Atomic.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Equipment
{
    public enum ProjectileTrajectoryEnum
    {
        Straight, 
        VerticalArc,
        HorizontalArc,
    }
    
    [CreateAssetMenu(fileName = "Shoot Config", menuName = "Weapons/Ranged/Shoot Config", order = 2)]
    public class ProjectileConfigScriptableObject : ScriptableObject, System.ICloneable
    {
        public LayerMask HitMask;
        [FormerlySerializedAs("BulletPrefab")] public ProjectileBase bulletPrefab;
        public float projectileSpeed;
        public float lifeCycle;

        public List<HitEffectConfigScriptableObject> hitEffects;
        public DamageConfigScriptableObject damageConfig; 
        
        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
