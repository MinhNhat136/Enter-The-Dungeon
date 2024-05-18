
using System;

namespace Atomic.Character
{
    [Flags]
    public enum CharacterActionType
    {
        MoveNextSkill = 1 << 0,
        
        OnFoot = 1 << 1,
        OnFeedBack = 1 << 2,
        
        BeginRoll = 1 << 3,
        EndRoll = 1 << 4,
        
        BeginPrepareAttack = 1 << 5,
        EndPrepareAttack = 1 << 6,
        
        ChargeFull = 1 << 7,
        
        BeginAttackMove = 1 << 8,
        EndAttackMove = 1 << 9,
        
        BeginAttack = 1 << 10,
        EndAttack = 1 << 11,
        
        CustomAction = 1 << 22,
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