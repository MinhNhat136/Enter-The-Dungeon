using Atomic.Controllers;
using Atomic.Services;
using Atomic.UI.Views;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System;



namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Manages a minimalistic display of policy information through a popup UI, integrating with the application's command system for policy validation.
    /// </summary>
    public class PolicyValidationMini : IMiniMvcs
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public IContext Context
        {
            get { return _context; }
            private set { _context = value; }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;


        //  Dependencies ----------------------------------
        private IContext _context;

        //  Initialization  -------------------------------
        public PolicyValidationMini(IContext context)
        {
            _context = context;
        }
        public void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;

                PolicyValidationService _policyService = new();
                PolicyValidationController _policyValidationController = new(_policyService);

                RequireContext();

                _policyService.Initialize(_context);
                _policyValidationController.Initialize(_context);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("No instance of Policy Mini");
            }
        }

        public void RequireContext()
        {
            if (Context == null)
            {
                throw new Exception("Policy Mini not have Context");
            }
        }

        //  Other Methods ---------------------------------

        //  Event Handlers --------------------------------

    }
}



