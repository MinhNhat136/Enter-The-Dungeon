using Atomic.Chain;
using Atomic.Command;
using Atomic.Core.Interface;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System.Collections.Generic;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// CONTROLLER xử lý Logic Loading các Components và Features cần thiết khi vừa khởi động game
    /// </summary>
    public class InitAppController : IController
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
        private readonly InitAppView _view;
        private readonly List<IChain> _chains = new();

        //  Initialization  -------------------------------
        public InitAppController(InitAppView view)
        {
            _view = view;
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                Context.CommandManager.AddCommandListener<UserProfileValidateCompletionCommand>((p) =>
                {
                    if (p.WasSuccess)
                    {
                        _view.SendSignal_LoadLobbyScene();
                    }
                    else _view.SendSignal_ShowAppTitleView();
                });

                IChain loadingInitApp = new StartInitAppChain();
                IChain policyValidationChain = new PolicyValidateChain();
                IChain userProfileValidationChain = new UserProfileValidateChain();

                loadingInitApp.SetContext(_context).SetNextHandler(policyValidationChain);
                policyValidationChain.SetContext(_context).SetNextHandler(userProfileValidationChain);
                userProfileValidationChain.SetContext(_context);

                _chains.Add(loadingInitApp);
                _chains.Add(policyValidationChain);
                _chains.Add(userProfileValidationChain);

                _chains[0].Handle();
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Init App Controller not initialied");

            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------



    }
}

