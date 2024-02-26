using Atomic.Command;
using Atomic.Models;
using Atomic.Services;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System;

namespace Atomic.Controllers
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class UserProfileFormFillController : IController
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

        private readonly UserProfileFormFillView _view;
        private readonly UserProfileFormFillService _service;

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public UserProfileFormFillController(UserProfileFormFillView view, UserProfileFormFillService service)
        {
            _view = view;
            _service = service;
        }

        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                _view.OnClickButtonOkUnityEvent.AddListener(View_OnClickedButtonOk);
                _service.OnSignUpCompletedUnityEvent.AddListener(Service_OnFillCompletedUnityEvent);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("No instance of Sign Up with Guess Controller");
            }
        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------

        private void View_OnClickedButtonOk(string username)
        {
            _service.SignUp(username);
        }

        private void Service_OnFillCompletedUnityEvent(UserProfileData userProfileData)
        {
            Context.CommandManager.InvokeCommand(new OnFormFillCompleteCommand(userProfileData));
        }
    }
}

