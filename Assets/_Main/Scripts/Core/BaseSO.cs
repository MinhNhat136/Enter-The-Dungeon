using Atomic.Core.Interface;
using UnityEngine;

namespace Atomic.Core
{
    [CreateAssetMenu(fileName = "Base", menuName = "BaseSO")]
    public abstract class BaseSO : ScriptableObject, IEntity
    {
        private bool _isInitialized;
        private bool _isCleanUp;

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public bool IsCleanUp
        {
            get { return _isCleanUp; }
        }

        public virtual void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

            }
        }

        public virtual void DoEnable()
        {
        }

        public virtual void DoDisable()
        {
        }

        public virtual void RequireIsInitialized()
        {
            if(!_isInitialized) 
            {
                throw new System.Exception("BaseSO not initialized yet");
            }
        }

        public virtual void Tick()
        {
        }

        public virtual void CleanUp()
        {
        }

        
    }

}
