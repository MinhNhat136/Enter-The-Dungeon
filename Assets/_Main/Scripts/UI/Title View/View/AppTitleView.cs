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
        public UnityEvent SignUpWithGuessUnityEvent = new();

        [HideInInspector]
        public readonly UnityEvent SignUpWithGameCenterUnityEvent = new();

        [HideInInspector]
        public readonly UnityEvent SignUpWithFacebookUnityEvent = new();

        [HideInInspector]
        public readonly UnityEvent SignUpWithGoogleUnityEvent = new();

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

                Context.CommandManager.AddCommandListener<UserProfileValidateCompletionCommand>(OnUserProfileValidateCompleted);
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
            Context?.CommandManager?.RemoveCommandListener<UserProfileValidateCompletionCommand>(
                OnUserProfileValidateCompleted);

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

        private void SetVisibleSettingsButton(bool isVisible)
        {
            _buttonSetting.gameObject.SetActive(isVisible);
        }

        //  Event Handlers --------------------------------
        private void SignInWithGuessButton_OnClicked()
        {
            SignUpWithGuessUnityEvent.Invoke();
        }

        private void SignInWithGameCenterButton_OnClicked()
        {
            SignUpWithGameCenterUnityEvent.Invoke();
        }

        private void SignInWithFacebookButton_OnClicked()
        {
            SignUpWithFacebookUnityEvent.Invoke();
        }

        private void SignInWithGoogleButton_OnClicked()
        {
            SignUpWithGoogleUnityEvent.Invoke();
        }

        private void OnUserProfileValidateCompleted(UserProfileValidateCompletionCommand command)
        {
            SetVisibleSignInButtons(!command.WasSuccess);
            SetVisibleSettingsButton(!command.WasSuccess);
            SetVisibleTaptoStartButton(command.WasSuccess);
        }

    }
}

