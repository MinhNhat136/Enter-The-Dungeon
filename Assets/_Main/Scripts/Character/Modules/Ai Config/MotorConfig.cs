using Atomic.Character.Module;
using Atomic.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.Character.Config
{
    [CreateAssetMenu(menuName = "Ai Module Config/Motor")]
    public class MotorConfig : BaseSO
    {
        [Header("ABILITIES")] 
        public bool canWalk;
        public bool canFly;
        public bool canRoll;
        public bool canJump;
        public bool canMeleeCombat;
        public bool canRangedCombat; 

        [ShowIf("canWalk")] public LocomotionConfig locomotionConfig = new();
        [ShowIf("canRoll")] public RollConfig rollConfig = new(); 
        
        [Header("NAVMESH")]
        public int avoidancePriorityNormal;
        public int avoidancePriorityAttack;
        public float enemyCloseRange;
        public float deadDuration;

        public void Assign(AiMotorController motorController)
        {
            if(canWalk) locomotionConfig.Assign(motorController);
            if(canRoll) rollConfig.Assign(motorController);
        }
        
    }

    public enum LocomotionType
    {
        Basic,
        RootMotion,
    }
    
    [System.Serializable]
    public class LocomotionConfig
    {
        public float walkSpeed;
        public float runSpeed;
        public float rotateSpeed;
        public float acceleration;
        public LocomotionType type;

        public void Assign(AiMotorController motorController)
        {
            switch (type)
            {
                case LocomotionType.Basic:
                    motorController.LocomotionController = new AiBasicLocomotionController();
                    break; 
                case LocomotionType.RootMotion:
                    break; 
            }
            
            motorController.LocomotionController.RotationSpeed = rotateSpeed;
            motorController.LocomotionController.MoveSpeed = walkSpeed;
            motorController.LocomotionController.Acceleration = acceleration;
            
            motorController.LocomotionController.Initialize(motorController);
            Debug.Log(motorController.GetInstanceID());
        }
    }
    
    [System.Serializable]
    public class RollConfig
    {
        public float distance;
        public LayerMask colliderLayer; 

        public void Assign(AiMotorController motorController)
        {
            motorController.RollController = new()
            {
                Distance = distance,
                ColliderLayer = colliderLayer
            };

            motorController.RollController.Initialize(motorController);
        }
    }
}
