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
    public sealed class AiMotorController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>,
        ICharacterActionTrigger
    {
        //  Statics ---------------------------------------

        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public ILocomotionController LocomotionController
        {
            get; set;
        }

        public AiRollController RollController
        {
            get; set;
        }
        
        public BaseAgent Model => _model;
        public NavMeshAgent BaseNavMeshAgent => _navMeshAgent;
        public Animator BaseAnimator => _animator;
        public Vector3 MoveDirection { get; set; }

        public bool IsInitialized => _isInitialized;
        public bool IsGrounded { get; set; }
        public bool IsRolling { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsJumping { get; set; }

        public Dictionary<CharacterActionType, Action> ActionTriggers => _actionTriggers;

        //  Collections -----------------------------------
        private readonly Dictionary<CharacterActionType, Action> _actionTriggers = new();

        //  Fields ----------------------------------------
        [FormerlySerializedAs("_config")] [SerializeField]
        private MotorConfig config;

        private bool _isInitialized;

        private BaseAgent _model;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                _navMeshAgent = GetComponent<NavMeshAgent>();
                _animator = GetComponentInChildren<Animator>();

                config.Assign(this);
                ResetStateFlag();
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

        //  Other Methods ---------------------------------
        private void ResetStateFlag()
        {
            IsGrounded = true;
            IsRolling = false;
            IsJumping = false;
            IsAttacking = false;
        }
        
        //  Event Handlers --------------------------------
        public void RegisterActionTrigger(CharacterActionType actionType, Action action)
        {
            if (!_actionTriggers.TryAdd(actionType, action))
            {
                _actionTriggers[actionType] += action;
            }
        }
        
        public void OnCharacterActionTrigger(CharacterActionType actionType)
        {
            if (_actionTriggers.TryGetValue(actionType, out var trigger))
            {
                trigger.Invoke();
            }
        }
    }
}