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
    public class GuessSignInController : ISignInController, IController
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
        private readonly GuestSignInService _service;


        //  Initialization  -------------------------------
        public GuessSignInController(GuestSignInService service)
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


