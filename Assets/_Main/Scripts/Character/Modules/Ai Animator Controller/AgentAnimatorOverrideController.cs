using System.Collections.Generic;
using Atomic.Core.Interface;
using Atomic.Equipment;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AgentAnimatorOverrideController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------
        

        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; } = false; 
        public BaseAgent Model { get; private set; }

        
       //  Collections -----------------------------------
       public Dictionary<AttachWeaponType, RuntimeAnimatorController> AnimatorOverriders { get; } = new();
       
       //  Fields ----------------------------------------
       [SerializeField] 
       private Animator animator; 
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;

                Model = model;
                animator = GetComponentInChildren<Animator>();
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------

        //  Event Handlers --------------------------------

    }
    
}
