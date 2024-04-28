using System;
using UnityEngine;

namespace  Atomic.Equipment
{
    public interface IProjectileTrajectoryController
    {
        public void ApplyTrajectory(ProjectileBase projectile);
        public IProjectileTrajectoryController Clone();
    }

    
    public class StraightTrajectory : IProjectileTrajectoryController
    {
        public void ApplyTrajectory(ProjectileBase projectile)
        {
            projectile.Rigidbody.AddForce(projectile.ShootForce, ForceMode.VelocityChange);
        }

        public IProjectileTrajectoryController Clone()
        {
            return new StraightTrajectory();
        }
    }
    
    public class VerticalArcTrajectory : IProjectileTrajectoryController
    {
       private float initialSpeed;
       private Vector3 initialDirection;
       private Vector3 gravityDirection;
       private float gravityMagnitude;
       private float maxDistance;

    public void ApplyTrajectory(ProjectileBase projectile)
    {
        // if (initialSpeed == 0f)
        // {
        //     /*initialSpeed = projectile.InitialVelocity.magnitude;
        //     initialDirection = projectile.InitialVelocity.normalized;
        //     gravityDirection = Vector3.down; 
        //     gravityMagnitude = projectile.Rigidbody.mass * Mathf.Abs(Physics.gravity.y);
        //     maxDistance = 10f; *//**/
        // }
        //
        // // float distanceTravelled = Vector3.Distance(projectile.SpawnPosition, projectile.transform.position);
        //
        // if (distanceTravelled >= maxDistance)
        // {
        //     projectile.gameObject.SetActive(false);
        //     return;
        // }
        //
        // float velocityY = initialSpeed * Mathf.Sin(projectile.transform.rotation.eulerAngles.x * Mathf.Deg2Rad) - gravityMagnitude * projectile.transform.position.y;
        // Vector3 velocityXZ = initialDirection * initialSpeed * Mathf.Cos(projectile.transform.rotation.eulerAngles.x * Mathf.Deg2Rad);
        //
        // Vector3 velocity = new Vector3(velocityXZ.x, velocityY, velocityXZ.z);
        //
        // projectile.transform.position += velocity * Time.deltaTime;
    }

    public IProjectileTrajectoryController Clone()
    {
        throw new NotImplementedException();
    }
    }
    
    public class HorizontalArcTrajectory : IProjectileTrajectoryController
    {
        public void ApplyTrajectory(ProjectileBase projectile)
        {
            Debug.Log("spread");
        }

        public IProjectileTrajectoryController Clone()
        {
            throw new NotImplementedException();
        }
    }
}
