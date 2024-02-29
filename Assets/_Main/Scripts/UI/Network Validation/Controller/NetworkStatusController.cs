using Atomic.Core;
using Atomic.Models;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Controller.Commands;
using System.Collections;
using UnityEngine;


namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class OnNetworkConnectChangeCommand : ValueChangedCommand<bool>
    {
        public OnNetworkConnectChangeCommand(bool previousValue, bool currentValue) : base(previousValue, currentValue)
        {

        }
    }

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
                _model.GetConnectionStatus.OnValueChanged.AddListener(Model_OnNetworkStatusChange);
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new System.Exception("No instance of NetworkChecker");
            }
        }

        //  Other Methods ---------------------------------
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

        // Event Handling ---------------------------------
        public void Model_OnNetworkStatusChange(bool previousValue, bool currentValue)
        {
            Context.CommandManager.InvokeCommand(new OnNetworkConnectChangeCommand(previousValue, currentValue));
        }

    }
}
