using Atomic.Core;
using UnityEngine;

namespace Atomic.Character.Config
{
    [CreateAssetMenu(menuName = "AgentConfig/Default")]
    public class AgentConfig : BaseSO
    {
        [SerializeField]
        public string title;

        [SerializeField]
        public float _walkSpeed;

        [SerializeField]
        public float _runSpeed;

        [SerializeField]
        public float _rotateSpeed;

        [SerializeField]
        public float _acceleration;

        [SerializeField]
        public LayerMask _hitBoxLayer;

        [SerializeField]
        public LayerMask _enemyHitBoxLayer;

        [SerializeField]
        public int _avoidancePriorityNormal;

        [SerializeField]
        public int _avoidancePriorityAttack;

        [SerializeField]
        public float enemyCloseRange;

        [SerializeField]
        public float deadDuration;

        [SerializeField]
        public bool canBeStagger;

        [SerializeField]
        public bool riseAfterBreak;

        [SerializeField]
        public bool canGetHitAnim;

        [SerializeField]
        public bool _canBeFreeze;

        [SerializeField]
        public bool _canBeFear;

        [SerializeField]
        public bool _canBeRabid;

        [SerializeField]
        public bool _canBeBurn;

        [SerializeField]
        public bool _canBeShock;

        [SerializeField]
        public bool _canBeToxic;

        [SerializeField]
        public bool _canBeBleed;

        [SerializeField]
        public bool _canBeSlow;

        [SerializeField]
        public bool _canBeArmorBreak;

        [SerializeField]
        public bool _canBeKnockBack;

        [SerializeField]
        public bool _canBeKnockDown;

        [SerializeField]
        public bool _canBeHooked;

        [SerializeField]
        public float _knockBackDeceleration;

        [SerializeField]
        [Tooltip("Play Appear state")]
        public bool _needSpawnAnimation;

        [SerializeField]
        [Tooltip("Play animation in Appear state")]
        public bool _hasSpawnAnimation;

        [SerializeField]
        public ParticleSystem appearFxPrefab;

        [SerializeField]
        public float appearFxScale;
    }

}
