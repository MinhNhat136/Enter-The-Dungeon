using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.Core
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class InitAppBaseObject : BaseGameObjectInitializableWithContext
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
      

        //  Fields ----------------------------------------
        [SerializeField]
        private InitAppView _view;

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                InitAppMini mini = new(_context, _view);
                mini.Initialize();
            }
        }

        public override void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Init App Game Object not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
    }
}

