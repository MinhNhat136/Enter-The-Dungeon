using System;
using System.Collections.Generic;
using Atomic.Core;
using Atomic.Core.Interface;
using Atomic.Equipment;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    [Flags]
    public enum Command : byte
    {
        Interrupt = 1 << 0,
        Move = 1 << 1,
        Roll = 1 << 2,
        PrepareAttack = 1 << 4,
        AttackMove = 1 << 5,
        Attack = 1 << 6,
        SwapWeapon = 1 << 7,
    }
    
    /// <summary>
    /// Base class for defining characters with modular control systems.
    /// </summary>
    [RequireComponent(
        typeof(NavMeshAgent), 
        typeof(AiHealth), 
        typeof(AiPassiveStateController))]
    public abstract class BaseAgent : MonoBehaviour, IInitializable, ICharacterActionTrigger
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        #region Config Runtime
        public bool IsInitialized => _isInitialized;
        public Transform BodyWeakPoint => bodyWeakPoint;
        public BaseAgent TargetAgent { get; set; }
        
        [field: SerializeField]
        public bool IsPlayer { get; protected set; }
        
        public WeaponScriptableObject CurrentWeapon { get; set; }
        public CombatMode CurrentCombatMode { get; set; }

        protected virtual CharacterActionType DefaultActionState { get; set; }
        private Dictionary<CharacterActionType, Action> ActionTriggers { get; } = new();

        [field: SerializeField]
        public CharacterActionType CurrentActionState { get; set; }
        [field: SerializeField]
        public Command Command { get; set; }

        #endregion

        #region Module Controllers

        protected IAgentAnimator AgentAnimatorController => _agentAnimatorController;
        public AiMotorController MotorController => _motorController;
        public AiMemoryController MemoryController => _memoryController;
        public AiVisionSensorController VisionController => _visionController;
        public ITargetingController TargetingController
        {
            get => _targetingController;
            protected set => _targetingController = value;
        }
        protected AiWeaponVisualsController WeaponVisualsController => _weaponVisualsController;
        public AiPassiveStateController PassiveStateController => _passiveStateController;
        public AiHealth HealthController => _healthController;

        #endregion
        
        //  Collections -----------------------------------


        //  Fields ----------------------------------------
        [SerializeField] 
        private Transform bodyWeakPoint;
        private bool _isInitialized;

        #region Controller
        private IAgentAnimator _agentAnimatorController;
        private AiVisionSensorController _visionController;
        private ITargetingController _targetingController;
        private AiMotorController _motorController;
        private AiWeaponVisualsController _weaponVisualsController;
        private AiPassiveStateController _passiveStateController;
        
        [SerializeField]
        private AiHealth _healthController;
        private AiMemoryController _memoryController;
        #endregion
        
        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                DefaultActionState |= CharacterActionType.EndAttack | CharacterActionType.EndAttackMove |
                                      CharacterActionType.EndPrepareAttack | CharacterActionType.EndRoll |
                                      CharacterActionType.MoveNextSkill;

                CurrentActionState = DefaultActionState;
                Command |= Command.Move;
                
                AssignControllers();
            }
        }


        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new System.Exception("Base Agent not initialized");
            }
        }

        public virtual void DoEnable()
        {

        }

        public virtual void DoDisable()
        {

        }

        //  Unity Methods   -------------------------------
        void OnDestroy()
        {
        }

        //  Other Methods ---------------------------------
        public abstract void Assign();

        private void AssignControllers()
        {
            _memoryController = new()
            {
                MemorySpan = 1
            };
            this.AttachControllerToModel(out _healthController);
            this.AttachControllerToModel(out _agentAnimatorController);
            this.AttachControllerToModel(out _visionController);
            this.AttachControllerToModel(out _motorController);
            this.AttachControllerToModel(out _targetingController);
            this.AttachControllerToModel(out _weaponVisualsController);
            this.AttachControllerToModel(out _passiveStateController);
        }
        
        //  Event Handlers --------------------------------
        protected void RegisterActionTrigger(CharacterActionType actionType, Action action)
        {
            if (!ActionTriggers.TryAdd(actionType, action))
            {
                ActionTriggers[actionType] += action;
            }
        }

        public void OnCharacterActionTrigger(CharacterActionType actionType)
        {
            if (ActionTriggers.TryGetValue(actionType, out var trigger))
            {
                trigger.Invoke();
            }
        }
        
    }
}

