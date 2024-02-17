using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine;
using UnityEngine.Events;


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
        public void CheckAcceptedPolicy()
        {
            RequireIsInitialized();
            bool isAccepted = ES3.Load<bool>(GameDataKey.IsAcceptedPolicy, false);
            OnCheckCompleted.Invoke(isAccepted);
        }
    }
}



    
