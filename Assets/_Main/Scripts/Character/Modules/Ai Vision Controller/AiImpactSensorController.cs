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
        public event Action<Vector3> OnImpact; 

        //  Properties ------------------------------------
        public Vector3 ImpactDirection { get; set; }
        public float ImpactForce { get; set; }
        public BodyPartType ImpactPart { get; set; }
        
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }

        //  Fields ----------------------------------------
        private AiBodyPart[] _bodyParts;
        [SerializeField]
        private ImpactConfig _config;
        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
                _bodyParts = Model.BodyParts;
                
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

        public void TurnOnAllHitBox()
        {
            foreach (var bodyPart in _bodyParts)
            {
                bodyPart.enabled = true;
            }
        }

        public void TurnOffAllHitBox()
        {
            foreach (var bodyPart in _bodyParts)
            {
                bodyPart.enabled = false;
            }
        }

        //  Event Handlers --------------------------------
        public void Impact()
        {
            OnImpact?.Invoke(ImpactDirection * ImpactForce);
        }
        
        
    }
    
}