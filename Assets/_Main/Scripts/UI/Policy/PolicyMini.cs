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
    /// TODO: Replace with comments...
    /// </summary>
    public class PolicyMini : IMiniMvcs
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }
        public PolicyValidationController PolicyChecker
        {
            get { return _policyValidationController; }
        }

        public PolicyService PolicyService
        {
            get { return _policyService; }
        }

        public IContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;


        //  Dependencies ----------------------------------
        private readonly UIPopup _policyPopup;
        private IContext _context;
        private PolicyValidationController _policyValidationController;
        private PolicyService _policyService;


        //  Initialization  -------------------------------
        public PolicyMini(UIPopup policyPopup)
        {
            _policyPopup = policyPopup;
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;

                _policyService = new PolicyService();

                _policyValidationController = new PolicyValidationController(_policyService);
                _policyValidationController.OnShowPolicy.AddListener(InitPolicyPopupMVC);

                RequireContext();

                _policyService.Initialize(_context);
                _policyValidationController.Initialize(_context);

                _policyValidationController.CheckPolicy();

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
                throw new Exception("Sign Up Mini not have Context");
            }
        }

        //  Other Methods ---------------------------------
        public void InitPolicyPopupMVC()
        {
            var popup = UIPopup.Get(_policyPopup.name);
            if (popup.TryGetComponent<PolicyInfoView>(out PolicyInfoView view))
            {
                PolicyDisplayController controller = new(_policyService, view);

                view.Initialize(Context);
                controller.Initialize(Context);

                popup!.Show();
            }
            else throw new System.Exception("Module Policy null view");

        }

        //  Event Handlers --------------------------------

    }
}



