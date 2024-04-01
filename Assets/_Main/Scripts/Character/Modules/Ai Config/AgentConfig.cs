using Atomic.Core;
using UnityEngine;
using UnityEngine.Serialization;

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
    }

}
