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

    public class GuestSignInService : BaseService
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

            LoginAsync();
        }

        private async void LoginAsync()
        {
            RequireIsInitialized();

            await Task.Delay(500);

            // CAUTION: Complete this code
            bool wasSuccess = true;

            // Doing Something here

            OnSignInCompleted.Invoke(new("Nhat", 18), wasSuccess);

        }

        //  Event Handlers --------------------------------
    }
}


