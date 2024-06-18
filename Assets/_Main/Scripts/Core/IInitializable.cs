namespace Atomic.Core.Interface
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IInitializable
    {
        //  Properties  ------------------------------------
        public bool IsInitialized { get; }

        //  General Methods  ------------------------------
        public void Initialize();
        public void DoEnable() { }
        public void DoDisable() { } 
        public void DoDestroy() { }
        void RequireIsInitialized();
    }
}