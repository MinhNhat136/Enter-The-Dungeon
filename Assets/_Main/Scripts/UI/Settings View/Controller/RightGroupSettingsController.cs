using Atomic.Services;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;


namespace Atomic.Template
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO:.
    /// </summary>
    public class RightGroupSettingsController : IController
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
        private RightGroupSettingsView _view;

        private GameStoreController _gameStoreController; 

        //  Initialization  -------------------------------
        public RightGroupSettingsController(RightGroupSettingsView view)
        {
            _view = view;
            _gameStoreController = new GameStoreController(new GameStoreService());
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = false;
                _context = context;

                _view.OnClickedButtonLanguageUnityEvent.AddListener(View_OnClickedButtonLanguge);
                _view.OnClickedButtonUserIDUnityEvent.AddListener(View_OnClickedButtonUserID);
                _view.OnClickedButtonLikeUnityEvent.AddListener(View_OnClickedButtonLike);
                _view.OnClickedButtonRateUnityEvent.AddListener(View_OnClickedButtonRate);
                _view.OnClickeedButtonAboutUnityEvent.AddListener(View_OnClickedButtonAbout);
                _view.OnClickedButtonOtherGamesUnityEvent.AddListener(View_OnClickedButtonOtherGames);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Right group settings CONTROLLER not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        private void View_OnClickedButtonLanguge()
        {
        }

        private void View_OnClickedButtonUserID()
        {

        }

        private void View_OnClickedButtonLike()
        {
            _gameStoreController.OnOpenGameLikePage();
        }

        private void View_OnClickedButtonRate()
        {
            _gameStoreController.OnOpenGameLikePage();
        }

        private void View_OnClickedButtonAbout()
        {
            _gameStoreController.OnOpenGameInfoPage();
        }

        private void View_OnClickedButtonOtherGames()
        {
            _gameStoreController.OnOpenStudioGamesPage();
        }
    }
}


