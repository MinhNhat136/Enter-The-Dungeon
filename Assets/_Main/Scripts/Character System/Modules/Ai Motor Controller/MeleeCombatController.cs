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
        public WeaponBuilder CurrentWeapon
        {
            get => _meleeWeapon;
            set
            {
                if (value is MeleeWeaponBuilder meleeWeapon)
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
        
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }
        public string NextAnimationName { get; set; }


        [field: SerializeField] public LayerMask ColliderObstacleLayer { get; private set; }


        //  Fields ----------------------------------------
        private MeleeWeaponBuilder _meleeWeapon;
        
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
            _meleeWeapon.BeginAttack();
        }

        private bool _stopMove; 

        public void BeginAttackMove()
        {
            _stopMove = false; 
            Model.NavmeshAgent.enabled = false;
        }

        public void AttackMoving()
        {
            if (_stopMove)
                return;
            var distanceMove = _meleeWeapon.attackData[_meleeWeapon.CurrentCombo].AttackMoveSpeed * Model.SpeedRatio * Time.deltaTime;
            if (Physics.Raycast(Model.modelTransform.position + new Vector3(0, 1, 0), Model.modelTransform.forward, out _, 
                    Model.Width + distanceMove, ColliderObstacleLayer))
            {
                    _stopMove = true;
                    return;
            }
            
            if (NavMesh.SamplePosition(Model.modelTransform.position + distanceMove * Model.modelTransform.forward
                    , out _,
                    1, NavMesh.AllAreas))
            {
                Model.modelTransform.position += distanceMove * Model.modelTransform.forward;
            }
        }

        public void EndAttackMove()
        {
            Model.NavmeshAgent.Warp(Model.modelTransform.position);
            Model.NavmeshAgent.enabled = true;
        }

        public void BeginHit()
        {
            _meleeWeapon.BeginHit();
        }

        public void EndHit()
        {
            _meleeWeapon.EndHit();
        }
        
        public void EndAttack()
        {
            Model.LastAttackTime = Time.time;
            _meleeWeapon.EndAttack();
            NextAnimationName = _meleeWeapon.attackData[_meleeWeapon.CurrentCombo].AnimationName;
        }
        
        //  Event Handlers --------------------------------
    }
}