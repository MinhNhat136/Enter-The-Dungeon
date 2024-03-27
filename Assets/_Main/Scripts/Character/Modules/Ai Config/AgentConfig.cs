using Atomic.Core;
using UnityEngine;

namespace Atomic.Character.Config
{
    [CreateAssetMenu(menuName = "Ai Module Config/Agent/Default")]
    public class AgentConfig : BaseSO
    {
        [Header("DESCRIPTION")]
        [SerializeField]
        public string Title;

        [Header("LOCOMOTION")]
        [SerializeField]
        public float WalkSpeed;

        [SerializeField]
        public float RunSpeed;

        [SerializeField]
        public float RotateSpeed;

        [SerializeField]
        public float Acceleration;

        [Header("LAYER")]
        [SerializeField]
        public LayerMask HitBoxLayer;

        [SerializeField]
        public LayerMask AgentTargetLayers;

        [Header("NAVMESH")]
        [SerializeField]
        public int AvoidancePriorityNormal;

        [SerializeField]
        public int AvoidancePriorityAttack;


        [SerializeField]
        public float EnemyCloseRange;

        [SerializeField]
        public float DeadDuration;

        [Header("STATUS")]
        [SerializeField]
        public bool CanBeStagger;

        [SerializeField]
        public bool RiseAfterBreak;

        [SerializeField]
        public bool CanGetHitAnim;

        [SerializeField]
        public bool CanBeFreeze;

        [SerializeField]
        public bool CanBeFear;

        [SerializeField]
        public bool CanBeRabid;

        [SerializeField]
        public bool CanBeBurn;

        [SerializeField]
        public bool CanBeShock;

        [SerializeField]
        public bool CanBeToxic;

        [SerializeField]
        public bool CanBeBleed;

        [SerializeField]
        public bool CanBeSlow;

        [SerializeField]
        public bool CanBeArmorBreak;

        [SerializeField]
        public bool CanBeKnockBack;

        [SerializeField]
        public bool CanBeKnockDown;

        [SerializeField]
        public bool CanBeHooked;

        [SerializeField]
        public float KnockBackDeceleration;

    }

}
