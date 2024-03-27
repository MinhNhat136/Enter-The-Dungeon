using UnityEngine;

namespace Atomic.Character.Module
{
    public class Damageable : MonoBehaviour
    {

        public float SensibilityToSelfdamage = 0.5f;

        public AiHealth Health { get; private set; }

        void Awake()
        {
            Health = GetComponent<AiHealth>();
            if (!Health)
            {
                Health = GetComponentInParent<AiHealth>();
            }
        }

        public void InflictDamage(float damage, GameObject damageSource)
        {
            if (Health)
            {
                var totalDamage = damage;

                Health.Decrease(totalDamage, damageSource);
            }
        }
    }
}