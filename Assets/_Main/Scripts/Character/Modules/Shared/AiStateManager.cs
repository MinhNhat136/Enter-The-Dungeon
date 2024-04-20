using System;
using System.Collections.Generic;
using Atomic.Character.Model;
using Atomic.Core.Interface;
using ParadoxNotion.Serialization.FullSerializer;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
   
    
    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiStateManager : SerializedMonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }

        
        //  Fields ----------------------------------------

        
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
            throw new System.NotImplementedException();
        }

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------

        

       
        
        //  Event Handlers --------------------------------
        
    }
    
}
