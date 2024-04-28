using System.Collections.Generic;
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
    public class ShootConfigScriptableObject : ScriptableObject, System.ICloneable
    {
        public LayerMask HitMask;
        [FormerlySerializedAs("BulletPrefab")] public ProjectileBase bulletPrefab;
        [FormerlySerializedAs("projectileSpeed")] public float projectileShootForce;
        [FormerlySerializedAs("lifeCycle")] public float delayDisableTime;

        public List<HitEffectConfigScriptableObject> hitEffects;
        
        public object Clone()
        {
            throw new System.NotImplementedException();
        }
        
        
    }
}
