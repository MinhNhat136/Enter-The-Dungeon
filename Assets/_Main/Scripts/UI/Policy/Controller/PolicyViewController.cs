




using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>


    public class PolicyViewController : IController
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

        IContext IInitializableWithContext.Context => throw new System.NotImplementedException();


        //  Fields ----------------------------------------
        private bool _isIntiallized;
        private IContext _context;

        //  Dependencies ----------------------------------
        private PolicyService _service;
        private PolicyView _view;

        //  Initialization  -------------------------------
        public PolicyViewController(PolicyService service, PolicyView view)
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
        private void View_OnClickAcceptPolicy()
        {
            RequireIsInitialized();

            _service.AcceptPolicy();
            Context.CommandManager.InvokeCommand(new AcceptedPolicyCommand());
        }

        private void View_OnClickShowTermsOfPolicy()
        {
            RequireIsInitialized();
            _service.ShowTermsOfPolicy();
        }

    }
}

