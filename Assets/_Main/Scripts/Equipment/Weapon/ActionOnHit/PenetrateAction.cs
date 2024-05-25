using System.Collections.Generic;
using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    public class PenetrateAction : ActionOnHit
    {
        [SerializeField] private float chancePenetratePercentage;

        private bool _canPenetrate;
        
        /// <summary>
        /// An agent can have multiple colliders, 
        /// and we want the projectile to apply damage only on the first collision.
        /// Therefore, we use a list of agents to check if the projectile has already applied damage to an agent.
        /// </summary>
        private readonly List<BaseAgent> _hitAgents = new(8);

        public override void Initialize()
        {
            _canPenetrate = Random.Range(0, 100) < chancePenetratePercentage;
            _hitAgents.Clear();
        }

        public override void OnHit(Vector3 point, Vector3 normal, Collider collide)
        {
            var agent = collide.GetComponentInParent<BaseAgent>();
            if (!agent)
            {
                var vfx = projectile.HitVfx.Get();

                var vfxObject = vfx.gameObject;
                vfxObject.transform.position = projectile.MyTransform.position;
                vfxObject.transform.forward = -projectile.MyTransform.forward;
                
                projectile.Release?.Invoke(projectile);
                projectile.PassiveEffect.Clear();
                return;
            }
            
            if (!_hitAgents.Contains(agent))
            {
                _hitAgents.Add(agent);
                for (var index = 0; index < projectile.PassiveEffect.Count; index++)
                {
                    var effect = projectile.PassiveEffect[index];
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

            if (_canPenetrate) return;
            projectile.Release?.Invoke(projectile);
            projectile.PassiveEffect.Clear();
        }
    }
}