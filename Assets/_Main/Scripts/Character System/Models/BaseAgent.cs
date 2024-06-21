using System;
using System.Collections.Generic;
using Atomic.AbilitySystem;
using Atomic.Core;
using Atomic.Equipment;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

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
        typeof(AiVisionSensorController))]
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
        
        [field: SerializeField]
        public CharacterActionType CurrentActionState { get; set; }
        
        public CombatMode CurrentCombatMode { get; set; }
        public SurvivalState SurvivalState { get; set; } = SurvivalState.Revive;
        
        private Collider AgentCollider { get; set; }
        public AiBodyPart[] BodyParts { get; set; } 
        public float Height { get; private set; }
        public float Width { get; private set; }
        public float SpeedRatio { get; protected set; }
        public float StabilityRatio { get; protected set; }
        public float Health { get; private set; }
        public Vector3 ImpactDirection { get; private set; }

        public bool IsMovement => !Mathf.Approximately(MotorController.MoveInput.sqrMagnitude, 0f);
        
        public float LastAttackTime { get; set; }
        public float LastImpactTime { get; set; }
        public float LastSwapWeaponTime { get; set; }
        public float SinceLastImpactTime => Time.time - LastImpactTime;
        public float SinceLastAttackTime => Time.time - LastAttackTime;
        public float SinceLastSwapWeaponTime => Time.time - LastSwapWeaponTime;
        
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
        private Dictionary<AttributeScriptableObject, Action> ActionOnAttributeChanged = new(16);
        
        //  Collections -----------------------------------
        
        //  Fields ----------------------------------------
        [HideInInspector] 
        public Transform modelTransform;

        [FormerlySerializedAs("stabilityRatio")] [Header("SPEC")]
        public AttributeScriptableObject stabilityAttribute;
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
        private AiMemoryController _memoryController;
        
        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;
            
            DefaultActionState |= CharacterActionType.EndAttack | CharacterActionType.EndAttackMove |
                                  CharacterActionType.EndPrepareAttack | CharacterActionType.EndRoll |
                                  CharacterActionType.MoveNextSkill;
            SurvivalState = SurvivalState.Revive;
            
            NavmeshAgent = GetComponent<NavMeshAgent>();
            AgentCollider = GetComponent<Collider>();
            BodyParts =  GetComponentsInChildren<AiBodyPart>();
            
            modelTransform = transform;
            Height = NavmeshAgent.height;
            Width = NavmeshAgent.radius;
                
            AssignControllers();
            
            AiAbilityController.abilitySystemController.onApplyGameplayEffect += OnGameplayEffectApply;
            
            ActionOnAttributeChanged.Add(speedAttribute, OnSpeedChanged);
            ActionOnAttributeChanged.Add(healthAttribute, OnHealthChanged);
            ActionOnAttributeChanged.Add(stabilityAttribute, OnStabilityChanged);
                
            AttributeSystemComponent.onAttributeChanged += (attribute) =>
            {
                if (ActionOnAttributeChanged.TryGetValue(attribute, out var action))
                {
                    action?.Invoke();
                }
            };

            GetComponent<BehaviourTreeOwner>().enabled = true;

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
            CurrentActionState = DefaultActionState;
            Command |= Command.Move;
        }

        public virtual void DoDisable()
        {
            Command = 0;
            SurvivalState = SurvivalState.Dead;
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
            this.AttachControllerToModel(out _visionController);
            this.AttachControllerToModel(out _impactSensorController);
            this.AttachControllerToModel(out _motorController);
            this.AttachControllerToModel(out _targetingController);
            this.AttachControllerToModel(out _weaponVisualsController);
            this.AttachControllerToModel(out _agentAnimatorController);
        }
        
        #region Behaviour
        // Shared
        public void SetForwardDirection() => MotorController.SetForwardDirection();
        public void ApplyDirection() => MotorController.ApplyDirection();
        public void SetEnableNavMeshAgent(bool value) => NavmeshAgent.enabled = value;
        public void SetEnableAgentCollider(bool value) => AgentCollider.enabled = value;
        public void CalculateDirectionToTarget(BaseAgent targetAgent)
        {
            var directionToTarget = (targetAgent.transform.position - transform.position);
            var moveInput = new Vector2(directionToTarget.x, directionToTarget.z);
            MotorController.MoveInput = moveInput;
        }
        
        // Movement Behaviour
        public void ApplyStop() => MotorController.LocomotionController.ApplyStop();
        public void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        public void Warp() => NavmeshAgent.Warp(transform.position);

        // Roll  Behaviour 
        public void BeginRoll() => MotorController.RollController.BeginRoll();
        public void Rolling() => MotorController.RollController.Rolling();
        public void EndRoll() => MotorController.RollController.EndRoll();
        public void SetWeaponVisible(bool value) => MotorController.CombatController.CurrentWeapon.gameObject.SetActive(value);
        public void SetActiveImpactSensor(bool value) => _impactSensorController.SetActiveSensor(value);
        
        //  Attack Behaviour
        public void AimTarget() => MotorController.CombatController.AimTarget();
        
        public void BeginPrepareAttack() => MotorController.CombatController.BeginPrepareAttack();
        public void PreparingAttack() => MotorController.CombatController.PreparingAttack();
        public void EndPrepareAttack() => MotorController.CombatController.EndPrepareAttack();

        public void BeginAttack() => MotorController.CombatController.BeginAttack();
        public void Attacking() => MotorController.CombatController.Attacking();
        public void EndAttack() => MotorController.CombatController.EndAttack();

        protected void BeginHit() => MotorController.CombatController.BeginHit();
        protected void EndHit() => MotorController.CombatController.EndHit();
        
        public void BeginAttackMove() => MotorController.CombatController.BeginAttackMove();
        public void AttackMoving() => MotorController.CombatController.AttackMoving();
        public void EndAttackMove() => MotorController.CombatController.EndAttackMove();

        public void ResetAttackState()
        {
            CurrentActionState = 0;

            CurrentActionState = DefaultActionState;
            EndHit();
        }
        
        public void CustomActionAttack() => MotorController.CombatController.CustomAction();
        
        
        // Swap weapon
        public void ActivateOtherWeapon()
        {
            WeaponVisualsController.ActivateOtherWeapon();
            LastSwapWeaponTime = Time.time;
        }

        public void IsCurrentWeaponType(WeaponType weaponType)
        {
            WeaponVisualsController.IsCurrentWeaponType(weaponType);
        }

        public void ActivateOtherWeapon(WeaponType weaponType)
        {
            LastSwapWeaponTime = Time.time;
            WeaponVisualsController.ActivateOtherWeapon(weaponType);
        }
        
        public void SwitchCombatMode()
        {
            MotorController.SwitchCombatMode(CurrentCombatMode);
            MotorController.CombatController.CurrentWeapon = CurrentWeapon;
        }

        #endregion
        
        //  Event Handlers --------------------------------
        protected void AssignCharacterActionEvents()
        {
            RegisterActionTrigger(CharacterActionType.BeginHit, BeginHit);
            RegisterActionTrigger(CharacterActionType.EndHit, EndHit);
        }
        
        protected void RegisterActionTrigger(CharacterActionType actionType, Action action)
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

        private void OnGameplayEffectApply(GameplayEffectSpec effectSpec)
        {
            if (!effectSpec.Source)
            {
                ImpactDirection = Vector3.zero;
                return;
            }

            ImpactDirection = transform.position - effectSpec.Source.transform.position ;
        } 

        private void OnSpeedChanged()
        {
            AttributeSystemComponent.GetAttributeValue(speedAttribute, out var speedRatio);
            SpeedRatio = speedRatio.currentValue;
            AgentAnimatorController.Animator.speed = speedRatio.currentValue;

            _motorController.MoveSpeed = Mathf.Lerp(0, _motorController.MaxMoveSpeed, speedRatio.currentValue);
            _motorController.RotationSpeed = Mathf.Lerp(0, _motorController.MaxRotationSpeed, speedRatio.currentValue);
        }

        private void OnStabilityChanged()
        {
            LastImpactTime = Time.time;
            AttributeSystemComponent.GetAttributeValue(stabilityAttribute, out var stabilityRatio);
            StabilityRatio = stabilityRatio.currentValue;
        }

        private void OnHealthChanged()
        {
            AttributeSystemComponent.GetAttributeValue(healthAttribute, out var health);
            Health = health.currentValue;
            if (health.currentValue <= 0) 
                Health = 0;
            
        }
    }
}

