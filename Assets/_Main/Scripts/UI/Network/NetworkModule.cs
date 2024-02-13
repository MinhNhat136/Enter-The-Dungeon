using Atomic.Controllers;
using Atomic.Models;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;


namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------


    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class NetworkModule : IMiniMvcs
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public IContext Context { get { return _context; } }

        //  Fields ----------------------------------------
        private bool _isInitialized;
        private IContext _context;
        private readonly UIPopup _popup;
        private NetworkStateModel _model; 
        private NetworkStatusController _checker;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public NetworkModule(UIPopup popup)
        {
            _popup = popup;
        }

        public void Initialize()
        {
            if(!IsInitialized)
            {
                _isInitialized = true;

                _context = new Context();

                _model = new NetworkStateModel();
                _checker = new NetworkStatusController(_model);

                _model.Initialize(_context);
                _checker.Initialize(_context);

                _model.GetConnectionStatus.OnValueChanged.AddListener(OnNetworkConnectionChange);
            
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        private void OnNetworkConnectionChange(bool previousConnetion, bool currentConnection)
        {
            if(previousConnetion == currentConnection)
            {
                return;
            }

            if (currentConnection)
            {
                return; 
            }

            InitNetworkErrorPopupMVC();
        }

        private void View_DisplayNetworkStatus(UIPopup popup) => popup.Show();

        private void InitNetworkErrorPopupMVC()
        {
            var popup = UIPopup.Get(_popup.name);
            View_DisplayNetworkStatus(popup);
            if (popup.TryGetComponent<NetworkErrorDisplayView>(out NetworkErrorDisplayView view))
            {
                NetworkErrorDisplayController _controller = new(view, _model);

                view.Initialize(Context);
                _controller.Initialize(Context);

            }
            else throw new System.Exception("Module Policy null view");
        }


    }
}


