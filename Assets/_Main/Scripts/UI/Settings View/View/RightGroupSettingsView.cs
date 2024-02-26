using Doozy.Runtime.UIManager.Components;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: 
    /// </summary>
    public class RightGroupSettingsView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] public UnityEvent OnClickedButtonLanguageUnityEvent = new();
        [HideInInspector] public UnityEvent OnClickedButtonUserIDUnityEvent = new();
        [HideInInspector] public UnityEvent OnClickedButtonLikeUnityEvent = new();
        [HideInInspector] public UnityEvent OnClickedButtonRateUnityEvent = new();
        [HideInInspector] public UnityEvent OnClickeedButtonAboutUnityEvent = new();
        [HideInInspector] public UnityEvent OnClickedButtonOtherGamesUnityEvent = new();

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
        [SerializeField]
        private UIButton _buttonLanguage;

        [SerializeField]
        private UIButton _buttonUserID;

        [SerializeField]
        private UIButton _buttonLike;

        [SerializeField]
        private UIButton _buttonRate;

        [SerializeField]
        private UIButton _buttonAbout;

        [SerializeField]
        private UIButton _buttonOtherGame;

        private bool _isInitialized;
        private IContext _context;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = false;
                _context = context;

                _buttonLanguage.onClickEvent.AddListener(ButtonLanguage_OnClicked);
                _buttonUserID.onClickEvent.AddListener(ButtonUserID_OnClicked);
                _buttonLike.onClickEvent.AddListener(ButtonLike_OnClicked);
                _buttonAbout.onClickEvent.AddListener(ButtonAbout_OnClicked);
                _buttonOtherGame.onClickEvent.AddListener(ButtonOtherGames_OnClicked);
                _buttonRate.onClickEvent.AddListener(ButtonRate_OnClicked);
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new System.Exception("Right group settings not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        private void ButtonLanguage_OnClicked()
        {
            OnClickedButtonLanguageUnityEvent.Invoke();
        }

        private void ButtonUserID_OnClicked()
        {
            OnClickedButtonUserIDUnityEvent.Invoke();
        }

        private void ButtonLike_OnClicked()
        {
            OnClickedButtonLikeUnityEvent.Invoke();
        }

        private void ButtonRate_OnClicked()
        {
            OnClickedButtonRateUnityEvent.Invoke();
        }

        private void ButtonAbout_OnClicked()
        {
            OnClickeedButtonAboutUnityEvent.Invoke();
        }

        private void ButtonOtherGames_OnClicked()
        {
            OnClickedButtonOtherGamesUnityEvent.Invoke();
        }
    }
}

