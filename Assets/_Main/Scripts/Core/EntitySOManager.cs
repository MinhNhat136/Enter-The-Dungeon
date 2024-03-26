using Atomic.Core.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Core
{
    [CreateAssetMenu]
    public class EntitySOManager : ScriptableObject
    {
        private List<IEntity> entities = new();
        private List<IInitializable> _initializables = new();
        private List<IDoEnable> _doEnables = new();
        private List<ITickable> _tickables = new();
        private List<ICleanUp> _cleanUps = new();

        public void RegistryInitialize(IEntity entity)
        {

        }

        public void UnRegistryInitialize(IEntity entity) 
        {
            
        }

        public void RegistryDoEnable() { }

        public void UnRegistryDoEnable() { }

        public void RegistryTick() { }
        public void UnRegistryTick() { }    

        public void Initialize()
        {

        }

        public void DoEnable()
        {

        }

        public void DoDisable()
        {

        }

        public void Tick()
        {

        }

        public void CleanUp()
        {

        }
    }

}
