using System;
using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
   
    
    /// <summary>
    /// Controls the attachment and detachment of weapons.
    /// </summary>
    public class AttachWeaponController : MonoBehaviour, IAttachWeaponController
    {
        //  Events ----------------------------------------
        private event Action<CombatMode, Weapon> _onActivate;

        //  Properties ------------------------------------
        public event Action<CombatMode, Weapon> OnActivate
        {
            add => _onActivate += value;
            remove => _onActivate -= value;
        }
        
        [field: SerializeField]
        public bool IsAttach { get; set; }
        
        [field: SerializeField]
        public bool IsActivated { get; set; }
        
        public bool IsInitialized { get; set; }

        public Weapon WeaponPrefab => weaponPrefab;
        
        //  Fields ----------------------------------------
        [Header("DATA")]
        [SerializeReference] private Weapon weaponPrefab;
        [SerializeField] private Transform transformParent;
        [SerializeField] private CombatMode combatMode;
        
        private Weapon _weapon;
        
        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
                transformParent.gameObject.SetActive(false);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception(" not initialized yet");
            }
        }
        
        //  Other Methods ---------------------------------
        public void Attach()
        {
            if (!IsAttach)
            {
                IsAttach = true;
                _weapon = Instantiate(weaponPrefab, transformParent, true);
                _weapon.WeaponRoot = transformParent.gameObject;
                _weapon.transform.localPosition = Vector3.zero;
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
            }
        }

        public void Activate(bool value)
        {
            IsActivated = value;
            transformParent.gameObject.SetActive(value);
            if (!value) return;
            
            _onActivate?.Invoke(combatMode, _weapon);
        }

        //  Event Handlers --------------------------------
        
    }
}