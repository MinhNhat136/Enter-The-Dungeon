using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.UI.Views
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PolicyInfoView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector] public UnityEvent OnAcceptPolicy = new();
        [HideInInspector] public UnityEvent OnShowTermsOfPolicy = new();

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
        [SerializeField] private UIButton _buttonAccept;
        [SerializeField] private UIButton _buttonTermsOfService;
        [SerializeField] private UIPopup _popup;

        private IContext _context;
        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _buttonAccept.onClickEvent.AddListener(AcceptButton_OnClicked);
                _buttonTermsOfService.onClickEvent.AddListener(TermsOfPolicyButton_OnClicked);

                Context.CommandManager.AddCommandListener<AcceptedPolicyRulesCommand>(OnAcceptedPolicyCommand);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("No initialize yet");
            }
        }

        //  Unity Methods   -------------------------------
        private void AcceptButton_OnClicked()
        {
            OnAcceptPolicy.Invoke();
        }

        private void TermsOfPolicyButton_OnClicked()
        {
            OnShowTermsOfPolicy.Invoke();
        }

        private void OnDestroy()
        {
            Context?.CommandManager.RemoveCommandListener<AcceptedPolicyRulesCommand>(OnAcceptedPolicyCommand);
        }

        //  Other Methods ---------------------------------



        //  Event Handlers --------------------------------
        private void OnAcceptedPolicyCommand(AcceptedPolicyRulesCommand command)
        {
            RequireIsInitialized();
            _popup.Hide();
        }
    }

}