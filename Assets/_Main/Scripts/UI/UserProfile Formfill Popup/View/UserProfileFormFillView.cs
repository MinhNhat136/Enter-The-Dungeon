using Atomic.Controllers;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class UserProfileFormFillView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        public readonly UnityEvent<string> OnClickButtonOkUnityEvent = new();

        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public IContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        //  Fields ----------------------------------------
        [SerializeField]
        private UIButton _buttonOK;

        [SerializeField]
        private TMP_InputField _inputUsername;

        [SerializeField]
        private UIPopup _popup;

        private bool _isInitialized;
        private IContext _context;


        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _buttonOK.onClickEvent.AddListener(ButtonOk_OnClicked);
                Context.CommandManager.AddCommandListener<UserProfileValidateCompletionCommand>(HidePopup);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("GuestSignUpView not yet initialized");
            }
        }

        //  Unity Methods   -------------------------------
        private void OnDestroy()
        {
            _buttonOK.onClickEvent.RemoveListener(ButtonOk_OnClicked);
            Context.CommandManager.RemoveCommandListener<UserProfileValidateCompletionCommand>(HidePopup);
        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        private void ButtonOk_OnClicked()
        {
            if (_inputUsername.text == "") return;
            OnClickButtonOkUnityEvent.Invoke(_inputUsername.text);
        }

        private void HidePopup(UserProfileValidateCompletionCommand command)
        {
            RequireIsInitialized();
            if (command.WasSuccess)
            {
                _popup.Hide();

            }
        }
    }
}