using System;
using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class MeleeWeaponObject : MonoBehaviour
    {
        //  Events ----------------------------------------
        public event Action<Collider> OnHitObject;


        //  Properties ------------------------------------
        public Collider Collider { get; private set; }


        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------
        public void Awake()
        {
            Collider = GetComponent<Collider>();
            Collider.enabled = false;
        }


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        private void OnTriggerEnter(Collider other)
        {
            OnHitObject?.Invoke(other);
        }
    }
    
}
