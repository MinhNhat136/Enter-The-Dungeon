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
        public override float CalculateScore(AiMemoryObject memory)
        {
            Vector3 aimDirection = Model.MotorController.MoveDirection.normalized;
            Vector3 targetDirection = (memory.gameObject.transform.position - transform.position).normalized;
            aimDirection.y = 0;
            targetDirection.y = 0;
            
            return  Vector3.Dot(aimDirection,targetDirection) * AngleWeight;
        }
    }
}