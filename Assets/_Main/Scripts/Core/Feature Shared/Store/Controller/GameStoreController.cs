using Atomic.Services;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;

namespace Atomic.Template
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: 
    /// </summary>
    public class GameStoreController : IController
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
        private GameStoreService _service;

        //  Initialization  -------------------------------
        public GameStoreController(GameStoreService service)
        {
            _service = service; 
        }

        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Game Store Controller not initialized");
            }
        }

        //  Other Methods ---------------------------------
        public void OnOpenGameLikePage()
        {
            _service.OpenGameLikePage();
        }

        public void OnOpenGameInfoPage()
        {
            _service.OpenGameInfoPage();
        }

        public void OnOpenGameRatePage() 
        { 
            _service.OpenGameRatePage();
        }
        
        public void OnOpenStudioGamesPage()
        {
            _service.OpenStudioGamesPage();
        }


    }
}
