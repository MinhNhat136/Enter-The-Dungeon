using UnityEngine;

namespace Atomic.Character.Config
{
    [CreateAssetMenu(menuName = "Ai Module Config/Agent/Ai", fileName ="Ai Config")]
    public class AiConfig : MotorConfig
    {
        [SerializeField]
        public bool canSpawnRandomPosition;

        [SerializeField]
        public bool spawnMaxOnePerPosition;

        [SerializeField]
        public float maxHp;

        [SerializeField]
        private bool fixedMaxHp;

        [SerializeField]
        public int dmg;

        [SerializeField]
        private float damageReduction;

        [SerializeField]
        public float evadeJumpSpeed;

        [SerializeField]
        public float escapeDuration;

        [SerializeField]
        public float attackInterval;

        [SerializeField]
        public float bleedDuration;

        private static float[] DamageChapterScale;

        private static float[] HpChapterScale;

        private static float[] HpStageFactor;

        private static float[] DamageStageFactor;

        [HideInInspector]
        public int ChapterUnlock;

        [HideInInspector]
        public int RoomUnlock;

        public void SetupUnlockInfo(int chapterIndex, int roomIndex)
        {
        }

        private float GetMaxHp(int chapterIndex, int room, int difficulty)
        {
            return 0f;
        }

        private int GetDamage(int chapterIndex, int room, int difficulty)
        {
            return 0;
        }

        private float GetDamageReduction()
        {
            return 0f;
        }


    }
}

