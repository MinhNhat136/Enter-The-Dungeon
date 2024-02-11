using Atomic.Core;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System.Collections;
using UnityEngine;


namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// </summary>
    public class NetworkChecker : IController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }
        public IContext Context
        {
            get { return _context;}
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;
        private IContext _context;
        private readonly NetworkModel _model;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public NetworkChecker(NetworkModel model)
        {
            _model = model;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                Coroutines.StartCoroutine(Coroutine_CheckNetworkConnection());
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new System.Exception("No instance of NetworkChecker");
            }
        }

        public IEnumerator Coroutine_CheckNetworkConnection()
        {
            RequireIsInitialized();

            yield return new WaitForSeconds(2);
            CheckNetworkConnection();

        }

        public void CheckNetworkConnection()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                _model.IsNetworkConntected = false;
            }
            else _model.IsNetworkConntected = true;
            Coroutines.StartCoroutine(Coroutine_CheckNetworkConnection());
        }


        //  Other Methods ---------------------------------

    }
}
