using Atomic.Controllers;
using Doozy.Runtime.Nody;
using Doozy.Runtime.Signals;
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

    public class SplashUIFlowView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public UnityEvent OnViewDestroy = new();

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
        private FlowController _flowController;

        [SerializeField]
        private SignalSender _loadLobbyScene;
        [SerializeField]
        private SignalSender _showAppTitleView;



        private bool _isInitialized;
        private IContext _context;


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
                throw new System.Exception("UI FLow View not initialized");
            }
        }


        //  Unity Methods   -------------------------------
        private void OnDestroy()
        {
            OnViewDestroy.Invoke();
        }

        //  Other Methods ---------------------------------
        public void PauseFlow() => _flowController.PauseFlow();
        public void ResumeFlow() => _flowController.ResumeFlow();
        public void StartFlow() => _flowController.StartFlow();
        public void StopFlow() => _flowController.StopFlow();

        public void SendSignal_ShowAppTitleView()
        {
            _showAppTitleView.SendSignal();
        }

        public void SendSignal_LoadLobbyScene()
        {
            _loadLobbyScene.SendSignal();
        }
        //  Event Handlers --------------------------------

    }

}