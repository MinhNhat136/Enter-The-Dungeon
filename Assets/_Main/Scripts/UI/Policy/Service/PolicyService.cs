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
            bool isAccepted = ES3.Load<bool>(GameDataKey.IsAcceptedPolicy, false);
            OnCheckCompleted.Invoke(isAccepted);
        }

        public void AcceptPolicy()
        {
            ES3.Save<bool>(GameDataKey.IsAcceptedPolicy, true);
        }

        public void ShowTermsOfPolicy()
        {
            Application.OpenURL("https://www.law365.co/blog/end-user-license-agreement");
        }
    }
}



    
