using Atomic.UI;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System;
using UnityEngine;

namespace Atomic.Core
{
    public class PolicyDisplayGameObject : GameObjInitializableWithContext
    {
        [SerializeField]
        private UIPopup _policyPopup;


        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                PolicyDisplayMini mini = new(_policyPopup,_context);

                mini.Initialize();
            }
        }

        public override void RequireIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("Policy Game Object not initialize");
            }
        }
    }
}