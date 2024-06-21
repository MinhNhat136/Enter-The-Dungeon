using System;
using System.Collections.Generic;
using Atomic.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Attribute Event Handler/Damage Numbers")]
    public class ShowValueChangesEventHandler : AbstractAttributeEventHandler
    {
        [SerializeField]
        private AttributeScriptableObject primaryAttribute;

        [SerializeField]
        private EffectPopupAnimation popupAnimation;

        [SerializeField] 
        private bool isAbsolute;
        
        private ObjectPool<EffectPopupAnimation> _popupAnimationPools;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Reset();
        }

        public override void Initialize()
        {
            base.Initialize();
            CreatePool();
        }

        public override void Reset()
        {
            _popupAnimationPools.Clear(); 
        }
        
        public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
        {
            var attributeCacheDict = attributeSystem.MAttributeIndexCache;
            if (attributeCacheDict.TryGetValue(primaryAttribute, out var primaryAttributeIndex))
            {
                var prevValue = prevAttributeValues[primaryAttributeIndex].currentValue;
                var currentValue = currentAttributeValues[primaryAttributeIndex].currentValue;

                if (Math.Abs(prevValue - currentValue) > 0.01f)
                {
                    var damageNumber = _popupAnimationPools.Get();
                    damageNumber
                        .SetPosition(attributeSystem.transform.position)
                        .SetShowValue(currentValue - prevValue)
                        .SetDescription("")
                        .SetTimeDelay(0)
                        .Play();
                }
            }
        }
        
        private void CreatePool()
        {
            _popupAnimationPools = new ObjectPool<EffectPopupAnimation>
            (
                CreatePopup,
                OnGetPopup,
                OnReleasePopup,
                OnDestroyPopup,
                true, 5, 10);
        }
        
        private EffectPopupAnimation CreatePopup()
        {
            EffectPopupAnimation popupInstance = Instantiate(popupAnimation);
            popupInstance.gameObject.SetActive(false);
            popupInstance.MyPools = _popupAnimationPools;
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
            Destroy(popupAnimation);
        }
    }
    
}
