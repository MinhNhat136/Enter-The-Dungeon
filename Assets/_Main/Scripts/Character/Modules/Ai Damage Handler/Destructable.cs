using UnityEngine;

namespace Atomic.Character.Module
{
    public class Destructable : MonoBehaviour
    {
        AiHealth m_Health;

        void Start()
        {
            m_Health = GetComponent<AiHealth>();

            m_Health.OnZero += OnDie;
            m_Health.OnDecreased += OnDamaged;
        }

        void OnDamaged(float damage, GameObject damageSource)
        {
        }

        void OnDie()
        {
        }
    }
}