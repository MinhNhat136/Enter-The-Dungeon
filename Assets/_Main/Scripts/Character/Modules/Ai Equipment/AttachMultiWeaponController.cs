using System;
using Atomic.Character.Module;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controls the attachment and detachment of weapons.
    /// ToDo: ReWrite this code for multi weapon (knife)
    /// </summary>
    public class AttachMultiWeaponController : MonoBehaviour, IAttachWeaponController
    {
        //  Events ----------------------------------------
        private event Action _onAttach;
        private event Action _onDetach;
        private event Action<CombatMode> _onActivate;

        //  Properties ------------------------------------
        public event Action OnAttach
        {
            add => _onAttach += value;
            remove => _onAttach -= value;
        }

        public event Action OnDetach
        {
            add => _onDetach += value;
            remove => _onDetach -= value;
        }
        
        public event Action<CombatMode> OnActivate
        {
            add => _onActivate += value;
            remove => _onActivate -= value;
        }

        public AttachWeaponType WeaponType => weaponType;
        
        [field: SerializeField]
        public bool IsAttach { get; set; } = false; 
        
        [field: SerializeField]
        public bool IsActivated { get; set; } = false;
        
        public bool IsInitialized { get; set; }
        
        //  Fields ----------------------------------------
        [Header("DATA")]
        [SerializeField] private AttachWeaponType weaponType;
        [SerializeReference] private Weapon weaponPrefab;
        [SerializeField] private Transform transformParent;
        [SerializeField] private CombatMode combatMode;
        [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;
        [SerializeField] private AttachPoint attachPoint;
        
        private Animator animator;
        private Weapon _weapon;
        
        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                animator = GetComponentInParent<Animator>();
                
                transformParent.gameObject.SetActive(false);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception($"{typeof(AttachWeaponController)} of {this.weaponType} not initialized yet");
            }
        }
        
        //  Other Methods ---------------------------------
        public void Attach()
        {
            if (!IsAttach)
            {
                IsAttach = true;
                _weapon = Instantiate(weaponPrefab, transformParent, true);
                _weapon.transform.localPosition = attachPoint.Position;
                _weapon.transform.localRotation = attachPoint.Rotation;
                animator.runtimeAnimatorController = runtimeAnimatorController;
                _onAttach?.Invoke();
            }
        }

        public void Detach()
        {
            if (_weapon && IsAttach)
            {
                IsActivated = false;
                Activate(false);
                Destroy(_weapon.gameObject);
                _weapon = null;
                _onDetach?.Invoke();
            }
        }

        public void Activate(bool value)
        {
            IsActivated = value;
            transformParent.gameObject.SetActive(value);
            animator.runtimeAnimatorController = value ? runtimeAnimatorController : null;
        }

        //  Event Handlers --------------------------------
        
    }
}
