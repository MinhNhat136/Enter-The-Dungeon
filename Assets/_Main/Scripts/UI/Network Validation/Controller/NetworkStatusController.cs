using Atomic.Command;
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

    public class StartValidateNetworkConnectionCommand : ICommand
    {

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

                Context.CommandManager.AddCommandListener<StartValidateNetworkConnectionCommand>(Command_CheckNetworkConnection);
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
        public IEnumerator Coroutine_CheckNetworkConnection()
        {
            yield return new WaitForSeconds(2f);
            Check_NetworkAvailability();
        } 

        public void Check_NetworkAvailability()
        {
            bool previousValue = _model.GetConnectionStatus.Value;
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                _model.SetConnectionStatus = false;
            }
            else _model.SetConnectionStatus = true;
            Context.CommandManager.InvokeCommand(new OnNetworkConnectChangeCommand(previousValue, _model.GetConnectionStatus.Value));

            Coroutines.StartCoroutine(Coroutine_CheckNetworkConnection());
        }

        // Event Handling ---------------------------------
        public void Command_CheckNetworkConnection(StartValidateNetworkConnectionCommand command)
        {
            Check_NetworkAvailability();
        }
    }
}
