using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    public class PenetrateAction : ActionOnHit
    {
        [SerializeField]
        private float chancePenetratePercentage;
        
        private bool _canPenetrate;

        public override void Initialize()
        {
            _canPenetrate = Random.Range(0, 100) < chancePenetratePercentage;
        }

        public override void OnHit(Vector3 point, Vector3 normal, Collider collide)
        {
            if (collide.TryGetComponent(out BaseAgent agent))
            {
                foreach (var effect in projectile.PassiveEffect)
                {
                    var effectApply = effect.Clone();
                    effectApply.Target = agent;
                    effectApply.Source = projectile.Owner;
                    
                    effectApply.Apply();
                }
                
                var vfx = projectile.HitVfx.Get();
                
                var vfxObject = vfx.gameObject;
                vfxObject.transform.position = projectile.transform.position;
                vfxObject.transform.forward = -projectile.transform.forward;
            }
            
            if (!_canPenetrate)
            {
                projectile.Release?.Invoke(projectile);
                projectile.PassiveEffect.Clear();
            }
            
        }
    }
    
}
