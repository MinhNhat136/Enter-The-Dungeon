using Atomic.Command;
using Atomic.Models;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// </summary>
    public class UserProfileValidateController : IController
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
        private readonly UserProfileValidateService _service;


        //  Initialization  -------------------------------
        public UserProfileValidateController(UserProfileValidateService service)
        {
            _service = service;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _service.OnValidateCompleted.AddListener(OnValidateComplete);

                Context.CommandManager.AddCommandListener<UserProfileValidateCommand>((p) => StartValidate());
                Context.CommandManager.AddCommandListener<OnFormFillCompleteCommand>((p) => StartValidate());
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("No instance of SignInWithGuessController");
            }
        }

        //  Unity Methods   -------------------------------



        //  Other Methods ---------------------------------
        public void StartValidate()
        {
            RequireIsInitialized();
            _service.UserProfileValidate();
        }

        public void OnValidateComplete(UserProfileData userData, bool wasSuccessful)
        {
            Context.CommandManager.InvokeCommand(new UserProfileValidateCompletionCommand(userData, wasSuccessful));
        }

        //  Event Handlers --------------------------------

    }
}


