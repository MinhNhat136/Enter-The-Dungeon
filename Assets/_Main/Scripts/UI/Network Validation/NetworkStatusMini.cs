using Atomic.Controllers;
using Atomic.Models;
using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Atomic.Template
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class NetworkStatusMini : IInitializableWithContext
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

        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                NetworkStateModel _model = new();
                NetworkStatusController _checker = new(_model);

                _model.Initialize(_context);
                _checker.Initialize(_context);
            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new System.Exception("Network Status Mini not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------



    }
}

