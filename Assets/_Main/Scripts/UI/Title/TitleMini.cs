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

                AppTitleViewController _controller = new(_view);

                _view.Initialize(context);
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

