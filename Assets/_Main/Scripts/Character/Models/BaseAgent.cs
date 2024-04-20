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
    public enum CharacterActionState : short
    {
        None = 0,
        MoveNextSkill = 1 << 0,
        Idle = 1 << 1,
        SwappingWeapon = 1 << 2,
        Rolling = 1 << 3,
        BeginPrepareAttack = 1 << 4,
        PreparingAttack = 1 << 5, 
        EndPrepareAttack = 1 << 6,
        BeginAttackMove = 1 << 7,
        AttackMoving = 1 << 8,
        EndAttackMove = 1 << 9,
        BeginAttack = 1 << 10,
        Attacking = 1 << 11,
        EndAttack = 1 << 12,
    }
    
    public enum Command : byte
    {
        Move,
        Roll,
        PrepareAttack,
        Attack,
        SwapWeapon,
    }
    
    public class EnumFlagsAttribute : PropertyAttribute
    {
        public EnumFlagsAttribute() { }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            int newValue = EditorGUI.MaskField(position, label, property.intValue, property.enumDisplayNames);
            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }
    }
#endif
    
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
        [field: EnumFlags]
        public CharacterActionState CurrentActionState { get; set; }
        public Dictionary<CharacterActionType, Action> ActionTriggers { get; } = new();
        
        [field: SerializeField]
        public Dictionary<Command, bool> CommandEvents { get; } = new(8);
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

