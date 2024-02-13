using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using System.Threading.Tasks;
using UnityEngine.Events;


// CAUTION: NOT YET COMPELTED THIS CLASS 
namespace Atomic.Services
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>

    public class PolicyService : BaseService
    {
        //  Events ----------------------------------------
        public readonly UnityEvent<bool> OnCheckCompleted = new();

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            base.Initialize(context);

        }

        //  Other Methods ---------------------------------
        public void CheckAcceptedPolicy()
        {
            RequireIsInitialized();

            CheckAsync();
        }

        private async void CheckAsync()
        {
            RequireIsInitialized();

            await Task.Delay(500);

            bool IsAccepted = false;

            OnCheckCompleted.Invoke(IsAccepted);
        }

        public void AcceptPolicy()
        {

        }

        public void ShowTermsOfPolicy()
        {

        }

        //  Event Handlers --------------------------------


    }
}



    
