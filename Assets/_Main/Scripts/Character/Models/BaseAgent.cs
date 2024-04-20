using System;
using System.Collections.Generic;
using Atomic.Character.Module;
using Atomic.Core;
using Atomic.Core.Interface;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Atomic.Character.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    [Flags]
    public enum Command : byte
    {
        Move,
        Roll,
        PrepareAttack,
        AttackMove,
        Attack,
        SwapWeapon,
    }
    
    /// <summary>
    /// Base class for defining characters with modular control systems.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseAgent : SerializedMonoBehaviour, IInitializable, ICharacterActionTrigger
    {
        //  Statics ---------------------------------------


        //  Events ----------------------------------------


        //  Properties ------------------------------------
        #region Config Runtime
        public bool IsInitialized => _isInitialized;
        public Transform BodyWeakPoint => bodyWeakPoint;
        public BaseAgent TargetAgent { get; set; }

        #endregion

        #region Module Controllers 
        public virtual IAgentAnimator AgentAnimatorController
        {
            get => _agentAnimatorController;
            protected set => _agentAnimatorController = value;
        }
        public virtual AiMotorController MotorController
        {
            get => _motorController;
            protected set => _motorController = value;
        }

        public virtual AiHitBoxController HitBoxController
        {
            get => _hitBoxController;
            protected set => _hitBoxController = value;
        }

        public virtual AiMemoryController MemoryController
        {
            get => _memoryController;
            protected set => _memoryController = value;
        }

        public virtual IVisionController VisionController
        {
            get => _visionController;
            protected set => _visionController = value;
        }
        public virtual ITargetingController TargetingController
        {
            get => _targetingController;
            protected set => _targetingController = value;
        }
        public virtual AiWeaponVisualsController WeaponVisualsController
        {
            get => _weaponVisualsController;
            set => _weaponVisualsController = value; 
        }
        
        public virtual AiHealth HealthController => _healthController;

        #endregion


        [field: SerializeField]
        public CharacterActionType CurrentActionState { get; set; }
        public virtual CharacterActionType DefaultActionState { get; set; } 
        public Dictionary<CharacterActionType, Action> ActionTriggers { get; } = new();
        
        [field: SerializeField]
        public Command Command { get; set; }
        public CombatMode CurrentCombatMode { get; set; }
        
        //  Collections -----------------------------------


        //  Fields ----------------------------------------
        [FormerlySerializedAs("_bodyWeakPoint")] 
        [SerializeField] private Transform bodyWeakPoint;

        private bool _isInitialized;

        private AgentsManager _agentManager;

        #region Controller
        private IAgentAnimator _agentAnimatorController;
        private IVisionController _visionController;
        private ITargetingController _targetingController;
        private AiHitBoxController _hitBoxController;
        private AiMotorController _motorController;
        private AiWeaponVisualsController _weaponVisualsController;
        #endregion

        #region Shared Controller
        private AiHealth _healthController;
        private AiMemoryController _memoryController;
        #endregion

        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _agentManager = FindObjectOfType<AgentsManager>();
                _agentManager.RegisterAgent(this);

                DefaultActionState |= CharacterActionType.EndAttack | CharacterActionType.EndAttackMove |
                                      CharacterActionType.EndPrepareAttack | CharacterActionType.EndRoll |
                                      CharacterActionType.MoveNextSkill;

                CurrentActionState = DefaultActionState;
                
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
            _agentManager.UnRegisterAgent(this);
        }

        //  Other Methods ---------------------------------
        public abstract void Assign();
        
        protected virtual void AssignControllers()
        {
            _memoryController = new()
            {
                MemorySpan = 1
            };
            
            
            _healthController = new AiHealth();
            
            this.AttachControllerToModel(out _agentAnimatorController);
            this.AttachControllerToModel(out _visionController);
            this.AttachControllerToModel(out _targetingController);
            this.AttachControllerToModel(out _motorController);
            this.AttachControllerToModel(out _hitBoxController);
            this.AttachControllerToModel(out _weaponVisualsController);
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

