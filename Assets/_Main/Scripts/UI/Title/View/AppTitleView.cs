using Atomic.Command;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    //  Class ---------------------------------------------
    public class AppTitleView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public UnityEvent SignInWithGuessUnityEvent = new();

        [HideInInspector]
        public readonly UnityEvent SignInWithGameCenterUnityEvent = new();

        [HideInInspector]
        public readonly UnityEvent SignInWithFacebookUnityEvent = new();

        [HideInInspector]
        public readonly UnityEvent SignInWithGoogleUnityEvent = new();

        [HideInInspector]
        public readonly UnityEvent ShowViewSettingsUnityEvent = new();


        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized; } }
        public IContext Context { get { return _context; } }

        //  Fields ----------------------------------------
        [SerializeField] 
        private UIButton _buttonSignInWithGameCenter;
        
        [SerializeField] 
        private UIButton _buttonSignInWithFacebook;
        
        [SerializeField] 
        private UIButton _buttonSignInWithGoogle;
        
        [SerializeField] 
        private UIButton _buttonSignInWithGuest;
        
        [SerializeField]
        private UIButton _buttonSetting;
        
        [SerializeField] 
        private UIButton _buttonTapToStart;

        [SerializeField]
        private GameObject _loadingProgressBar;

        [SerializeField]
        private SignalSender _loadSceneSignal;


        private bool _isInitialized = false;
        private IContext _context;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _buttonSignInWithGameCenter.onClickEvent.AddListener(SignInWithGameCenterButton_OnClicked);
                _buttonSignInWithFacebook.onClickEvent.AddListener(SignInWithFacebookButton_OnClicked);
                _buttonSignInWithGoogle.onClickEvent.AddListener(SignInWithGoogleButton_OnClicked);
                _buttonSignInWithGuest.onClickEvent.AddListener(SignInWithGuessButton_OnClicked);

                _buttonTapToStart.onClickEvent.AddListener(TapToStartButton_OnClicked);

                Context.CommandManager.AddCommandListener<SignInCompletionCommand>(SignInController_OnSignInCompleted);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("MustBeInitialized");
            }
        }

        //  Unity Methods   -------------------------------
        protected void OnDestroy()
        {
            Context?.CommandManager?.RemoveCommandListener<SignInCompletionCommand>(
                SignInController_OnSignInCompleted);

        }

        //  Other Methods ---------------------------------
        private void SetVisibleSignInButtons(bool isVisible)
        {
            _buttonSignInWithFacebook.gameObject.SetActive(isVisible);
            _buttonSignInWithGuest.gameObject.SetActive(isVisible);
            _buttonSignInWithGoogle.gameObject.SetActive(isVisible);
            _buttonSignInWithGameCenter.gameObject.SetActive(isVisible);
        }

        private void SetVisibleTaptoStartButton(bool isVisible)
        {
            _buttonTapToStart.gameObject.SetActive(isVisible);
        }

        private void SetVisibleLoadingProgressBar(bool isVisible)
        {
            _loadingProgressBar.gameObject.SetActive(isVisible);
        }

        //  Event Handlers --------------------------------
        private void SignInWithGuessButton_OnClicked()
        {
            SignInWithGuessUnityEvent.Invoke();
        }

        private void SignInWithGameCenterButton_OnClicked()
        {
            SignInWithGameCenterUnityEvent.Invoke();
        }

        private void SignInWithFacebookButton_OnClicked()
        {
            SignInWithFacebookUnityEvent.Invoke();
        }

        private void SignInWithGoogleButton_OnClicked()
        {
            SignInWithGoogleUnityEvent.Invoke();
        }

        private void SettingsButton_OnClicked()
        {
            ShowViewSettingsUnityEvent.Invoke();
        }

        private void SignInController_OnSignInCompleted(SignInCompletionCommand command)
        {
            SetVisibleSignInButtons(!command.WasSuccess);
            SetVisibleTaptoStartButton(command.WasSuccess);
        }

        private void TapToStartButton_OnClicked()
        {
            SetVisibleLoadingProgressBar(true);
            SetVisibleTaptoStartButton(false);
            _loadSceneSignal.SendSignal();
        }
    }
}

