using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;


namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------


    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class NetworkErrorDisplayView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] public UnityEvent OnClickButtonOK = new();
        [HideInInspector] public UnityEvent OnViewDestroy = new();

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
        [SerializeField] private UIButton _buttonOK;
        private UIPopup _uiPopup;

        private bool _isInitialized;
        private IContext _context;

        
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _uiPopup = GetComponent<UIPopup>();
                _buttonOK.onClickEvent.AddListener(ButtonOK_OnClicked);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("No instance of Network Error View");
            }
        }

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------
        public void HidePopup()
        {
            RequireIsInitialized();
            OnViewDestroy.Invoke();
            _uiPopup.Hide();
        }

        public void OnDestroy()
        {
            _buttonOK.onClickEvent.RemoveAllListeners();
        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        private void ButtonOK_OnClicked()
        {
            OnClickButtonOK.Invoke();
        }
    }
}
