using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PolicyChecker : IController
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
        private PolicyService _service;
        
        // Initialization  --------------------------------
        public PolicyChecker(PolicyService service)
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



