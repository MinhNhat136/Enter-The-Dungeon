using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using System;
using System.Diagnostics;

namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// </summary>
    public class SignInWithGuessController : ISignInController, IController
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
        private readonly SignInWithGuessService _service;


        //  Initialization  -------------------------------
        public SignInWithGuessController(SignInWithGuessService service)
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

        public void OnSignedInComplete(UserData userData, bool wasSuccessful)
        {
            Context.CommandManager.InvokeCommand(new SignInCompletedCommand(userData, wasSuccessful));
        }

        //  Event Handlers --------------------------------

    }
}


