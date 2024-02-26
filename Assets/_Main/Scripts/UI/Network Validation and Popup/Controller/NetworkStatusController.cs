using Atomic.Core;
using Atomic.Models;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System.Collections;
using UnityEngine;


namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// </summary>
    public class NetworkStatusController : IController
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
        private readonly NetworkStateModel _model;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public NetworkStatusController(NetworkStateModel model)
        {
            _model = model;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                Coroutines.StartCoroutine(Coroutine_CheckNetworkAvailability());
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new System.Exception("No instance of NetworkChecker");
            }
        }

        public IEnumerator Coroutine_CheckNetworkAvailability()
        {
            RequireIsInitialized();
            yield return new WaitForSeconds(2);
            Check_NetworkAvailability();

        }

        public void Check_NetworkAvailability()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                _model.SetConnectionStatus = false;
            }
            else _model.SetConnectionStatus = true;
            Coroutines.StartCoroutine(Coroutine_CheckNetworkAvailability());
        }


        //  Other Methods ---------------------------------

    }
}
