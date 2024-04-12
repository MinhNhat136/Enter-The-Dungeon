
namespace Atomic.Character.Module
{
    
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