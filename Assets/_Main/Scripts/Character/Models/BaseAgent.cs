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

    public enum AgentCondition
    {
        Normal, 
        Hit,
        Stun,
        Break,
        Knockdown, 
    }
    
    /// <summary>
    /// Base class for defining characters with modular control systems.
    /// </summary>
    [RequireComponent(
        typeof(NavMeshAgent), 
        typeof(AiHealth), 
        typeof(AiPassiveStateController))]
    [RequireComponent(
        typeof(AiImpactSensorController),
        typeof(AiVisionSensorController),
        typeof(AiImpactReactionController))]
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
        public bool IsMovement => !Mathf.Approximately(MotorController.MoveInput.sqrMagnitude, 0f);
        
        protected virtual CharacterActionType DefaultActionState { get; set; }
        private Dictionary<CharacterActionType, Action> ActionTriggers { get; } = new();
        public AiBodyPart[] BodyParts { get; set; } 
        public Vector3 ForceHit { get; set; }

        [field: SerializeField]
        public CharacterActionType CurrentActionState { get; set; }
        [field: SerializeField]
        public Command Command { get; set; }

        public AgentCondition AgentCondition { get; set; } = AgentCondition.Normal;

        #endregion

        #region Module Controllers

        public NavMeshAgent NavmeshAgent { get; private set; }
        protected AgentAnimator AgentAnimatorController => _agentAnimatorController;
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
        public AiPassiveStateController PassiveStateController => _passiveStateController;
        public AiHealth HealthController => _healthController;

        #endregion
        
        //  Collections -----------------------------------


        //  Fields ----------------------------------------
        [SerializeField] 
        public AiConfig config; 
        
        [SerializeField] 
        private Transform bodyWeakPoint;
        private bool _isInitialized;

        #region Controller
        private AgentAnimator _agentAnimatorController;
        private AiVisionSensorController _visionController;
        private AiImpactSensorController _impactSensorController;
        private ITargetingController _targetingController;
        private AiMotorController _motorController;
        private AiWeaponVisualsController _weaponVisualsController;
        private AiPassiveStateController _passiveStateController;
        private AiImpactReactionController _impactReactionController;
        
        private AiHealth _healthController;
        private AiMemoryController _memoryController;

        
        #endregion
        
        //  Initialization  -------------------------------
        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                NavmeshAgent = GetComponent<NavMeshAgent>();

                DefaultActionState |= CharacterActionType.EndAttack | CharacterActionType.EndAttackMove |
                                      CharacterActionType.EndPrepareAttack | CharacterActionType.EndRoll |
                                      CharacterActionType.MoveNextSkill;

                CurrentActionState = DefaultActionState;
                Command |= Command.Move;

                BodyParts =  GetComponentsInChildren<AiBodyPart>();
                
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
            this.AttachControllerToModel(out _impactSensorController);
            this.AttachControllerToModel(out _motorController);
            this.AttachControllerToModel(out _targetingController);
            this.AttachControllerToModel(out _weaponVisualsController);
            this.AttachControllerToModel(out _passiveStateController);
            this.AttachControllerToModel(out _impactReactionController);
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

        #region Behaviour
        // Shared
        public void SetForwardDirection() => MotorController.SetForwardDirection();
        public void ApplyDirection() => MotorController.ApplyDirection();
        
        // Movement Behaviour
        public void ApplyStop() => MotorController.LocomotionController.ApplyStop();
        public void ApplyMovement() => MotorController.LocomotionController.ApplyMovement();
        public void ApplyRotation() => MotorController.LocomotionController.ApplyRotation();
        
        // Roll Behaviour 
        public void ApplyRoll() => MotorController.RollController.Roll();
        
        /// <summary>
        /// TODO: Fix this
        /// </summary>
        /// <param name="value"></param>
        public void SetWeaponVisible(bool value) {
            
            // MotorController.CombatController.CurrentWeapon.WeaponRoot.SetActive(value);
        }

        public void ChangeVisionDistance() => VisionController.VisionDistance = MotorController.CombatController.CurrentWeapon.range;
        
        // Attack Behaviour
        public void AimTarget() => MotorController.CombatController.AimTarget();
        public void BeginPrepareAttack() => MotorController.CombatController.BeginPrepareAttack();
        public void PreparingAttack() => MotorController.CombatController.PreparingAttack();
        public void EndPrepareAttack() => MotorController.CombatController.EndPrepareAttack();
        public void InterruptPrepareAttack() => MotorController.CombatController.InterruptPrepareAttack();

        public void BeginAttack() => MotorController.CombatController.BeginAttack();
        public void Attacking() => MotorController.CombatController.Attacking();
        public void EndAttack() => MotorController.CombatController.EndAttack();
        public void InterruptAttack()
        {
            CurrentActionState &= ~CharacterActionType.BeginAttack; 
            CurrentActionState |= CharacterActionType.EndAttack;
            MotorController.CombatController.InterruptAttack();
        }

        public void BeginAttackMove() => MotorController.CombatController.BeginAttackMove();
        public void AttackMoving() => MotorController.CombatController.Attacking();
        public void EndAttackMove() => MotorController.CombatController.EndAttackMove();
        public void InterruptAttackMove()
        {
            CurrentActionState &= ~CharacterActionType.BeginAttackMove; 
            CurrentActionState |= CharacterActionType.EndAttackMove;
            MotorController.CombatController.InterruptAttackMove();
        }

        public void CustomActionAttack() => MotorController.CombatController.CustomAction();


        // Swap weapon
        public void ActivateOtherWeapon() => WeaponVisualsController.ActivateOtherWeapon();

        public void SwitchCombatMode()
        {
            MotorController.SwitchCombatMode(CurrentCombatMode);
            MotorController.CombatController.CurrentWeapon = CurrentWeapon;
        }


        // Hit Reaction Behaviour
        

        #endregion
    }
}

