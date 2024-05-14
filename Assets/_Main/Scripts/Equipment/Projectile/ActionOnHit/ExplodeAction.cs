using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    public class ExplodeAction : ActionOnHit
    {
        public float areaOfEffectDistance = 5f;
        public LayerMask impactLayers;

        private readonly Collider[] _affectedColliders = new Collider[32];
        private readonly BaseAgent[] _affectedAgents = new BaseAgent[16];
        private Vector3 _centerPoint; 
        
        public override void Initialize()
        {
            
        }

        public override void OnHit(Vector3 point, Vector3 normal, Collider collide)
        {
            ScanDamageArea(point);
            Explode();
            AfterExplode();
        }
        
        private void ScanDamageArea(Vector3 center)
        {
            Physics.OverlapSphereNonAlloc(center, areaOfEffectDistance, _affectedColliders, impactLayers);
            _centerPoint = center;
            
            var currentAgentIndex = 0; 
            foreach (var coll in _affectedColliders)
            {
                if(!coll) continue;
                if (coll.gameObject == projectile.Owner.gameObject) continue;
                if (coll.TryGetComponent(out BaseAgent agent) && currentAgentIndex < _affectedAgents.Length)
                {
                    _affectedAgents[currentAgentIndex] = agent;
                    currentAgentIndex++;
                }
            }
        }

        private void Explode()
        {
            for (int index = 0; index < _affectedAgents.Length; index++)
            {
                if(!_affectedAgents[index]) continue;
                foreach (var effect in projectile.PassiveEffect)
                {
                    
                    var effectApply = effect.Clone();
                    effectApply.Target = _affectedAgents[index];
                    effectApply.Source = projectile.Owner;
                    
                    effectApply.Apply();
                }
            }
            var vfx = projectile.HitVfx.Get();
                
            var vfxObject = vfx.gameObject;
            vfxObject.transform.position = _centerPoint;
        }

        private void AfterExplode()
        {
            for (int index = 0; index < _affectedAgents.Length; index++)
            {
                _affectedAgents[index] = null;
            }
        }
        
    }
}