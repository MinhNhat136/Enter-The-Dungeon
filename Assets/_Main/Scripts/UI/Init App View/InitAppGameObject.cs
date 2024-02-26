using Atomic.Chain;
using Atomic.Command;
using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Core
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
    public interface IInitAppChain
    {
        public IInitAppChain NextChain { get; set; }
        public void Execute();
        public void ExecuteNextChain();

    }

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class InitAppGameObject : GameObjInitializableWithContext
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
      

        //  Fields ----------------------------------------
        [SerializeField]
        private InitAppView _view;

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                InitAppMini mini = new(_context, _view);
                mini.Initialize();
            }
        }

        public override void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Init App Game Object not initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
    }
}

