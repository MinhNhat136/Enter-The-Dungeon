using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Atomic.Damage
{
    public abstract class DamagePassiveEffectSo : ScriptableObject
    {
        public float chance;
        public float chanceBonus; 
        
        public float interval;
        public float intervalBonus;

        [FormerlySerializedAs("damagePerTick")] public float damage;
        [FormerlySerializedAs("damagePerTickBonus")] public float damageBonus;

        public float tick;
        public float tickBonus;

        public EffectPopupAnimation popupPrefab;
        public ObjectPool<EffectPopupAnimation> popupPool; 

        public BaseAgent Damager { get; set; }
        public abstract PassiveEffect CreatePassiveEffect();

        public void CreatePopupPool()
        {
            popupPool = new ObjectPool<EffectPopupAnimation>(CreateInstance , OnGetPopup, OnReleasePopup, OnDestroyPopup, true, 5, 10);
        }
        
        private EffectPopupAnimation CreateInstance()
        {
            EffectPopupAnimation popupInstance = Instantiate(popupPrefab);
            return popupInstance;
        }
            
        private void OnGetPopup(EffectPopupAnimation popup)
        {
        }

        private void OnReleasePopup(EffectPopupAnimation popup)
        {
            popup.gameObject.SetActive(false);
            popup.EndAnimation();
        }

        private void OnDestroyPopup(EffectPopupAnimation popup)
        {
            Destroy(popup);
        }
        
    }
    
}
