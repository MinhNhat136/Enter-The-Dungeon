using Atomic.Core.Interface;
using Sirenix.OdinInspector;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------
   
    
    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiStateManager : SerializedMonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        
        public bool IsInitialized { get; private set; }
        public BaseAgent Model { get; private set; }

        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------

        

       
        
        //  Event Handlers --------------------------------
        
    }
    
}
