using Atomic.Command;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;


namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class AppTitleViewController : IController
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
        private readonly AppTitleView _view;

        //  Initialization  -------------------------------
        public AppTitleViewController(AppTitleView view)
        {
            _view = view;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _view.SignInWithGuessUnityEvent.AddListener(View_OnSignInWithGuess);
                _view.SignInWithGameCenterUnityEvent.AddListener(View_OnSignInWithGameCenter);
                _view.SignInWithFacebookUnityEvent.AddListener(View_OnSignInWithFacebook);
                _view.SignInWithGoogleUnityEvent.AddListener(View_OnSignInWithGoogle);
            }
        }

        public void RequireIsInitialized()
        {
            if(!_isInitialized)
            {
                throw new Exception("Title View Controller MustBeInitialized");
            }
        }

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
        private void View_OnSignInWithGuess()
        {
            Context.CommandManager.InvokeCommand(new OnGuestSignInCommand());
        }

        private void View_OnSignInWithGameCenter()
        {
            UnityEngine.Debug.Log("Sign In With Guess");

        }

        private void View_OnSignInWithFacebook()
        {
            UnityEngine.Debug.Log("Sign In With Guess");
        }

        private void View_OnSignInWithGoogle()
        {
            UnityEngine.Debug.Log("Sign In With Guess");
        }

    }
}

