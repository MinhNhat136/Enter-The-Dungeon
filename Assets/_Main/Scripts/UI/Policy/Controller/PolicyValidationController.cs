using Atomic.Services;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;
using UnityEngine.Events;


namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PolicyValidationController : IController
    {
        //  Events ----------------------------------------
        public UnityEvent OnShowPolicy = new();

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
        private readonly PolicyService _service;
        
        // Initialization  --------------------------------
        public PolicyValidationController(PolicyService service)
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

                _service.CheckAcceptedPolicy();
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



        //  Event Handlers --------------------------------
        private void Service_OnCheckCompleted(bool isAccepted)
        {
            RequireIsInitialized();
            if (!isAccepted)
            {
                OnShowPolicy.Invoke();
            }

        }


    }

}



