using System.Collections;
using Atomic.Equipment;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controller for melee combat actions.
    /// </summary>
    public class MeleeCombatController : MonoBehaviour, ICombatController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public WeaponScriptableObject CurrentWeapon
        {
            get => _meleeWeapon;
            set
            {
                if (value is MeleeWeaponScriptableObject meleeWeapon)
                {
                    _meleeWeapon = meleeWeapon;
                    NextAnimationName = _meleeWeapon.attackData[0].AnimationName;
                }
                else
                {
                    Debug.Log(_meleeWeapon);
                    throw new System.Exception("Melee weapon invalid");
                }
            }
        }

        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }

        [field: SerializeField] public LayerMask ColliderObstacleLayer { get; private set; }

        [field: SerializeField] public string NextAnimationName { get; private set; }

        //  Fields ----------------------------------------
        private int _currentCombo = 0;
        private RaycastHit _rayCastHit;
        private NavMeshHit _navMeshHit;
        private MeleeWeaponScriptableObject _meleeWeapon;

        private Vector3 _attackMoveDestination;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("MeleeCombatController not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void AimTarget()
        {
        }

        public void BeginAttack()
        {
            CancelInvoke(nameof(ResetCombo));
        }

        public void BeginAttackMove()
        {
            Model.NavmeshAgent.enabled = false;
            _attackMoveDestination = FindDestination();
            _meleeWeapon.BeginAttackMove();
        }

        public void AttackMoving()
        {
            Model.modelTransform.position = Vector3.Lerp(Model.modelTransform.position, _attackMoveDestination,
                _meleeWeapon.attackData[_currentCombo].AttackMoveSpeed * Time.deltaTime);
        }

        public void EndAttackMove()
        {
            Model.NavmeshAgent.Warp(Model.modelTransform.position);
            Model.NavmeshAgent.enabled = true;
            _meleeWeapon.EndAttackMove();
        }
        
        public void EndAttack()
        {
            _meleeWeapon.CurrentCombo = _currentCombo;
            _meleeWeapon.EndAttack();
            Invoke(nameof(ResetCombo), _meleeWeapon.attackData[_currentCombo].DelayResetCombo);
            if (_currentCombo < _meleeWeapon.attackData.Count - 1)
            {
                _currentCombo++;
                NextAnimationName = _meleeWeapon.attackData[_currentCombo].AnimationName;
            }
            else ResetCombo();
        }

        public void InterruptAttackMove()
        {
        }

        public void ResetCombo()
        {
            _currentCombo = 0;
            NextAnimationName = _meleeWeapon.attackData[_currentCombo].AnimationName;
        }

        private Vector3 FindDestination()
        {
            Vector3 attackMoveDirection = Model.modelTransform.forward;
            var destination = Model.modelTransform.position +
                              _meleeWeapon.attackData[_currentCombo].AttackDistance * attackMoveDirection;
            if (Physics.Raycast(Model.modelTransform.position, attackMoveDirection, out _rayCastHit,
                    _meleeWeapon.attackData[_currentCombo].AttackDistance, ColliderObstacleLayer))
            {
                if (NavMesh.SamplePosition(_rayCastHit.point, 
                        out _navMeshHit, 5f, NavMesh.AllAreas))
                {
                    return _navMeshHit.position;
                }
            }
            if (NavMesh.SamplePosition(destination, out _navMeshHit, 2f, NavMesh.AllAreas))
            {
                return destination;
            }
            return Model.modelTransform.position;
        }


        //  Event Handlers --------------------------------
    }
}