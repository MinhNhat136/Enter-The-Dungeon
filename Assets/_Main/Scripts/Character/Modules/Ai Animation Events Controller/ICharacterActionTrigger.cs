
using System;

namespace Atomic.Character.Module
{
    [Flags]
    public enum CharacterActionType
    {
        OnFoot = 1 << 0,
        
        BeginRoll = 1 << 1,
        Rolling = 1 << 2,
        EndRoll = 1 << 3,
        
        BeginPrepareAttack = 1 << 4,
        PreparingAttack = 1 << 5,
        EndPrepareAttack = 1 << 6,
        
        ChargeFull = 1 << 7,
        
        BeginAttackMove = 1 << 8,
        AttackMoving = 1 << 9,
        EndAttackMove = 1 << 10,
        
        BeginAttack = 1 << 11,
        Attacking = 1 << 12,
        EndAttack = 1 << 13,
        
        MoveNextSkill = 1 << 14,
        
        BeginTrack = 1 << 15,
        StopTrack = 1 << 16,
        CustomAction = 1 << 17,
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