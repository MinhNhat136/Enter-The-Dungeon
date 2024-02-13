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

        private readonly ISignInController _signInWithGuessController;
        private readonly ISignInController _signInWithGameCenterController;
        private readonly ISignInController _signInWithFacebookController;
        private readonly ISignInController _signInWithGoogleController;

        private readonly GuestSignUpController _signUpWithGuessController; 

        //  Initialization  -------------------------------
        public AppTitleViewController(SignInControllerBuilder builder, AppTitleView view, GuestSignUpController signUpWithGuessController)
        {
            _signInWithGuessController = builder.GetSignInController(SignInType.Guess);
            /*_signInWithGameCenterController = builder.GetSignInController(SignInType.GameCenter);
            _signInWithFacebookController = builder.GetSignInController(SignInType.Facebook);
            _signInWithGoogleController = builder.GetSignInController(SignInType.Google);*/

            _view = view;
            _signUpWithGuessController = signUpWithGuessController;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _view.SignInWithGuessUnityEvent.AddListener(View_OnSignInWithGuess);
                /*_view.SignInWithGameCenterUnityEvent.AddListener(View_OnSignInWithGameCenter);
                _view.SignInWithFacebookUnityEvent.AddListener(View_OnSignInWithFacebook);
                _view.SignInWithGoogleUnityEvent.AddListener(View_OnSignInWithGoogle);*/
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
            _signInWithGuessController.StartSignInProcess();
        }

        private void View_OnSignInWithGameCenter()
        {
            _signInWithGameCenterController.StartSignInProcess();
        }

        private void View_OnSignInWithFacebook()
        {
            _signInWithFacebookController.StartSignInProcess();
        }

        private void View_OnSignInWithGoogle()
        {
            _signInWithGoogleController.StartSignInProcess();
        }

    }
}

