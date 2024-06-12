using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    public class GhostAgent : EnemyAgent
    {
        public bool IsDiving;
        public float LastDivingTime { get; private set; }
        public float SinceLastDivingTime => Time.time - LastDivingTime;
        
        public void ApplyFloat()
        {
            
        }

        public void ApplyRise()
        {
            LastDivingTime = Time.time;
        }

        public void ApplyDive()
        {
            IsDiving = true;
        }

        private Vector3 _diveDestination;

        public void GoToTheEdge()
        {
            if (!IsDiving)
            {
                return;
            }
            if (Physics.Raycast(modelTransform.position + Vector3.up, modelTransform.forward, out var hit,  MotorController.MoveSpeed * Time.deltaTime,
                    LayerMask.NameToLayer("Obstacle")))
            {
                if (NavMesh.SamplePosition(hit.point, out var navHit, 1, NavMesh.AllAreas))
                {
                    var direction = (navHit.position - modelTransform.position).normalized;
                    modelTransform.position += direction * MotorController.MoveSpeed * Time.deltaTime;
                }
                else
                {
                    IsDiving = false;
                    return;
                }
            }
            if (NavMesh.SamplePosition(transform.position + transform.forward * MotorController.MoveSpeed * Time.deltaTime, 
                    out var newNavHit, 1, NavMesh.AllAreas))
            {
                modelTransform.position += modelTransform.forward * MotorController.MoveSpeed * Time.deltaTime;
            }
            else
            {
                IsDiving = false;
            }
        }
        
        public void RandomDirection(float maxArc)
        {
            float randomArc = Random.Range(0, maxArc);
            CalculateMoveDirectionFromArc(randomArc);
        }

        
    }
    
}
