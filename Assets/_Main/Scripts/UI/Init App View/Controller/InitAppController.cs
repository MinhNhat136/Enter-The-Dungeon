using Atomic.Chain;
using Atomic.Command;
using Atomic.Core.Interface;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// CONTROLLER xử lý Logic Loading các Components và Features cần thiết khi vừa khởi động game
    /// </summary>
    public class InitAppController : IController
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
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;
        private IContext _context;
        private readonly InitAppView _view;

        //  Initialization  -------------------------------
        public InitAppController(InitAppView view)
        {
            _view = view;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;


                Context.CommandManager.AddCommandListener<OnUIFlowStartCommand>(InitApp);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Init App Controller not initialied");

            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        private void InitApp(OnUIFlowStartCommand command)
        {
            InitChain();
        }

        private void InitChain()
        {
            IChain loadingInitApp = new StartInitAppChain();
            IChain policyValidationChain = new PolicyValidateChain();
            IChain userProfileValidationChain = new UserProfileValidateChain();

            loadingInitApp.SetContext(_context).SetNextHandler(policyValidationChain);
            policyValidationChain.SetContext(_context).SetNextHandler(userProfileValidationChain);
            userProfileValidationChain.SetContext(_context);

            loadingInitApp.Handle();
        }
        //  Event Handlers --------------------------------



    }
}

