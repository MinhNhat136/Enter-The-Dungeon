using System;
using System.Collections.Generic;
using Atomic.Character.Config;
using Atomic.Character.Model;
using Atomic.Core;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controls the behavior of an AI character, including movement, rolling, jumping, attack, and other actions.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class AiMotorController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>, ICharacterActionTrigger
    {
        //  Statics ---------------------------------------
        private static readonly int ControllerLocomotionIndex = 1 << 0;
        private static readonly int ControllerJumpIndex = 1 << 1;
        private static readonly int ControllerRollIndex = 1 << 2;
        private static readonly int ControllerFlyIndex = 1 << 3;

        //  Events ----------------------------------------
        
        //  Properties ------------------------------------
        public ILocomotionController LocomotionController => _locomotionController;
        public AiRollController RollController => _rollController;
        public BaseAgent Model => _model;
        public NavMeshAgent BaseNavMeshAgent => _navMeshAgent;
        public Animator BaseAnimator => _animator;
        public Vector3 MoveDirection { get; set; }

        public bool IsInitialized => _isInitialized;
        public bool IsGrounded { get; set; }
        public bool IsRolling { get; set; }
        public bool IsJumping { get; set; }

        public Dictionary<CharacterActionType, Action> ActionTriggers => _actionTriggers;
        
        //  Collections -----------------------------------
        private readonly Dictionary<CharacterActionType, Action> _actionTriggers = new(); 

        //  Fields ----------------------------------------
        [FormerlySerializedAs("_config")] 
        [SerializeField] private AgentConfig config; 

        private int _controllerBitSequence = 0;
        private bool _isInitialized;
        
        private BaseAgent _model;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private ILocomotionController _locomotionController;
        private AiRollController _rollController;
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if(!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                _navMeshAgent = GetComponent<NavMeshAgent>();
                _animator = GetComponentInChildren<Animator>();
                
                AssignLocomotionController();
                AssignRollController();
                
                _locomotionController.Initialize(this);
                _rollController.Initialize(this);
                
                
                IsGrounded = true;
                IsRolling = false;
                IsJumping = false; 

            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new Exception("AiMotorController not initialized");
            }
        }

        //  Unity Methods   -------------------------------
        private void AssignLocomotionController()
        {
            this.SetController(ref _locomotionController, ControllerLocomotionIndex, ref _controllerBitSequence);
            if(_locomotionController == null)
            {
                return; 
            }
            LocomotionController.RotationSpeed = config.RotateSpeed;
            LocomotionController.MoveSpeed = config.WalkSpeed;
            LocomotionController.Acceleration = config.Acceleration;
        }

        private void AssignRollController()
        {
            this.SetController(ref _rollController, ControllerRollIndex, ref _controllerBitSequence);
        }
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        public void OnCharacterActionTrigger(CharacterActionType actionType)
        {
            if (_actionTriggers.TryGetValue(actionType, out var trigger))
            {
                trigger.Invoke();
            }
        }
    }
}
