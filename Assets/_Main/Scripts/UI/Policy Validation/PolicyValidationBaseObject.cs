using Atomic.UI;
using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System;
using UnityEngine;

namespace Atomic.Core
{
    public class PolicyValidationBaseObject : BaseGameObjectInitializableWithContext
    {
        
        public override void Initialize(IContext context)
        {
            if(!IsInitialized)
            {
                _isInitialized = true;
                _context = context;

                PolicyValidationMini validationMini = new(_context);

                validationMini.Initialize();
            }
        }

        public override void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new Exception("Policy Game Object not initialize");
            }
        }

    }
}
