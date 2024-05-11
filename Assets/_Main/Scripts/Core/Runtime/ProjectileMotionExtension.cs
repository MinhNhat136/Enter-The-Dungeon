using UnityEngine;

namespace Atomic.Core
{
    public static class ProjectileMotionExtension 
    {
        public static Vector3 CalculateInitialVelocity(Transform startTransform, Vector3 endPosition, float angle)
        {
            var tanAlpha = Mathf.Tan(angle * Mathf.Deg2Rad);
            var startPosition = startTransform.position;
            var distance = Vector3.Distance(startPosition, endPosition);
            var height =  endPosition.y - startPosition.y ;
            
            var velocityXZ = Mathf.Sqrt(
                -9.81f * distance * distance
                / (2.0f * (height - distance * tanAlpha))
            );
            var velocityY = tanAlpha * velocityXZ ;

            var startForward = startTransform.forward;
            var forwardAngle = Mathf.Atan2(startForward.z, startForward.x);
            
            var velocityX = velocityXZ * Mathf.Cos(forwardAngle);
            var velocityZ = velocityXZ * Mathf.Sin(forwardAngle);

            var velocity = new Vector3(velocityX, velocityY, velocityZ);
            return velocity;
        }

        public static Vector3 CalculateVelocityAtTime(Vector3 velocity, float time)
        {
            var velocityY  = velocity.y + -9.81f * time;
            return new Vector3(velocity.x, velocityY, velocity.z);
        }

        public static float TimeToReachTarget(float distance, Vector3 velocity) => distance / Mathf.Sqrt(velocity.z * velocity.z + velocity.x * velocity.x);

        public static Vector3 CalculatePosition(Vector3 startPosition, Vector3 endPosition, Vector3 velocity, float time)
        {
            var result = startPosition + velocity * time;
            result.y = 0.5f * time * time * -9.81f  +
                       velocity.y * time + (startPosition.y - endPosition.y);
            return result;
        }
    }    
}

