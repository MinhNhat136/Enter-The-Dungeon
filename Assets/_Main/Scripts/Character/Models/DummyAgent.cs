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
                ChangeVisionDistance();
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

        private Vector3 _nextDestinationAfterAttack;
        private Vector3 _startMovePositionAfterAttack;
        private float _startMoveTimeAfterAttack;
        
        
        public void CalculateDirectionToCirclingPlayer(float angle)
        {
            var isNegative = Random.value > 0.5f; 
            angle = isNegative ? -angle : angle;
            var rotation = Quaternion.AngleAxis(angle, Vector3.up);
            var directionToPlayer = (_playerTransform.position - transform.position);
            var rotatedVector = rotation * directionToPlayer;
            _nextDestinationAfterAttack = modelTransform.position + rotatedVector.normalized * VisionController.VisionDistance;
            _startMovePositionAfterAttack = modelTransform.position;
            _startMoveTimeAfterAttack = Time.time;
            MotorController.MoveInput = new Vector2(rotatedVector.x, rotatedVector.z);
        }

        public bool IsReachMaxTimeMoveAfterAttack(float value)
        {
            return (Time.time - _startMoveTimeAfterAttack) > value;
        }

        public bool IsForwardTarget()
        {
            return Physics.Raycast(modelTransform.position + new Vector3(0, 1, 0), modelTransform.forward, 10,
                TargetAgent.gameObject.layer);
        }
    }
}