using Atomic.Template;
using Atomic.UI;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Atomic.Core
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class NetworkStatusBaseObject : BaseGameObjectInitializableWithContext
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------


        //  Fields ----------------------------------------


        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = false;
                _context = context;

                NetworkStatusMini mini = new();
                mini.Initialize(_context);

            }
        }

        public override void RequireIsInitialized()
        {
        }
    }
}

