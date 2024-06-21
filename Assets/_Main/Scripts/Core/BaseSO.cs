using UnityEngine;

namespace Atomic.Core
{
    public class BaseSo : ScriptableObject, IInitializable 
    {
        public bool IsInitialized { get; set; }
        
        public virtual void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;
        }

        public void RequireIsInitialized()
        {
        }

        public virtual void Reset()
        {
            IsInitialized = false;
        }
    }
    
}
