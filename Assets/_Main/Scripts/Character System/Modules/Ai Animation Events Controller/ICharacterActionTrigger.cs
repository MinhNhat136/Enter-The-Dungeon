
using System;

namespace Atomic.Character
{
    [Flags]
    public enum CharacterActionType
    {
        MoveNextSkill = 1 << 0,
        
        BeginRoll = 1 << 1,
        EndRoll = 1 << 2,
        
        BeginPrepareAttack = 1 << 3,
        EndPrepareAttack = 1 << 4,
        
        ChargeFull = 1 << 5,
        
        BeginAttackMove = 1 << 6,
        EndAttackMove = 1 << 7,
        
        BeginAttack = 1 << 8,
        EndAttack = 1 << 9,
        
        BeginHit = 1 << 10, 
        EndHit = 1 << 11, 
        
        CustomAction = 1 << 12,
    }
    
    public interface ICharacterActionTrigger 
    {
        /// <summary>
        /// Check if Component is Enabled
        /// </summary>
        bool enabled { get; set; }
        /// <summary>
        /// Method Called from <seealso cref="AnimatorEventsListener"/>
        /// </summary>
        void OnCharacterActionTrigger(CharacterActionType actionType);

    }
}