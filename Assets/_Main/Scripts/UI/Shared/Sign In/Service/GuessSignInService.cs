using Atomic.Models;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using System.Threading.Tasks;
using UnityEngine.Events;

// CAUTION: THIS CLASS NOT COMPLETE SIGN IN FUNCTION
namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>

    public class GuessSignInService : BaseService
    {
        //  Events ----------------------------------------
        public readonly UnityEvent<UserProfileData, bool> OnSignInCompleted = new();

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
        public void SignIn()
        {
            RequireIsInitialized();

            Login();
        }

        private void Login()
        {
            RequireIsInitialized();
            UserProfileData user = ES3.Load<UserProfileData>(GameDataKey.UserProfileData, new UserProfileData(""));
            bool wasSuccess = user.Name != "";
            OnSignInCompleted.Invoke(user, wasSuccess);
        }

        //  Event Handlers --------------------------------
    }
}



