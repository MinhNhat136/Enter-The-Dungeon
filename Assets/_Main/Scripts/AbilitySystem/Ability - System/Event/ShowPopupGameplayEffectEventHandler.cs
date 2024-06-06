using System.Linq;
using Atomic.Damage;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Ability Event Handler/Popup Effect")]
    public class ShowPopupGameplayEffectEventHandler : AbstractApplyGameplayEffectEventHandler
    {
        public AttributeScriptableObject[] attributeCanShow;
        
        [SerializeField] private EffectPopupAnimation popupAnimation;

        [SerializeField] private float offsetTime;
        [SerializeField] private float popupDelay;

        private ObjectPool<EffectPopupAnimation> _popupAnimationPools;

        public override void Initialize()
        {
            base.Initialize();
            CreatePool();
        }

        public override void Reset()
        {
            base.Reset();
            _popupAnimationPools.Clear();
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
            Destroy(popup);
        }

        public override void PreApplyEffectSpec(GameplayEffectSpec effectSpec)
        {
            float multiplierHorizontalDirectionValue = Vector3.Dot(Vector3.right,
                effectSpec.Target.transform.position - effectSpec.Source.transform.position) > 0 ? 1 : -1;

            float multiplierVerticalDirectionValue = Vector3.Dot(Vector3.forward,
                effectSpec.Target.transform.position - effectSpec.Source.transform.position) > 0 ? 1 : -1;

            var currentIndex = 0;
            for (var index = 0; index < effectSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers.Length; index++)
            {
                if (!attributeCanShow.Contains(effectSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers[index]
                        .attribute)) return;
                
                var damageNumber = _popupAnimationPools.Get();
                damageNumber.SetPosition(effectSpec.Target.transform.position)
                    .SetShowValue(effectSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers[index].multiplier)
                    .SetDescription("")
                    .SetColor(effectSpec.GameplayEffectScriptableObject.gameplayEffect.modifiers[index].attribute.color)
                    .SetHorizontalDirection(multiplierHorizontalDirectionValue)
                    .SetVerticalDirection(multiplierVerticalDirectionValue)
                    .SetTimeDelay(offsetTime * currentIndex + popupDelay * effectSpec.Index)
                    .Play();
                currentIndex++;
            }
        }
    }
}