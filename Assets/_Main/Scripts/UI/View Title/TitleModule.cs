using RMC.Core.Architectures.Mini;
using RMC.Core.Architectures.Mini.Context;


namespace Atomic.UI
{
    public class TitleModule : MiniMvcs<Context, TitleViewModel, TitleView, TitleViewController, TitleViewService>
    {
        public TitleModule(TitleView view)
        {
            _view = view;

        }

        public override void Initialize()
        {
            if (!IsInitialized)
            {
                _isInitialized = true;

                //
                _context = new Context();
                _model = new TitleViewModel();
                _service = new TitleViewService();
                _controller = new TitleViewController(_model, _view, _service);

                //
                _model.Initialize(_context);
                _view.Initialize(_context);
                _service.Initialize(_context);
                _controller.Initialize(_context);
            }
        }
    }
}

