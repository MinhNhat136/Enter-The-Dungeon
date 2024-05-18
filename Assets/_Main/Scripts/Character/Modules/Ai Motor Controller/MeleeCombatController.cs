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
                    _meleeWeapon = meleeWeapon;
                else
                {
                    Debug.Log(_meleeWeapon);
                    throw new System.Exception("Ranged weapon invalid");
                }
            }
        }


        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }
        public float currentCombo = 1;

        private float _defaultSpeed;
        private float _defaultAcceleration;

        private RaycastHit _rayCastHit;
        private NavMeshHit _navMeshHit;

        //  Fields ----------------------------------------
        private MeleeWeaponScriptableObject _meleeWeapon;

        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;

                _defaultSpeed = Model.NavmeshAgent.speed;
                _defaultAcceleration = Model.NavmeshAgent.acceleration;
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
        public void RegisterWeapon()
        {
            throw new System.NotImplementedException();
        }

        public void AimTarget()
        {
        }

        public void BeginAttackMove()
        {
            if (_meleeWeapon)
            {
                _meleeWeapon.BeginAttackMove();
                Vector3 targetPosition = SetDestinationForAttackMove();
                Model.NavmeshAgent.SetDestination(targetPosition);
                Model.NavmeshAgent.speed *= _meleeWeapon.attackMoveSpeedWeight;
                Model.NavmeshAgent.acceleration *= _meleeWeapon.attackMoveAccelerationWeight;
            }
            
        }

        public void AttackMoving()
        {
        }

        public void EndAttackMove()
        {
            if (_meleeWeapon)
            {
                _meleeWeapon.EndAttackMove();
                Model.NavmeshAgent.speed /= _meleeWeapon.attackMoveSpeedWeight;
                Model.NavmeshAgent.acceleration /= _meleeWeapon.attackMoveAccelerationWeight;
            }
        }

        public void InterruptAttackMove()
        {
            Model.NavmeshAgent.speed = _defaultSpeed;
            Model.NavmeshAgent.acceleration = _defaultAcceleration;
            Model.NavmeshAgent.SetDestination(Model.transform.position);
        }

        public void BeginAttack()
        {
            if (_meleeWeapon)
            {
                if (currentCombo < _meleeWeapon.combo) currentCombo++;
                else currentCombo = 1;
                CancelInvoke(nameof(ResetCombo));
            }
        }

        public void Attacking()
        {
        }

        public void EndAttack()
        {
            if (_meleeWeapon)
            {
                _meleeWeapon.EndAttack();
                Invoke(nameof(ResetCombo), _meleeWeapon.delayResetCombo);
            }
        }

        public void CustomAction()
        {
        }

        public void InterruptAction()
        {
        }

        public void ResetCombo()
        {
            currentCombo = 1;
        }

        public LayerMask colliderObstacleLayer;


        private Vector3 SetDestinationForAttackMove()
        {
            Vector3 attackMoveDirection = Model.transform.forward;
            Vector3 targetPosition = Model.transform.position + attackMoveDirection * _meleeWeapon.attackMoveDistance;

            if (Physics.Raycast(Model.transform.position, attackMoveDirection, out _rayCastHit,
                    _meleeWeapon.attackMoveDistance, colliderObstacleLayer))
            {
                if (NavMesh.SamplePosition(_rayCastHit.point, out _navMeshHit, 10, NavMesh.AllAreas))
                {
                    return _navMeshHit.position;
                }
            }

            return targetPosition;
        }

        //  Event Handlers --------------------------------
    }
}