using System;
using System.Linq;
using UnityEngine;

namespace  Atomic.Character.Module
{

    public enum CharacterActionType
    {
        BeginRoll, 
        StopRoll,
        BeginChargeWeapon,
        ChargeFull,
        BeginShoot,
        BeginMeleeAttack,
        EndAttack,
        MoveNextSkill,
        BeginTrack, 
        StopTrack,
    }    
    public class AnimatorEventsListener : MonoBehaviour
    {
        private event Action<CharacterActionType> _onCharacterActionEvent;
        public event Action<CharacterActionType> OnCharacterActionEvent
        {
            add => _onCharacterActionEvent += value;
            remove => _onCharacterActionEvent -= value; 
        }
        public void Awake()
        {
            ICharacterActionTrigger[] actionTrigger = GetComponentsInParent<ICharacterActionTrigger>();
            if (!actionTrigger.Any()) return; 
            foreach (var receiver in actionTrigger)
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
        
        public void TriggerCharacterAction(string actionType)
        {
            _onCharacterActionEvent?.Invoke((CharacterActionType)Enum.Parse(typeof(CharacterActionType), actionType));    
        }
    
    }
}
