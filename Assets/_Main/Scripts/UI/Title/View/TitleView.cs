using Doozy.Runtime.UIManager.Components;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.View;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    //  Class ---------------------------------------------
    public class TitleView : MonoBehaviour, IView
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

        //  Properties ------------------------------------
        public bool IsInitialized { get { return _isInitialized; } }
        public IContext Context { get { return _context; } }

        //  Fields ----------------------------------------
        [SerializeField] 
        private UIButton buttonSignInWithGameCenter;
        
        [SerializeField] 
        private UIButton buttonSignInWithFacebook;
        
        [SerializeField] 
        private UIButton buttonSignInWithGoogle;
        
        [SerializeField] 
        private UIButton buttonSignInWithGuest;
        
        [SerializeField] 
        private UIButton buttonTapToStart;

        private bool _isInitialized = false;
        private IContext _context;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                buttonSignInWithGameCenter.onClickEvent.AddListener(SignInWithGameCenterButton_OnClicked);
                buttonSignInWithFacebook.onClickEvent.AddListener(SignInWithFacebookButton_OnClicked);
                buttonSignInWithGoogle.onClickEvent.AddListener(SignInWithGoogleButton_OnClicked);
                buttonSignInWithGuest.onClickEvent.AddListener(SignInWithGuessButton_OnClicked);

                buttonTapToStart.onClickEvent.AddListener(TapToStartButton_OnClicked);

                Context.CommandManager.AddCommandListener<SignInCompletedCommand>(SignInController_OnSignInCompleted);
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
            Context?.CommandManager?.RemoveCommandListener<SignInCompletedCommand>(
                SignInController_OnSignInCompleted);

        }

        //  Other Methods ---------------------------------
        private void SetVisibleSignInButtons(bool isVisible)
        {
            buttonSignInWithFacebook.gameObject.SetActive(isVisible);
            buttonSignInWithGuest.gameObject.SetActive(isVisible);
            buttonSignInWithGoogle.gameObject.SetActive(isVisible);
            buttonSignInWithGameCenter.gameObject.SetActive(isVisible);
        }

        private void SetVisibleTaptoStartButton(bool isVisible)
        {
            buttonTapToStart.gameObject.SetActive(isVisible);
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

        private void SignInController_OnSignInCompleted(SignInCompletedCommand command)
        {
            SetVisibleSignInButtons(!command.WasSuccess);
            SetVisibleTaptoStartButton(command.WasSuccess);
        }

        private void TapToStartButton_OnClicked()
        {

        }
    }
}

