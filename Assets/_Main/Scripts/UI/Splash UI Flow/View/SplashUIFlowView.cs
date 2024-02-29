using Atomic.Controllers;
using Doozy.Runtime.Nody;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public enum FlowState
    {
        Pause,
        Normal,
    }

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>

    public class SplashUIFlowView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public UnityAction onFlowStart;

        [HideInInspector]
        public UnityAction onFlowResume;

        [HideInInspector]
        public UnityAction onFlowPause;

        [HideInInspector]
        public UnityAction onFlowStop;

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

        private FlowState _currentState;
        private bool _isInitialized;
        private IContext _context;


        //  Initialization  -------------------------------

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
                _currentState = FlowState.Normal;

                Context.CommandManager.AddCommandListener<OnNetworkConnectChangeCommand>(Command_OnNetworkConnectionChange);
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
        private void OnEnable()
        {
            _flowController.onStart.AddListener(onFlowStart.Invoke);
            _flowController.onPause.AddListener(onFlowPause.Invoke);
            _flowController.onResume.AddListener(onFlowResume.Invoke);
            _flowController.onStop.AddListener(onFlowStop.Invoke);
        }

        private void OnDisable()
        {
            _flowController.onStart.RemoveListener(onFlowStart.Invoke);
            _flowController.onPause.RemoveListener(onFlowPause.Invoke);
            _flowController.onResume.RemoveListener(onFlowResume.Invoke);
            _flowController.onStop.RemoveListener(onFlowStop.Invoke);
        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        private void Command_OnNetworkConnectionChange(OnNetworkConnectChangeCommand command)
        {
            if (command.PreviousValue == command.CurrentValue)
            {
                return;
            }

            if (command.CurrentValue && _currentState == FlowState.Pause)
            {
                _currentState = FlowState.Normal;
                return;
            }

            if(!command.CurrentValue && _currentState == FlowState.Normal)
            {
            }
        }




    }

}