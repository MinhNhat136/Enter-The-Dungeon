using Atomic.Core;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerTargetingAgentController : BaseTargetingAgentController
    {
        protected override float CalculateScore(AiMemoryObject memory)
        {
            Vector3 aimDirection = Model.MotorController.MoveDirection.normalized;
            Vector3 direction = memory.gameObject.transform.position - transform.position;
            
            Vector3 targetDirection = direction.normalized;
            
            aimDirection.y = 0;
            targetDirection.y = 0;
            
            return memory.distance.Normalize(visionSensor.config.distance) + Vector3.Dot(aimDirection,targetDirection) * config.angleWeight ;
        }
    }
}