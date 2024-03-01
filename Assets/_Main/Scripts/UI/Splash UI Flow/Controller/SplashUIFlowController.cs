using Atomic.Command;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class SplashUIFlowController : IController
    {
        //  Events ----------------------------------------


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
        private bool _isInitialized;
        private IContext _context;
        private readonly SplashUIFlowView _view;

        //  Initialization  -------------------------------
        public SplashUIFlowController(SplashUIFlowView view)
        {
            _view = view;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                Context.CommandManager.AddCommandListener<OnNetworkConnectChangeCommand>(Command_OnNetworkConntectionChange);
                Context.CommandManager.AddCommandListener<UserProfileValidateCompletionCommand>(Command_UserProfileValidateCompleted);

                StartFlow();
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Splash UI Flow Controller not inititialize");
            }
        }


        //  Unity Methods   -------------------------------
        public void OnDestroy()
        {
            Context.CommandManager.RemoveCommandListener<OnNetworkConnectChangeCommand>(Command_OnNetworkConntectionChange);
            Context.CommandManager.RemoveCommandListener<UserProfileValidateCompletionCommand>(Command_UserProfileValidateCompleted);
        }

        //  Other Methods ---------------------------------
        private void StartFlow()
        {
            _view.StartFlow();
            Context.CommandManager.InvokeCommand(new OnUIFlowStartCommand());
        }

        //  Event Handlers --------------------------------
        private void Command_OnNetworkConntectionChange(OnNetworkConnectChangeCommand command)
        {
            if(command.CurrentValue == command.PreviousValue)
            {
                return;
            }

            if(command.CurrentValue == false)
            {
                _view.StopFlow();
                return;
            }
            _view.ResumeFlow();
            
        }

        private void Command_UserProfileValidateCompleted(UserProfileValidateCompletionCommand command)
        {
            if (command.WasSuccess)
            {
                _view.SendSignal_LoadLobbyScene();
            }
            else _view.SendSignal_ShowAppTitleView();
        }
    }

}
