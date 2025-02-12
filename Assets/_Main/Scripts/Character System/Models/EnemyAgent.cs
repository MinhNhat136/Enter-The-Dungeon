using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Atomic.Character
{
    public class EnemyAgent : BaseAgent 
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public bool CanPerformAttack => Math.Abs(StabilityRatio - 1) < 0.01f &&
                                        TargetAgent != null &&
                                        Vector3.Distance(transform.position, TargetAgent.transform.position) <= MotorController.CombatController.CombatRange &&
                                        CurrentActionState.HasFlag(CharacterActionType.EndAttack) &&
                                        CurrentActionState.HasFlag(CharacterActionType.EndPrepareAttack) &&
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
                
                SwitchCombatMode();
                Assign();
            }
        }

        //  Unity Methods   -------------------------------
        

        private void Start()
        {
            _playerObject = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = _playerObject.transform;
            Initialize();
        }
        
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------

        public override void Assign()
        {
            AssignCharacterActionEvents();
        }

        private new void AssignCharacterActionEvents()
        {
            base.AssignCharacterActionEvents();
            #region Attack Move
            RegisterActionTrigger(CharacterActionType.BeginPrepareAttack, () =>
            {
                CurrentActionState |= CharacterActionType.BeginPrepareAttack;
                CurrentActionState &= ~CharacterActionType.EndPrepareAttack;
            });
            RegisterActionTrigger(CharacterActionType.EndPrepareAttack, () =>
            {
                CurrentActionState &= ~CharacterActionType.BeginPrepareAttack;
                CurrentActionState |= CharacterActionType.EndPrepareAttack;
            });
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
        
        protected float startMoveTimeAfterAttack;
        
        public void CalculateMoveDirectionFromArc(float angle)
        {
            var isNegative = Random.value > 0.5f; 
            angle = isNegative ? -angle : angle;
            var rotation = Quaternion.AngleAxis(angle, Vector3.up);
            var directionToPlayer = (_playerTransform.position - transform.position);
            var rotatedVector = rotation * directionToPlayer;
            startMoveTimeAfterAttack = Time.time;
            MotorController.MoveInput = new Vector2(rotatedVector.x, rotatedVector.z);
        }
        
        public void CalculateDirectionToPlayer()
        {
            var directionToPlayer = (_playerTransform.position - transform.position);
            var moveInput = new Vector2(directionToPlayer.x, directionToPlayer.z);
            MotorController.MoveInput = moveInput;
        }
        
        public bool IsReachMaxTimeMoveAfterAttack(float value)
        {
            return (Time.time - startMoveTimeAfterAttack) > value;
        }

        public bool IsForwardTarget()
        {
            return Physics.Raycast(modelTransform.position + new Vector3(0, 1, 0), modelTransform.forward, 10,
                TargetAgent.gameObject.layer);
        }
    }
    
}
