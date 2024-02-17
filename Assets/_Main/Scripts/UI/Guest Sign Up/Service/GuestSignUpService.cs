using Atomic.Models;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine.Events;

namespace Atomic.Services
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class GuestSignUpService : IService
    {
        //  Events ----------------------------------------
        public readonly UnityEvent<UserProfileData> OnSignUpCompletedUnityEvent = new();

        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public IContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;
        private IContext _context;



        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("GuestSignUpService not yet initialized");
            }
        }



        //  Other Methods ---------------------------------
        public void SignUp(string username)
        {
            var userProfileData = new UserProfileData(username);
            ES3.Save<UserProfileData>(GameDataKey.UserProfileData, userProfileData);
            OnSignUpCompletedUnityEvent.Invoke(userProfileData);
        }

    }
}

