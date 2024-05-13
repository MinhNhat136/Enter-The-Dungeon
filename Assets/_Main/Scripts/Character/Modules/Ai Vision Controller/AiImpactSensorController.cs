using System;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiImpactSensorController : ISensorController
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
            throw new NotImplementedException();
        }
        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void Scan()
        {
        }

        public int Filter(GameObject[] buffer, string layerName)
        {
            return 0;
        }

        //  Event Handlers --------------------------------
        
        
        

        
    }
}