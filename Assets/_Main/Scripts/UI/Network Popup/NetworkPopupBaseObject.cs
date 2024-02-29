using Atomic.UI;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.Core
{
    public class NetworkPopupBaseObject : BaseGameObjectInitializableWithContext
    {
        [SerializeField]
        private UIPopup _popup;

        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = false;
                _context = context;

                NetworkPopupMini module = new(_popup, new Context());
                module.Initialize(_context);

            }
        }

        public override void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("Network popup game object not Initalized");
            }
        }

        
    }
}

