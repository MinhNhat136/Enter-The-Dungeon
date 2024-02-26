using Atomic.Command;
using Atomic.Core;
using DG.Tweening;
using Doozy.Runtime.Signals;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------


    /// <summary>
    /// View hiển thị màn hình Loading khi vừa khởi động game
    /// </summary>
    public class InitAppView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }
        public IContext Context
        {
            get
            {
                return _context;
            }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;
        private IContext _context;

        [SerializeField]
        private TextMeshProUGUI _stateText;

        [SerializeField]
        private DOTweenAnimation _loadingAnimation;


        [SerializeField]
        private SignalSender _loadLobbyScene;

        [SerializeField]
        private SignalSender _showAppTitleView;

        //  Initialization  -------------------------------

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Init App Title View not intialize");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void SendSignal_ShowAppTitleView()
        {
            _showAppTitleView.SendSignal();
        }

        public void SendSignal_LoadLobbyScene()
        {
            _loadLobbyScene.SendSignal();
        }

        //  Event Handlers --------------------------------
        public void SetStateText(string value)
        {
            Debug.Log(value);
            _stateText.text = value;
        }




    }
}

