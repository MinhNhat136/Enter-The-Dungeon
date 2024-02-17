using Atomic.Controllers;
using RMC.Core.Architectures.Mini.Context;
using System;

namespace Atomic.UI
{
    public class AppTitleMini : IMiniMvcs
    {
        private readonly AppTitleView _view;

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        private bool _isInitialized;
        private IContext _context;

        public AppTitleMini(AppTitleView view) 
        {
            _view = view;
        }

        public IContext Context 
        { 
            get { return _context; } 
            set { _context = value; }
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                AppTitleViewController _controller = new(_view);

                RequireContext();

                _view.Initialize(_context);
                _controller.Initialize(_context);
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new Exception("No instance of TitleModule");
            }
        }

        public void RequireContext()
        {
            if(Context == null)
            {
                throw new Exception("App Title Mini not have Context");
            }
        }
    }
}

