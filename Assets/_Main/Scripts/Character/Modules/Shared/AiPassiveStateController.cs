using System;
using System.Collections.Generic;
using System.Linq;
using Atomic.Core.Interface;
using Atomic.Damage;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiPassiveStateController : MonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }

        [field: SerializeField] 
        public StatusEffectType Effects { get; set; }
        public StatusEffectType CurrentEffects { get; set; }

        //  Fields ----------------------------------------
        public readonly List<PassiveEffect> EffectHandlers = new(8);
        
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

        //  Other Methods -------------------------------
        public void Update()
        {
            HandleEffect();
        }

        public void HandleEffect()
        {
            if (EffectHandlers.Count == 0) return;
            foreach (var handler in EffectHandlers.ToList())
            {
                handler.Handle();
            }
        }
        //  Event Handlers --------------------------------
    }
}