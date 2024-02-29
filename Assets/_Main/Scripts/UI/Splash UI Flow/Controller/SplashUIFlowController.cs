using Atomic.Command;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class SplashUIFlowController : IController
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
        private readonly SplashUIFlowView _view;

        //  Initialization  -------------------------------
        public SplashUIFlowController(SplashUIFlowView view)
        {
            _view = view;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _view.onFlowStart += View_OnFlowStart;
                _view.onFlowResume += View_OnFlowResume;
                _view.onFlowPause += View_OnFlowPause;
                _view.onFlowStop += View_OnFlowStop;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Splash UI Flow Controller not inititialize");
            }
        }


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        public void View_OnFlowStart()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new OnUIFlowStartCommand());
        }

        public void View_OnFlowResume()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new OnUIFlowResumeCommand());
        }

        public void View_OnFlowPause()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new OnUIFlowPauseCommand());
        }

        public void View_OnFlowStop()
        {
            RequireIsInitialized();
            Context.CommandManager.InvokeCommand(new OnUIFlowStopCommad());
        }
    }

}
