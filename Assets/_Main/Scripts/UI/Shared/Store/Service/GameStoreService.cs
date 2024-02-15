using RMC.Core.Architectures.Mini.Service;
using UnityEngine;

namespace Atomic.Services
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: 
    /// </summary>
    public class GameStoreService : BaseService
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------
        private string gamePageUrl = "https://www.facebook.com/hominhnhat1362002";
        private string studioPageUrl = "https://www.google.com.vn/";

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------



        //  Other Methods ---------------------------------
        public void OpenGameRatePage()
        {
            OpenUrl(gamePageUrl);
        }

        public void OpenGameInfoPage()
        {
            OpenUrl(gamePageUrl);
        }

        public void OpenGameLikePage()
        {
            OpenUrl(studioPageUrl);
        }

        public void OpenStudioGamesPage()
        {
            OpenUrl(studioPageUrl);
        }
        private void OpenUrl(string url)
        {
            Application.OpenURL(url);
        }

    }
}
