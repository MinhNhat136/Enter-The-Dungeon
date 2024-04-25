using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Atomic.Equipment
{
    public class DamageConfigScriptableObject : MonoBehaviour, System.ICloneable
    {
        public MinMaxCurve DamageCurse;

        private void Reset()
        {
            DamageCurse.mode = ParticleSystemCurveMode.Curve;
        }

        public int GetDamage(float Distance = 0)
        {
            return Mathf.CeilToInt(DamageCurse.Evaluate(Distance, Random.value));
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
