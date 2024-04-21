
using System;

namespace Atomic.Character.Module
{
    [Flags]
    public enum CharacterActionType
    {
        MoveNextSkill = 1 << 0,
        
        OnFoot = 1 << 1,
        OnFeedBack = 1 << 2,
        
        BeginRoll = 1 << 3,
        Rolling = 1 << 4,
        EndRoll = 1 << 5,
        
        BeginPrepareAttack = 1 << 6,
        PreparingAttack = 1 << 7,
        EndPrepareAttack = 1 << 8,
        
        ChargeFull = 1 << 9,
        
        BeginAttackMove = 1 << 10,
        AttackMoving = 1 << 11,
        EndAttackMove = 1 << 12,
        
        BeginAttack = 1 << 13,
        Attacking = 1 << 14,
        EndAttack = 1 << 15,
        
        BeginHitDamage = 1 << 16,
        EndHitDamage = 1 << 17,
        
        BeginTrack = 1 << 18,
        StopTrack = 1 << 19,
        
        BeginTrail = 1 << 20, 
        StopTrail = 1 << 21,
        
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