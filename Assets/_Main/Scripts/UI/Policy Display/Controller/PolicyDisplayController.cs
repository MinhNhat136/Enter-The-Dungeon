using Atomic.Services;
using Atomic.UI.Views;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Controller.Commands;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public class AcceptedPolicyRulesCommand : ICommand
    {

    }
    /// <summary>
    /// Manages the display of policy information, handling user interactions and coordinating between the view and service layers.
    /// </summary>
    public class PolicyDisplayController : IController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isIntiallized; }
        }

        public IContext Context
        {
            get { return _context; }
        }

        //  Fields ----------------------------------------
        private bool _isIntiallized;
        private IContext _context;

        //  Dependencies ----------------------------------
        private readonly PolicyDisplayService _service;
        private readonly PolicyDisplayView _view;

        //  Initialization  -------------------------------
        public PolicyDisplayController(PolicyDisplayService service, PolicyDisplayView view)
        {
            _service = service;
            _view = view;
        }


        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isIntiallized = true;
                _context = context;

                _view.OnAcceptPolicy.AddListener(View_OnClickAcceptPolicy);
                _view.OnShowTermsOfPolicy.AddListener(View_OnClickShowTermsOfPolicy);
                _view.OnViewDestroy.AddListener(View_OnDestroy);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("No Instance of PolicyViewController");
            }
        }


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------

        private void View_OnDestroy()
        {
            _view.OnAcceptPolicy.RemoveListener(View_OnClickAcceptPolicy);
            _view.OnShowTermsOfPolicy.RemoveListener(View_OnClickShowTermsOfPolicy);
            _view.OnViewDestroy.RemoveListener(View_OnDestroy);
        }

        private void View_OnClickAcceptPolicy()
        {
            RequireIsInitialized();

            _service.AcceptPolicy();
            Context.CommandManager.InvokeCommand(new AcceptedPolicyRulesCommand());
            _view.HidePopup();
        }

        private void View_OnClickShowTermsOfPolicy()
        {
            RequireIsInitialized();
            _service.ShowTermsOfPolicy();
        }

    }
}

