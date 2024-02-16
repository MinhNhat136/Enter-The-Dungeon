using Atomic.Controllers;
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
        private IContext _context; 

        public TitleMini(AppTitleView view) 
        {
            _view = view;
        }

        public IContext Context { get { return _context; } }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;

                _context = new Context();

                AppTitleViewController _controller = new(_view);

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
    }
}

