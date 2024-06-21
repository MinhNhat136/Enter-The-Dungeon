using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------
    public enum LocomotionType
    {
        Basic,
        RootMotion,
    }

    //  Class Attributes ----------------------------------
    [System.Serializable]
    public class LocomotionConfig
    {
        public float speed;
        public float rotateSpeed;
        public float acceleration;
        public bool autoRotateWithNavmesh;
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

            motorController.LocomotionController.Initialize(motorController);

            motorController.RotationSpeed = rotateSpeed;
            motorController.MoveSpeed = speed;
            motorController.Acceleration = acceleration;
            motorController.LocomotionController.IsNavMeshRotate = autoRotateWithNavmesh;
        }
    }

    [System.Serializable]
    public class RollConfig
    {
        [FormerlySerializedAs("speed")] public float boostSpeedValue; 
        public LayerMask colliderLayer;

        public void Assign(AiMotorController motorController)
        {
            motorController.RollController = new()
            {
                BoostSpeedValue = boostSpeedValue,
                ColliderLayer = colliderLayer
            };

            motorController.RollController.Initialize(motorController);
        }
    }
  
    
    /// <summary>
    /// Configuration asset defining the motor abilities and behaviors for AI agents.
    /// </summary>
    [CreateAssetMenu(menuName = "Ai Module Config/Motor")]
    public class MotorConfig : SerializedScriptableObject
    {
        [Header("ABILITIES")] 
        public bool canWalk;
        public bool canRoll;

        [ShowIf("canWalk")] public LocomotionConfig locomotionConfig = new();
        [ShowIf("canRoll")] public RollConfig rollConfig = new();

        [Header("NAVMESH")] 
        public int avoidancePriorityNormal;
        public int avoidancePriorityAttack;
        public float enemyCloseRange;
        public float deadDuration;

        public void Assign(AiMotorController motorController)
        {
            if (canWalk) locomotionConfig.Assign(motorController);
            if (canRoll) rollConfig.Assign(motorController);
        }
    }
}