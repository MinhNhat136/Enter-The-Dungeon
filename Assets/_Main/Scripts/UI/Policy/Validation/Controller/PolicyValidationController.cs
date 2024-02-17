using Atomic.Command;
using Atomic.Services;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;
using System.Diagnostics;
using UnityEngine.Events;


namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class PolicyValidationController : IController
    {
        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }
        public IContext Context 
        { 
            get { return _context; } 
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;

        
        //  Dependencies-----------------------------------
        private IContext _context;
        private readonly PolicyValidationService _service;
        
        // Initialization  --------------------------------
        public PolicyValidationController(PolicyValidationService service)
        {
            _service = service;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true; 
                _context = context;

                _service.OnCheckCompleted.AddListener(Service_OnCheckCompleted);
                Context.CommandManager.AddCommandListener<StartPolicyValidateProgressCommand>(CheckPolicy);
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new Exception("No instance of PolicyChecker");
            }
        }

        //  Other Methods ---------------------------------
        public void CheckPolicy(StartPolicyValidateProgressCommand command)
        {
            RequireIsInitialized();
            _service.CheckAcceptedPolicy();
        }


        //  Event Handlers --------------------------------
        private void Service_OnCheckCompleted(bool isAccepted)
        {
            RequireIsInitialized();
            if (!isAccepted)
            {
                Context.CommandManager.InvokeCommand(new PolicyValidateCompletionCommand(isAccepted));
            }

        }
    }

}



