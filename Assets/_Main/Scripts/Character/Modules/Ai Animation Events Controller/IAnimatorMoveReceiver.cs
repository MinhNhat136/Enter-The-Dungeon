namespace Atomic.Character.Module
{
    /// <summary>
    /// Interface to receive events from <seealso cref="AnimatorEventsListenerWithRootMotion"/>
    /// </summary>
    public interface IAnimatorMoveReceive
    {
        /// <summary>
        /// Check if Component is Enabled
        /// </summary>
        bool enabled { get; set; }
        /// <summary>
        /// Method Called from <seealso cref="AnimatorMoveSender"/>
        /// </summary>
        void OnAnimatorMoveEvent();
    }
}

