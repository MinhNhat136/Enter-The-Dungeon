using Atomic.Controllers;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System;

namespace Atomic.UI
{
    public class TitleMini : IMiniMvcs
    {
        private readonly AppTitleView _view;

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        private bool _isInitialized;

        public TitleMini(AppTitleView view) 
        {
            _view = view;
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;

                Context context = new();

                SignInControllerBuilder builder = new();

                GuestSignInService signInWithGuessService = new();
                signInWithGuessService.Initialize(context);

                GuessSignInController signInWithGuessController = new(signInWithGuessService);
                signInWithGuessController.Initialize(context);

                GuestSignUpController signUpWithGuessController = new();

                builder.SetSignInWithGuessController(signInWithGuessController);
                _view.Initialize(context);

                AppTitleViewController _controller = new(builder, _view, signUpWithGuessController);
                _controller.Initialize(context);

            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new Exception("No instance of TitleModule");
            }
        }
    }
}

