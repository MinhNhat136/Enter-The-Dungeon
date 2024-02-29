using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.Core
{
    public class UserProfilFormFillBaseObject : BaseGameObjectInitializableWithContext
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
       

        //  Fields ----------------------------------------

        [SerializeField]
        private UIPopup _signUpPopup;


        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                UserProfileFormFillMini mini = new(_signUpPopup, context);

                mini.Initialize();
            }
        }

        public override void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Guest Sign Up GameObject not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------

    }

}
