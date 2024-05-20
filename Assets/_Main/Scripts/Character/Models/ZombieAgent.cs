using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------


    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class ZombieAgent : BaseAgent
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool CanPerformAttack => AgentCondition.HasFlag(AgentCondition.Normal) &&
                                        CurrentActionState.HasFlag(CharacterActionType.EndAttack) &&
                                        CurrentActionState.HasFlag(CharacterActionType.EndAttackMove);

        //  Fields ----------------------------------------

        private Transform _playerTransform;
        private GameObject _playerObject;

        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                base.Initialize();
                IsPlayer = false;
                MotorController.CombatController = GetComponent<ICombatController>();
                
                WeaponVisualsController.AttachDefaultWeapons();
                SwitchCombatMode();
                Assign();
            }
        }

        //  Unity Methods   -------------------------------
        public void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            _playerObject = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = _playerObject.transform;
        }

        public void OnEnable()
        {
            DoEnable();
        }

        public void OnDisable()
        {
            DoDisable();
        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------

        public override void Assign()
        {
            AssignCharacterActionEvents();
        }

        private void AssignCharacterActionEvents()
        {
            #region Attack Move

            RegisterActionTrigger(CharacterActionType.BeginAttackMove, () =>
            {
                CurrentActionState |= CharacterActionType.BeginAttackMove;
                CurrentActionState &= ~CharacterActionType.EndAttackMove;
                CurrentActionState &= ~CharacterActionType.MoveNextSkill;
            });
            RegisterActionTrigger(CharacterActionType.EndAttackMove, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginAttackMove;
                CurrentActionState |= CharacterActionType.EndAttackMove;
            });

            #endregion

            #region Attack

            RegisterActionTrigger(CharacterActionType.BeginAttack, () =>
            {
                CurrentActionState |= CharacterActionType.BeginAttack;
                CurrentActionState &= ~CharacterActionType.EndAttack;
            });
            RegisterActionTrigger(CharacterActionType.EndAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginAttack;
                CurrentActionState |= CharacterActionType.EndAttack;
            });

            #endregion

            #region Move next skill

            RegisterActionTrigger(CharacterActionType.MoveNextSkill,
                () => { CurrentActionState |= CharacterActionType.MoveNextSkill; });

            #endregion
        }
        
        public void ResetState() => CurrentActionState = DefaultActionState;
        
        public void CalculateDirectionToPlayer()
        {
            var directionToPlayer = (_playerTransform.position - transform.position);
            var moveInput = new Vector2(directionToPlayer.x, directionToPlayer.z);
            MotorController.MoveInput = moveInput;
        }

        public void GoAroundTarget()
        {
            var rotationAngle = 60; 
            var rotationAxis = Vector3.up; 

            var rotationQuaternion = Quaternion.AngleAxis(rotationAngle, rotationAxis);
            var newForwardVector = rotationQuaternion * transform.forward;

            var newDestination = modelTransform.position + newForwardVector * 5f;
            Debug.Log(newDestination);
            NavmeshAgent.SetDestination(newDestination);
        }

        public bool IsReachingDestination()
        {
            return Vector3.Distance(NavmeshAgent.destination, transform.position) <= 0.1f;
        }
    }
}