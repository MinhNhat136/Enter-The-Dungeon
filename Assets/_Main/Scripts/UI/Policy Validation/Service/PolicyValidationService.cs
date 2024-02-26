using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using System.Threading.Tasks;
using UnityEngine.Events;


// CAUTION: NOT COMPLETED GET POLICY DATA FROM SERVER
// IM USING FAKE TASK.DELAY

namespace Atomic.Services
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PolicyValidationService : BaseService
    {
        //  Events ----------------------------------------
        public readonly UnityEvent<bool> OnCheckCompleted = new();

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            base.Initialize(context);
        }

        //  Other Methods ---------------------------------
        public async void CheckAcceptedPolicy()
        {
            RequireIsInitialized();
            await Task.Delay(2000);
            OnCheckCompleted.Invoke(ES3.Load<bool>(GameDataKey.IsAcceptedPolicy, false));
        }
    }
}




