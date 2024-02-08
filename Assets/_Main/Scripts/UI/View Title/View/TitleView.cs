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
    public class CheckPlayFirstTimeUnityEvent : UnityEvent { }

    //  Class ---------------------------------------------
    public class TitleView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public readonly CheckPlayFirstTimeUnityEvent CheckPlayFirstTimeUnityEvent = new CheckPlayFirstTimeUnityEvent();

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

                Context.CommandManager.AddCommandListener<ShowFormFillUserDataCommand>(
                   OnShowFormFillUserData);
   

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

        private void SignInWithGuessButton_OnClicked()
        {
            CheckPlayFirstTimeUnityEvent.Invoke();
        }
        
        private void SignInWithGameCenterButton_OnClicked()
        {

        }

        private void SignInWithFacebookButton_OnClicked()
        {

        }

        private void SignInWithGoogleButton_OnClicked()
        {

        }

        private void TapToStartButton_OnClicked()
        {

        }

        protected void OnDestroy()
        {
            
        }
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        public void OnShowFormFillUserData(ShowFormFillUserDataCommand command)
        {
            Debug.Log("OnShowFormFillUserData");
        }
    }
}

