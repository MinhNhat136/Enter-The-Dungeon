using Atomic.Models;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

// CAUTION: THIS CLASS NOT COMPLETE SIGN IN FUNCTION
namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>

    public class UserProfileValidateService : BaseService
    {
        //  Events ----------------------------------------
        public readonly UnityEvent<UserProfileData, bool> OnValidateCompleted = new();

        //  Properties ------------------------------------

        //  Fields ----------------------------------------

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
            }
        }

        //  Methods ---------------------------------------
        public async void UserProfileValidate()
        {
            RequireIsInitialized();
            await Task.Delay(2000);
            UserProfileData user = ES3.Load<UserProfileData>(GameDataKey.UserProfileData, new UserProfileData(""));
            bool wasSuccess = user.Name != "";
            OnValidateCompleted.Invoke(user, wasSuccess);
        }

        //  Event Handlers --------------------------------
    }
}



