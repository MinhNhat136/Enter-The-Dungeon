using Atomic.Core;
using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiMotorController : MonoBehaviour, IInitializable
    {
        //  Statics ---------------------------------------
        private static long Controller_LocomotionIndex = 1L << 0;
        private static long Controller_RollIndex = 1L << 1;
        private static long Controller_FlyIndex = 1L << 2;

        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public ILocomotionController LocomotionController {
            get { return _locomotionController; }
            private set { _locomotionController = value; } }
        public AiRollController RollController { get; private set; }

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }
        //  Fields ----------------------------------------
        private long _controllerBitSequence = 0;
        private bool _isInitialized;
        private ILocomotionController _locomotionController;
        //  Initialization  -------------------------------
        public void Initialize()
        {
            if(!_isInitialized)
            {
                _isInitialized = true;

                AtomicExtension.SetController<ILocomotionController>(this, ref _locomotionController, Controller_LocomotionIndex, ref _controllerBitSequence);
            }
        }

        public void RequireIsInitialized()
        {
        }

        //  Unity Methods   -------------------------------

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
    }
}
