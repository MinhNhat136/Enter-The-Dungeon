using Atomic.Equipment;
using UnityEngine;

public class FollowAction : ActionOnShoot
{
    public Transform target; 
    public float rotationSpeed = 5.0f;
    public float gravity = -9.81f;
    
    public override void Initialize()
    {
        if (projectile.Owner.TargetAgent)
        {
            target = projectile.Owner.TargetAgent.LockPivot;
        }
    }

    private void Update()
    {
        OnShoot(transform.position, transform.forward);

    }

    public override void OnShoot(Vector3 point, Vector3 normal)
    {
        if (target == null)
        {
            return;
        }
        Vector3 newPosition = projectile.MyTransform.position;
        newPosition.y += Time.deltaTime * gravity;
        projectile.MyTransform.position = newPosition;
        Vector3 directionToTarget = (target.position - projectile.transform.position).normalized;
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        projectile.transform.rotation = Quaternion.RotateTowards(
            projectile.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}