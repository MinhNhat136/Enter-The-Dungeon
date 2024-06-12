using Atomic.AbilitySystem;
using CBS.Example;
using CBS.Models;
using UnityEngine;

namespace CBS
{
    public enum ERarity
    {
        Common = 0, 
        Great = 1,
        Rare = 2,
        Epic = 3, 
        Legendary = 4,
    }
    
    public class AtomicItemCustomData : CBSItemCustomData
    {
        public string id;
        public ERarity rarity;
        public GameplayEffectModifier[] modifiers;
        private ICBSItems ItemModule { get; set; }
        private EquipmentAttributeBuilder _builder;

        public void Initialize()
        {
            ItemModule = CBSModule.Get<CBSItemsModule>();

            ItemModule.GetCBSItemByID(id, (result) =>
            {
                var item = result.Item;
                _builder = item.GetLinkedScriptable<EquipmentAttributeBuilder>();
            });
            
            modifiers = _builder.Build((int)rarity);
        }

        public void Equip()
        {
            
        }

        public void UnEquip()
        {
            
        }
        

        private void Start()
        {
            
        }
        
    }
}

