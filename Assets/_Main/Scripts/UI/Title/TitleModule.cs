using RMC.Core.Architectures.Mini.Context;
using System;

namespace Atomic.UI
{
    public class TitleModule : IMiniMvcs
    {
        private TitleView _view; 

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        private bool _isInitialized;

        public TitleModule(TitleView view) 
        {
            _view = view;
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;

                Context context = new Context();

                SignInControllerBuilder builder = new SignInControllerBuilder();

                SignInWithGuessService signInWithGuessService = new();
                signInWithGuessService.Initialize(context);

                SignInWithGuessController signInWithGuessController = new(signInWithGuessService);
                signInWithGuessController.Initialize(context);

                SignUpWithGuessController signUpWithGuessController = new();

                builder.SetSignInWithGuessController(signInWithGuessController);
                _view.Initialize(context);

                TitleViewController _controller = new(builder, _view, signUpWithGuessController);
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

