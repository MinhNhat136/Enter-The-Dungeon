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

        //  Properties ------------------------------------
        public event Action<CharacterActionType> OnCharacterActionEvent
        {
            add => _onCharacterActionEvent += value;
            remove => _onCharacterActionEvent -= value; 
        }
        
        
        //  Fields ----------------------------------------
        

        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------
        public void Awake()
        {
            RegisterCharacterActionTriggers();
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

        //  Event Handlers --------------------------------
        
        
    }
}
