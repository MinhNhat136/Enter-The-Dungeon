using Atomic.Models;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using UnityEngine;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class NetworkErrorDisplayController : IController
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
        private readonly NetworkErrorDisplayView _view;
        private readonly NetworkStateModel _model;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public NetworkErrorDisplayController(NetworkErrorDisplayView view, NetworkStateModel model)
        {
            _view = view;
            _model = model;
        }


        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;


                _view.OnClickButtonOK.AddListener(View_OnClickButtonOK);
                _model.GetConnectionStatus.OnValueChanged.AddListener(Model_OnNetworkConnectionChange);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("No instance of Network Error View");
            }
        }

        //  Other Methods ---------------------------------
        private void OnDestroyController()
        {
            _view.OnClickButtonOK.RemoveListener(View_OnClickButtonOK);
            _model.GetConnectionStatus.OnValueChanged.RemoveListener(Model_OnNetworkConnectionChange);
        }

        //  Event Handlers --------------------------------
        private void View_OnClickButtonOK()
        {
            Application.Quit();
        }

        private void Model_OnNetworkConnectionChange(bool previousConnection, bool currentConnetion)
        {
            if(previousConnection == currentConnetion)
            {
                return;
            }

            if (currentConnetion)
            {
                OnDestroyController();
                _view.HidePopup();
            }

        }

    }
}
