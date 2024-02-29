using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.Core
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class UserProfileValidateBaseObject : BaseGameObjectInitializableWithContext
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------


        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                UserProfileValidateMini validateMini = new(context);
                validateMini.Initialize();

            }
        }

        public override void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("UserProfile Validation GameObject not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------

    }

}