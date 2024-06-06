using System;
using System.Collections.Generic;
using Atomic.AbilitySystem;
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

    public enum AgentCondition
    {
        Normal  = 1 << 0, 
        Hit = 1 << 1,
        Stun = 1 << 2,
        Break = 1 << 3,
        Knockdown = 1 << 4, 
    }

    public enum SurvivalState
    {
        Revive,
        Alive,
        Dead,
    }
    
    /// <summary>
    /// Base class for defining characters with modular control systems.
    /// </summary>
    [RequireComponent(
        typeof(NavMeshAgent),
        typeof(AiAbilityController))]
    [RequireComponent(
        typeof(AiImpactSensorController),
        typeof(AiVisionSensorController),
        typeof(AiImpactReactionController))]
    public abstract class BaseAgent : MonoBehaviour, IInitializable, ICharacterActionTrigger
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        [field: SerializeField]
        public bool IsPlayer { get; protected set; }
        [field: SerializeField]
        public float AgentMemorySpan { get; private set; }
        [field: SerializeField]
        public Transform LockPivot { get; private set; }

        public bool IsInitialized { get; private set; }
        public BaseAgent TargetAgent { get; set; }
        
        protected CharacterActionType DefaultActionState { get; set; }
        public WeaponBuilder CurrentWeapon { get; set; }
        public Command Command { get; set; }
        public CharacterActionType CurrentActionState { get; set; }
        public CombatMode CurrentCombatMode { get; set; }
        public SurvivalState SurvivalState { get; set; } = SurvivalState.Revive;
        public AgentCondition AgentCondition { get; set; } = AgentCondition.Normal;
        
        private Collider AgentCollider { get; set; }
        public Vector3 ImpactHit { get; set; }
        public AiBodyPart[] BodyParts { get; set; } 
        public float Height { get; private set; }
        public float Width { get; private set; }
        public float SpeedRatio { get; protected set; }
        public float StabilityRatio { get; protected set; }

        public bool IsMovement => !Mathf.Approximately(MotorController.MoveInput.sqrMagnitude, 0f);
        public float LastAttackTime { get; set; }
        
        private Dictionary<CharacterActionType, Action> ActionTriggers { get; } = new();

        public AttributeSystemComponent AttributeSystemComponent => _attributeSystemComponent;
        public AiAbilityController AiAbilityController => _aiAbilityController;
        public NavMeshAgent NavmeshAgent { get; private set; }
        public AgentAnimator AgentAnimatorController => _agentAnimatorController;
        public AiMotorController MotorController => _motorController;
        public AiMemoryController MemoryController => _memoryController;
        public AiVisionSensorController VisionController => _visionController;
        public AiImpactSensorController ImpactSensorController => _impactSensorController;
        public ITargetingController TargetingController
        {
            get => _targetingController;
            protected set => _targetingController = value;
        }
        protected AiWeaponVisualsController WeaponVisualsController => _weaponVisualsController;
        
        //  Collections -----------------------------------
        
        //  Fields ----------------------------------------
        [SerializeField] 
        public AiConfig config; 
        
        [HideInInspector] 
        public Transform modelTransform;

        [Header("SPEC")]
        public AttributeScriptableObject stabilityRatio;
        public AttributeScriptableObject speedAttribute;
        public AttributeScriptableObject healthAttribute; 
        
        private AttributeSystemComponent _attributeSystemComponent;
        private AiAbilityController _aiAbilityController;
        private AgentAnimator _agentAnimatorController;
        private AiVisionSensorController _visionController;
        private AiImpactSensorController _impactSensorController;
        private ITargetingController _targetingController;
        private AiMotorController _motorController;
        private AiWeaponVisualsController _weaponVisualsController;
        private AiImpactReactionController _impactReactionController;
        private AiMemoryController _memoryController;
        
        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            DefaultActionState |= CharacterActionType.EndAttack | CharacterActionType.EndAttackMove |
                                  CharacterActionType.EndPrepareAttack | CharacterActionType.EndRoll |
                                  CharacterActionType.MoveNextSkill;
            CurrentActionState = DefaultActionState;
            Command |= Command.Move;

            NavmeshAgent = GetComponent<NavMeshAgent>();
            AgentCollider = GetComponent<Collider>();
            BodyParts =  GetComponentsInChildren<AiBodyPart>();
            
            modelTransform = transform;
            Height = NavmeshAgent.height;
            Width = NavmeshAgent.radius;
                
            AssignControllers();
            
        }
        
        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Base Agent not initialized");
            }
        }

        public virtual void DoEnable()
        {
            SurvivalState = SurvivalState.Revive;
            
        }

        public virtual void DoDisable()
        {
            Command = 0;
            SurvivalState = SurvivalState.Dead;
            CurrentActionState = DefaultActionState;
            AgentCondition = AgentCondition.Normal;
            NavmeshAgent.enabled = false;
        }

        //  Unity Methods   -------------------------------

        
        //  Other Methods ---------------------------------
        public abstract void Assign();

        private void AssignControllers()
        {
            _memoryController = new AiMemoryController
            {
                MemorySpan = AgentMemorySpan
            };
            _attributeSystemComponent = GetComponent<AttributeSystemComponent>();
            
            this.AttachControllerToModel(out _aiAbilityController);
            this.AttachControllerToModel(out _agentAnimatorController);
            this.AttachControllerToModel(out _visionController);
            this.AttachControllerToModel(out _impactSensorController);
            this.AttachControllerToModel(out _motorController);
            this.AttachControllerToModel(out _targetingController);
            this.AttachControllerToModel(out _weaponVisualsController);
            this.AttachControllerToModel(out _impactReactionController);
        }
        
        #region Behaviour
        // Shared
        public virtual void SetForwardDirection() => MotorController.SetForwardDirection();
        public virtual void ApplyDirection() => MotorController.ApplyDirection();
        public virtual void SetEnableNavMeshAgent(bool value) => NavmeshAgent.enabled = value;
        public virtual void SetEnableAgentCollider(bool value) => AgentCollider.enabled = value;
        
        // Movement Behaviour
        public virtual void ApplyStop() => MotorController.LocomotionController.ApplyStop();
        public virtual void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public virtual void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        public virtual void Warp() => NavmeshAgent.Warp(transform.position);

        // Roll  Behaviour 
        public virtual void BeginRoll() => MotorController.RollController.BeginRoll();
        public virtual void Rolling() => MotorController.RollController.Rolling();
        public virtual void EndRoll() => MotorController.RollController.EndRoll();
        public virtual void SetWeaponVisible(bool value) => MotorController.CombatController.CurrentWeapon.gameObject.SetActive(value);
        public virtual void SetActiveImpactSensor(bool value) => _impactSensorController.SetActiveSensor(value);

        public virtual void ChangeVisionDistance() => VisionController.VisionDistance = MotorController.CombatController.CurrentWeapon.range;
        
        //  Attack Behaviour
        public virtual void AimTarget() => MotorController.CombatController.AimTarget();
        public virtual void BeginPrepareAttack() => MotorController.CombatController.BeginPrepareAttack();
        public virtual void PreparingAttack() => MotorController.CombatController.PreparingAttack();
        public virtual void EndPrepareAttack() => MotorController.CombatController.EndPrepareAttack();
        public virtual void BeginAttack() => MotorController.CombatController.BeginAttack();
        public virtual void Attacking() => MotorController.CombatController.Attacking();
        public virtual void EndAttack() => MotorController.CombatController.EndAttack();

        public virtual void BeginAttackMove() => MotorController.CombatController.BeginAttackMove();
        public virtual void AttackMoving() => MotorController.CombatController.AttackMoving();
        public virtual void EndAttackMove() => MotorController.CombatController.EndAttackMove();

        public virtual void ResetAttackState()
        {
            CurrentActionState &= ~CharacterActionType.BeginPrepareAttack;
            CurrentActionState &= ~CharacterActionType.BeginAttackMove;
            CurrentActionState &= ~CharacterActionType.BeginAttack;

            CurrentActionState |= CharacterActionType.EndPrepareAttack;
            CurrentActionState |= CharacterActionType.EndAttackMove;
            CurrentActionState |= CharacterActionType.EndAttack;
            CurrentActionState |= CharacterActionType.MoveNextSkill;

        }
        
        public virtual void CustomActionAttack() => MotorController.CombatController.CustomAction();
        public virtual void RemoveAllReaction()
        {
            AgentCondition = AgentCondition.Normal;
            ImpactHit = Vector3.zero;
        }
        
        // Swap weapon
        public virtual void ActivateOtherWeapon() => WeaponVisualsController.ActivateOtherWeapon();
        public virtual void SwitchCombatMode()
        {
            MotorController.SwitchCombatMode(CurrentCombatMode);
            MotorController.CombatController.CurrentWeapon = CurrentWeapon;
        }

        #endregion
        
        //  Event Handlers --------------------------------
        protected virtual void RegisterActionTrigger(CharacterActionType actionType, Action action)
        {
            if (!ActionTriggers.TryAdd(actionType, action))
            {
                ActionTriggers[actionType] += action;
            }
        }

        public virtual void OnCharacterActionTrigger(CharacterActionType actionType)
        {
            if (ActionTriggers.TryGetValue(actionType, out var trigger))
            {
                trigger.Invoke();
            }
        }
    }
}

