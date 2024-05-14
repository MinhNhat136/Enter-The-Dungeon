using Atomic.Character;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.Damage
{
    public abstract class DamagePassiveEffectSo : ScriptableObject
    {
        public float chance;
        public float chanceBonus; 
        
        public float interval;
        public float intervalBonus;

        public float damage;
        public float damageBonus;

        public float tick;
        public float tickBonus;

        public EffectPopupAnimation popupPrefab;
        protected ObjectPool<EffectPopupAnimation> popupPool; 

        public BaseAgent Damager { get; set; }
        public abstract PassiveEffect CreatePassiveEffect();

        public void CreatePopupPool()
        {
            popupPool = new ObjectPool<EffectPopupAnimation>(CreateInstance , OnGetPopup, OnReleasePopup, OnDestroyPopup, true, 5, 10);
        }
        
        private EffectPopupAnimation CreateInstance()
        {
            EffectPopupAnimation popupInstance = Instantiate(popupPrefab);
            popupInstance.myPool = popupPool;
            return popupInstance;
        }
            
        private void OnGetPopup(EffectPopupAnimation popup)
        {
        }

        private void OnReleasePopup(EffectPopupAnimation popup)
        {
            popup.gameObject.SetActive(false);
        }

        private void OnDestroyPopup(EffectPopupAnimation popup)
        {
            Destroy(popup);
        }
        
    }
    
}
