using System;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    
    
    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiImpactSensorController : MonoBehaviour, ISensorController
    {
        
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public BodyPartType ImpactPart { get; set; }
        
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }

        //  Fields ----------------------------------------
        private AiBodyPart[] _bodyParts;
        [SerializeField]
        private ImpactConfig _config;
        

        private Collider _collider;
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
                _bodyParts = Model.BodyParts;
                _collider = GetComponent<Collider>();
                foreach (var aiBodyPart in _bodyParts)
                {
                    aiBodyPart.SetLayer(_config.damageLayerIndex);
                }
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

        public void SetActiveSensor(bool value)
        {
            foreach (var bodyPart in _bodyParts)
            {
                if (value)
                {
                    bodyPart.TurnOnHitBox();
                }
                else
                {
                    bodyPart.TurnOffHitBox();
                }
            }
        }

        public void SetBodyCollide(bool value)
        {
            _collider.isTrigger = !value; 
        }

        //  Event Handlers --------------------------------
        
    }
    
}