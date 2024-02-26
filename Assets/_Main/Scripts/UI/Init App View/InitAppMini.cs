using Atomic.Controllers;
using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class InitAppMini : IMiniMvcs
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        //  Fields ----------------------------------------
        private bool _isInitialized;
        private readonly IContext _context;
        private readonly InitAppView _view;


        //  Initialization  -------------------------------
        public InitAppMini(IContext context, InitAppView view)
        {
            _context = context;
            _view = view;
        }

        public void Initialize()
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                InitAppController controller = new(_view);

                _view.Initialize(_context);
                controller.Initialize(_context);
            }
        }

        public void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Init App Mini not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------


    }
}


