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
    public class NetworkPopupMini : IInitializableWithContext
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
        private readonly UIPopup _popup;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public NetworkPopupMini(UIPopup popup, IContext context)
        {
            _popup = popup;
            _context = context;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                Context.CommandManager.AddCommandListener<OnNetworkConnectChangeCommand>(OnNetworkConnectionChange);
                 
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Network Mini not initialize");
            }
        }

        private void OnNetworkConnectionChange(OnNetworkConnectChangeCommand command)
        {
            if(command.PreviousValue == command.CurrentValue)
            {
                return;
            }

            if (command.CurrentValue)
            {
                return; 
            }

            RequireIsInitialized();
            InitNetworkErrorPopupMVC();
        }

        private void InitNetworkErrorPopupMVC()
        {
            var popup = UIPopup.Get(_popup.name);
            if (popup.TryGetComponent<NetworkErrorDisplayView>(out NetworkErrorDisplayView view))
            {
                var model = Context.ModelLocator.GetItem<NetworkStateModel>();
                NetworkErrorDisplayController _controller = new(view, model);

                view.Initialize(Context);
                _controller.Initialize(Context);

                popup!.Show();
            }
            else throw new System.Exception("Module Policy null view");
        }


    }
}


