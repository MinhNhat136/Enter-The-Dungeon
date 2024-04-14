using UnityEngine;

namespace Atomic.Character.Module
{
    public enum CharacterEnvironmentState
    {
        OnGrounded, 
        Airborne,
    }
    
    public class AiStateManager : MonoBehaviour, ICharacterActionTrigger
    {
        public CharacterEnvironmentState CharacterEnvironmentState { get; set; }

        public void OnCharacterActionTrigger(CharacterActionType actionType)
        {
            switch (actionType)
            {
                case CharacterActionType.StopRoll:
                    CharacterEnvironmentState = CharacterEnvironmentState.OnGrounded;
                    break; 
            }
        }
    }
    
}
