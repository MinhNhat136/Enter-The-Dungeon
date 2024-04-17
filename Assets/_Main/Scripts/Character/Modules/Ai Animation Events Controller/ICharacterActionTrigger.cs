
namespace Atomic.Character.Module
{
    public enum CharacterActionType
    {
        OnFoot,
        BeginRoll, 
        StopRoll,
        PrepareAttack,
        PrepareAttackCompleted,
        ChargeFull,
        BeginAttack,
        BeginAttackMove,
        StopAttackMove,
        EndAttack,
        MoveNextSkill,
        BeginTrack, 
        StopTrack,
        CustomAction,
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