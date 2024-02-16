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
    public class GuestSignInController : ISignInController, IController
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
        private readonly GuessSignInService _service;


        //  Initialization  -------------------------------
        public GuestSignInController(GuessSignInService service)
        {
            _service = service;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _service.OnSignInCompleted.AddListener(OnSignedInComplete);

                Context.CommandManager.AddCommandListener<OnGuestSignInCommand>((p) => StartSignInProcess());
                Context.CommandManager.AddCommandListener<OnSignUpCompleteCommand>((p) => StartSignInProcess());
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
        public void StartSignInProcess()
        {
            RequireIsInitialized();
            _service.SignIn();
        }

        public void OnSignedInComplete(UserProfileData userData, bool wasSuccessful)
        {
            Context.CommandManager.InvokeCommand(new SignInCompletionCommand(userData, wasSuccessful));
        }

        //  Event Handlers --------------------------------

    }
}


