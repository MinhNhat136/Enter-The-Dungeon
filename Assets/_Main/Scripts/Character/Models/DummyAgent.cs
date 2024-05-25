using Atomic.Collection;
using UnityEngine;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------


    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class DummyAgent : EnemyAgent
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
      

        //  Fields ----------------------------------------

        [SerializeField] 
        private DeadBodyPool deadBodyPool; 

        //  Initialization  -------------------------------
        

        //  Unity Methods   -------------------------------
        public new void Awake()
        {
            Initialize();
            deadBodyPool.Initialize();
        }
        
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        
        public void DeadHandle()
        {
            var deadBody = deadBodyPool.objectPool.Get();
            deadBody.Setup(modelTransform.position, true);
            deadBody.Explode();
        }
    }
}