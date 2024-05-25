using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    public class NormalAction : ActionOnHit
    {
        public override void Initialize()
        {
        }

        public override void OnHit(Vector3 point, Vector3 normal, Collider collide)
        {
            var vfx = projectile.HitVfx.Get();
            var vfxObject = vfx.gameObject;
            vfxObject.transform.position = projectile.MyTransform.position;
            vfxObject.transform.forward = -projectile.MyTransform.forward;
            
            if (!collide)
            {
                projectile.Release?.Invoke(projectile);
                projectile.PassiveEffect.Clear();
                return;
            }
            var agent = collide.GetComponentInParent<BaseAgent>();
            if (agent)
            {
                foreach (var effect in projectile.PassiveEffect)
                {
                    var effectApply = effect.Clone();
                    effectApply.Target = agent;
                    effectApply.Source = projectile.Owner;

                    effectApply.Apply();
                }

                agent.ImpactSensorController.ImpactValue = projectile.ForceWeight.GetValueFromRatio(projectile.EnergyValue);
                agent.ImpactSensorController.ImpactDirection =
                    (agent.transform.position - projectile.ShootPosition).normalized;
                agent.ImpactSensorController.Impact();
            }
            projectile.Release?.Invoke(projectile);
            projectile.PassiveEffect.Clear();
        }
    }
    
}
