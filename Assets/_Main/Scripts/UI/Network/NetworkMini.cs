using Atomic.Controllers;
using Atomic.Models;
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
    public class NetworkMini : IMiniMvcs
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
        private IContext _context;
        private readonly UIPopup _popup;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public NetworkMini(UIPopup popup, IContext context)
        {
            _popup = popup;
            _context = context;
        }

        public void Initialize()
        {
            if(!IsInitialized)
            {
                _isInitialized = true;

                NetworkStateModel _model = new();
                NetworkStatusController _checker = new(_model);

                RequireContext();

                _model.Initialize(_context);
                _checker.Initialize(_context);

                _model.GetConnectionStatus.OnValueChanged.AddListener(OnNetworkConnectionChange);
            
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        public void RequireContext()
        {
            if (Context == null)
            {
                throw new Exception("Network Mini not have Context");
            }
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
                var model = Context.ModelLocator.GetItem<NetworkStateModel>();

                NetworkErrorDisplayController _controller = new(view, model);

                RequireContext();

                view.Initialize(Context);
                _controller.Initialize(Context);

            }
            else throw new System.Exception("Module Policy null view");
        }


    }
}


