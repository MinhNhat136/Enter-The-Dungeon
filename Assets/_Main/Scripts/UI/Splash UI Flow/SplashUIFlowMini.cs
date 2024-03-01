using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;

namespace Atomic.Controllers
{
    public class SplashUIFlowMini : IInitializableWithContext
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
        private SplashUIFlowView _view; 

        //  Initialization  -------------------------------
        public SplashUIFlowMini(SplashUIFlowView view)
        {
            _view = view;
        }

        public void Initialize(IContext context)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _context = context;

                SplashUIFlowController controller = new(_view);

                _view.Initialize(_context);
                controller.Initialize(_context);
            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new System.Exception("Splash UI Flow Mini not initialized");
            }
        }


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------


    }
}
