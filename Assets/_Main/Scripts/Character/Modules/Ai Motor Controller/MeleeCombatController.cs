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
                    NextAnimationName = _meleeWeapon.AttackDatas[0].AnimationName;
                }
                else
                {
                    Debug.Log(_meleeWeapon);
                    throw new System.Exception("Ranged weapon invalid");
                }
            }
        }

        public bool IsInitialized { get; set; }
        public BaseAgent Model { get; set; }
        
        [field: SerializeField]
        public LayerMask ColliderObstacleLayer { get; private set; }
        
        [field: SerializeField]
        public string NextAnimationName{ get; private set; }

        //  Fields ----------------------------------------
        private int _currentCombo = 0;
        private RaycastHit _rayCastHit;
        private NavMeshHit _navMeshHit;
        private MeleeWeaponScriptableObject _meleeWeapon;

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
            Vector3 targetPosition = SetDestinationForAttackMove();
        }
        
        public void BeginAttackMove()
        {
            _meleeWeapon.BeginAttackMove();
            
        }

        public void EndAttackMove()
        {
            _meleeWeapon.EndAttackMove();
        }

        public void Attacking()
        {
        }

        public void EndAttack()
        {
            _meleeWeapon.EndAttack();
            Invoke(nameof(ResetCombo), _meleeWeapon.AttackDatas[_currentCombo].DelayResetCombo);
            if (_currentCombo < _meleeWeapon.AttackDatas.Count - 1)
            {
                _currentCombo++;
                NextAnimationName = _meleeWeapon.AttackDatas[_currentCombo].AnimationName;
            }
            else ResetCombo();
        }
        
        public void InterruptAttackMove()
        {
            
        }

        public void ResetCombo()
        {
            _currentCombo = 0;
            NextAnimationName = _meleeWeapon.AttackDatas[_currentCombo].AnimationName;
        }
        
        private Vector3 SetDestinationForAttackMove()
        {
            Vector3 attackMoveDirection = Model.modelTransform.forward;
            Vector3 targetPosition = Model.modelTransform.position + attackMoveDirection * _meleeWeapon.AttackDatas[_currentCombo].AttackMoveDistance;

            if (Physics.Raycast(Model.modelTransform.position, attackMoveDirection, out _rayCastHit,
                    _meleeWeapon.AttackDatas[_currentCombo].AttackMoveDistance, ColliderObstacleLayer))
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