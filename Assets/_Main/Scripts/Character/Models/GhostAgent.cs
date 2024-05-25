using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    public class GhostAgent : EnemyAgent
    {
        public new bool CanPerformAttack => Time.time - timeLastAttack > 6 &&
                                            base.CanPerformAttack;

        public bool CanPerformDive => Time.time - TimeLastDiving > 8 && 
                                      base.CanPerformAttack;

        public bool IsDiving;
        public float TimeLastDiving;
        
        
        public void ApplyFloat()
        {
            
        }

        public void ApplyRise()
        {
            TimeLastDiving = Time.time;
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
            if (Physics.Raycast(modelTransform.position + Vector3.up, modelTransform.forward, out var hit,  MotorController.LocomotionController.MoveSpeed * Time.deltaTime,
                    LayerMask.NameToLayer("Obstacle")))
            {
                if (NavMesh.SamplePosition(hit.point, out var navHit, 1, NavMesh.AllAreas))
                {
                    var direction = (navHit.position - modelTransform.position).normalized;
                    modelTransform.position += direction * MotorController.LocomotionController.MoveSpeed * Time.deltaTime;
                }
                else
                {
                    IsDiving = false;
                    return;
                }
            }
            if (NavMesh.SamplePosition(transform.position + transform.forward * MotorController.LocomotionController.MoveSpeed * Time.deltaTime, 
                    out var newNavHit, 1, NavMesh.AllAreas))
            {
                modelTransform.position += modelTransform.forward * MotorController.LocomotionController.MoveSpeed * Time.deltaTime;
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
