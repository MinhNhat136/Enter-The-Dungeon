using System.Collections.Generic;
using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    public class PenetrateAction : ActionOnHit
    {
        [SerializeField] private float chancePenetratePercentage;

        private bool _canPenetrate;
        private List<BaseAgent> _hitAgents = new(8);

        public override void Initialize()
        {
            _canPenetrate = Random.Range(0, 100) < chancePenetratePercentage;
            _hitAgents.Clear();
        }

        public override void OnHit(Vector3 point, Vector3 normal, Collider collide)
        {
            BaseAgent agent = collide.GetComponentInParent<BaseAgent>();

            if (!_hitAgents.Contains(agent))
            {
                _hitAgents.Add(agent);
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
                var vfx = projectile.HitVfx.Get();

                var vfxObject = vfx.gameObject;
                vfxObject.transform.position = projectile.MyTransform.position;
                vfxObject.transform.forward = -projectile.MyTransform.forward;
            }
            

            if (!_canPenetrate)
            {
                projectile.Release?.Invoke(projectile);
                projectile.PassiveEffect.Clear();
            }
        }
    }
}