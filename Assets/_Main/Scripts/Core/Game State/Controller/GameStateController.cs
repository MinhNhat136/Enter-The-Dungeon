using Atomic.Core;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Controller;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Controllers.Core
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class GameStateController : MonoBehaviour, IController
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

        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------



    }
}

