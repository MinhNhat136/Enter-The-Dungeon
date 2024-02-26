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

                _view.SignUpWithGuessUnityEvent.AddListener(View_OnSignUpWithGuess);
                _view.SignUpWithGameCenterUnityEvent.AddListener(View_OnSignUpWithGameCenter);
                _view.SignUpWithFacebookUnityEvent.AddListener(View_OnSignUpWithFacebook);
                _view.SignUpWithGoogleUnityEvent.AddListener(View_OnSignUpWithGoogle);
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
        private void View_OnSignUpWithGuess()
        {
            Context.CommandManager.InvokeCommand(new OnFillFormCommand());
        }

        private void View_OnSignUpWithGameCenter()
        {
            UnityEngine.Debug.Log("Sign In With Guess");

        }

        private void View_OnSignUpWithFacebook()
        {
            UnityEngine.Debug.Log("Sign In With Guess");
        }

        private void View_OnSignUpWithGoogle()
        {
            UnityEngine.Debug.Log("Sign In With Guess");
        }

    }
}

