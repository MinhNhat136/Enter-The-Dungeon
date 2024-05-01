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
    public class ShootBuilderScriptableObject : ScriptableObject, System.ICloneable
    {
        public ProjectileTrajectoryEnum trajectoryType;

        public LayerMask HitMask;
        public ProjectileBase bulletPrefab;
        
        public IProjectileTrajectoryController TrajectoryController { get; private set; }
        
        public void Initialize()
        {
            SetTrajectoryController();
        }
        
        public void Destroy()
        {
            TrajectoryController = null;
        }
        
        
        private void SetTrajectoryController()
        {
            switch (trajectoryType)
            {
                case ProjectileTrajectoryEnum.Straight:
                    TrajectoryController = new StraightTrajectory();
                    break;
                case ProjectileTrajectoryEnum.VerticalArc:
                    TrajectoryController = new VerticalArcTrajectory();
                    break;
                case ProjectileTrajectoryEnum.HorizontalArc:
                    TrajectoryController = new HorizontalArcTrajectory();
                    break;
                default:
                    TrajectoryController = null;
                    break;
            }
        }
        
        public object Clone()
        {
            throw new System.NotImplementedException();
        }
        
        
    }
}
