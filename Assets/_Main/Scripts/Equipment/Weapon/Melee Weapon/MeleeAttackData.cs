using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character
{
    [CreateAssetMenu(fileName = "Melee Attack Data", menuName = "Weapons/Melee/Attack Data")]
    public class MeleeAttackData : ScriptableObject
    {
        [field: SerializeField] 
        public string AnimationName { get; private set; }

        public float energy;
        public float attackMoveDistance;
        public float attackMoveSpeedWeight;
        public float attackMoveAccelerationWeight;
        
        public float delayResetCombo;
        
    }
    
}
