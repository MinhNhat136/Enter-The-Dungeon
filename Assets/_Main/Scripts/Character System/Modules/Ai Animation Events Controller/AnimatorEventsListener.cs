using System;
using System.Linq;
using UnityEngine;

namespace  Atomic.Character
{

    //  Namespace Properties ------------------------------
    

    
    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AnimatorEventsListener : MonoBehaviour
    {
        //  Events ----------------------------------------
        private event Action<CharacterActionType> _onCharacterActionEvent;
        private event Action<ActionEffectType> _onActionEffectEvent;

        //  Properties ------------------------------------
        public event Action<CharacterActionType> OnCharacterActionEvent
        {
            add => _onCharacterActionEvent += value;
            remove => _onCharacterActionEvent -= value; 
        }
        
        public event Action<ActionEffectType> OnActionEffectEvent
        {
            add => _onActionEffectEvent += value;
            remove => _onActionEffectEvent -= value; 
        }
        
        
        //  Fields ----------------------------------------
        

        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------
        public void Awake()
        {
            RegisterCharacterActionTriggers();
            RegisterActionEffectTriggers();
        }


        private void RegisterActionEffectTriggers()
        {
            IActionEffectTrigger[] effectTriggers = GetComponentsInParent<IActionEffectTrigger>();
            if (!effectTriggers.Any()) return; 
            foreach (var receiver in effectTriggers)
            {
                OnActionEffectEvent += (effectType) =>
                {
                    if (receiver.enabled)
                    {
                        receiver.OnActionEffectTrigger(effectType);
                    }
                };
            }
        }
        
        private void RegisterCharacterActionTriggers()
        {
            ICharacterActionTrigger[] actionTriggers = GetComponentsInParent<ICharacterActionTrigger>();
            if (!actionTriggers.Any()) return; 
            foreach (var receiver in actionTriggers)
            {
                OnCharacterActionEvent += (actionType) =>
                {
                    if (receiver.enabled)
                    {
                        receiver.OnCharacterActionTrigger(actionType);
                    }
                };
            }
        }

        //  Other Methods ---------------------------------
        public void TriggerCharacterAction(string actionType)
        {
            _onCharacterActionEvent?.Invoke((CharacterActionType)Enum.Parse(typeof(CharacterActionType), actionType));    
        }

        public void TriggerActionEffect(string effectType)
        {
            _onActionEffectEvent?.Invoke((ActionEffectType)Enum.Parse(typeof(ActionEffectType), effectType));
        }

        //  Event Handlers --------------------------------
        
        
    }
}
