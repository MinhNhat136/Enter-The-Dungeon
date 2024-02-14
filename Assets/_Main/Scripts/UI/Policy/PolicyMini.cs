using Atomic.Controllers;
using Atomic.Services;
using Atomic.UI.Views;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;



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
            get { return _policyChecker; }
        }

        public PolicyService PolicyService
        {
            get { return _policyService; }
        }

        public Context Context
        {
            get { return _context; }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;


        //  Dependencies ----------------------------------
        private readonly UIPopup _policyPopup;
        private Context _context;
        private PolicyValidationController _policyChecker;
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

                _context = new Context();
                _policyService = new PolicyService();
                _policyChecker = new PolicyValidationController(_policyService);

                _policyService.Initialize(_context);
                _policyChecker.Initialize(_context);

                _policyChecker.OnShowPolicy.AddListener(InitPolicyPopupMVC);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("No instance of Policy Mini");
            }
        }

        //  Other Methods ---------------------------------
        public void ShowPolicyPopup(UIPopup popup) => popup!.Show();

        public void InitPolicyPopupMVC()
        {
            var popup = UIPopup.Get(_policyPopup.name);
            if (popup.TryGetComponent<PolicyInfoView>(out PolicyInfoView view))
            {
                PolicyDisplayController controller = new(_policyService, view);

                view.Initialize(Context);
                controller.Initialize(Context);

                ShowPolicyPopup(popup);
            }
            else throw new System.Exception("Module Policy null view");

        }

        //  Event Handlers --------------------------------

    }
}



