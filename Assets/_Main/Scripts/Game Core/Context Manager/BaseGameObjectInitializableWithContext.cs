using RMC.Core.Architectures.Mini.Context;
using UnityEngine;

namespace Atomic.Core
{
    public abstract class BaseGameObjectInitializableWithContext : MonoBehaviour, IInitializableWithContext
    {
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
        protected bool _isInitialized;
        protected IContext _context;

        public abstract void Initialize(IContext context);

        public abstract void RequireIsInitialized();
    }
}