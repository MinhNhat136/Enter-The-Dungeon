using System;
using System.Collections.Generic;
using Atomic.Core.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controls the behavior of an AI character, including movement, rolling, jumping, attack, and other actions.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class AiMotorController : SerializedMonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------

        
        //  Properties ------------------------------------
        public ICombatController CombatController { get; set; }
        public ILocomotionController LocomotionController { get; set;}
        public AiRollController RollController { get; set; }
        public BaseAgent Model { get; private set; }
        
        public NavMeshAgent BaseNavMeshAgent { get; private set; }
        public Animator BaseAnimator { get; private set; }
        public Vector2 MoveInput { get; set; }
        public Vector3 MoveDirection { get; set; }

        public float MaxMoveSpeed { get; private set; }
        public float MaxRotationSpeed { get; private set; }
        
        public float MoveSpeed 
        {
            get => BaseNavMeshAgent.speed;
            set => BaseNavMeshAgent.speed = value; 
        }
        
        public float Acceleration 
        {  
            get => BaseNavMeshAgent.acceleration;
            set => BaseNavMeshAgent.acceleration = value; 
        }
        
        public float RotationSpeed 
        { 
            get => BaseNavMeshAgent.angularSpeed; 
            set => BaseNavMeshAgent.angularSpeed = value ; 
        }

        public bool IsInitialized { get; private set; }
        
        //  Collections -----------------------------------
        [SerializeField]
        private Dictionary<CombatMode, ICombatController> combatModes;

        
        //  Fields ----------------------------------------
        
        [Header("CONFIG")]
        [SerializeField]
        private MotorConfig config;
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;

                BaseNavMeshAgent = GetComponent<NavMeshAgent>();
                BaseAnimator = GetComponentInChildren<Animator>();
                
                config.Assign(this);
                MaxMoveSpeed = MoveSpeed;
                MaxRotationSpeed = RotationSpeed;
                
                foreach (var key in combatModes.Keys)
                {
                    combatModes[key].Initialize(model);
                }
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("AiMotorController not initialized");
            }
        }

        //  Unity Methods   -------------------------------

        
        //  Other Methods ---------------------------------

        public void ApplyDirection() => MoveDirection = new Vector3(MoveInput.x, 0, MoveInput.y);
        public void SetForwardDirection()
        {
            if (MoveDirection == Vector3.zero)
            {
                return;
            }
            transform.rotation = Quaternion.LookRotation(MoveDirection);
        }

        public void SwitchCombatMode(CombatMode combatMode)
        {
            if (combatModes.TryGetValue(combatMode, out var mode)) CombatController = mode;
        }
        
        //  Event Handlers --------------------------------



    }
}