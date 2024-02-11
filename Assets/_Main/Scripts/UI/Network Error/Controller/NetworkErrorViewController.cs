using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class NetworkErrorViewController : IController
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
        private readonly NetworkErrorView _view;
        private readonly NetworkModel _model;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public NetworkErrorViewController(NetworkErrorView view, NetworkModel model)
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
                _model.NetworkConnetion.OnValueChanged.AddListener(Model_OnNetworkConnectionChange);
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
            _model.NetworkConnetion.OnValueChanged.RemoveListener(Model_OnNetworkConnectionChange);
        }

        //  Event Handlers --------------------------------
        private void View_OnClickButtonOK()
        {
#if UNITY_EDITOR
            AppHelper.Quit();
#else
        Application.Quit();
#endif
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
