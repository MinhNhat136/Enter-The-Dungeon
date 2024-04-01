namespace Atomic.Core.Interface
{
    public interface IInitializableWithBaseModel<TModel>
    {
        //  Properties  ------------------------------------
        public bool IsInitialized { get; }
        public TModel Model { get; }
        //  General Methods  ------------------------------
        public void Initialize(TModel model);
        void RequireIsInitialized();
    }

}
