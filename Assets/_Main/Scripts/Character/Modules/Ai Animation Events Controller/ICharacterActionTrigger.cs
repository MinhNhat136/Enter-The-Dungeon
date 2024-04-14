
namespace Atomic.Character.Module
{
    public enum CharacterActionType
    {
        BeginRoll, 
        Rolling,
        StopRoll,
        BeginAttack,
        BeginCharge,
        Charge,
        BeginShoot,
        BeginMeleeAttack,
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
        /// Method Called from <seealso cref="AnimatorMoveSender"/>
        /// </summary>
        void OnCharacterActionTrigger(CharacterActionType actionType);

    }
}