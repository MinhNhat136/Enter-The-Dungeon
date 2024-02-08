using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;


namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Controller coordinates everything between
    /// the <see cref="IConcern"/>s and contains the core app logic 
    /// </summary>
    public class TitleViewController : BaseController<TitleViewModel, TitleView, TitleViewService>
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------

        //  Fields ----------------------------------------

        //  Initialization  -------------------------------
        public TitleViewController(TitleViewModel model, TitleView view, TitleViewService service)
            : base(model, view, service)
        {
        }

        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                _view.CheckPlayFirstTimeUnityEvent.AddListener(CheckPlayFirstTime);
               
            }
        }
        
        //  Methods ---------------------------------------
        private void CheckPlayFirstTime()
        {
            _service.IsPlayFirstTime();

            Context.CommandManager.InvokeCommand(new ShowFormFillUserDataCommand());
        }

        //  Event Handlers --------------------------------

    }
}

