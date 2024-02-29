using Atomic.UI;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.Core
{
    public class AppTitleBaseObject : BaseGameObjectInitializableWithContext
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
      

        //  Fields ----------------------------------------


        [SerializeField]
        private AppTitleView _titleView;

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                AppTitleMini titleMini = new(_titleView, _context);
                titleMini.Initialize();
            }
        }

        public override void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("App title Game object not initialied");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------






    }
}

