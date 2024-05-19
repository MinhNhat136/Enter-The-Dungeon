using UnityEngine;

namespace Atomic.Character
{
    [CreateAssetMenu(fileName = "Melee Attack Data", menuName = "Weapons/Melee/Attack Data")]
    public class MeleeAttackData : ScriptableObject
    {
        [field: SerializeField] 
        public string AnimationName { get; private set; }

        [field: SerializeField]
        public float EnergyValue { get; private set; }
        
        [field: SerializeField]
        public float AttackMoveDistance { get; private set; }

        [field: SerializeField]
        public float AttackMoveSpeedWeight{ get; private set; }
        
        [field: SerializeField]
        public float AttackMoveAccelerationWeight{ get; private set; }
        
        [field: SerializeField]
        public float DelayResetCombo{ get; private set; }
    }
}
