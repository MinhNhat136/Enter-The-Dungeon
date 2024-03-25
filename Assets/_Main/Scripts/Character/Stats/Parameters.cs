using Atomic.Core.Interface;
using System;

namespace Atomic.Character.Stats
{
    public class Parameters : IInitializable
    {
        public Parameter NegativeOne;

        public Parameter InitialMaxHp;

        public Parameter MaxHpFlat;

        public Parameter MaxHpBonus;

        public Parameter FinalMaxHp;

        public Parameter DamageReduction;

        public Parameter Evade;

        public Parameter DamageTaken;

        public Parameter CriticalChanceBonus;

        public Parameter CriticalDamageBonus;

        public Parameter MissChance;

        public Parameter DamageBonus;

        public Parameter FinalMovementSpeedBonus;

        public Parameter SkillChargeSpeedBonus;

        public Parameter SkillChargeSpeedBonusNegative;

        public Parameter SkillDamageBonus;

        public Parameter SkillCooldownBonus;

        public Parameter PetAttackIntervalReduction;

        public Parameter WeaponDamageBonus;

        public Parameter WeaponDamageFlat;

        public Parameter FinalWeaponDamage;

        public Parameter FreezeDurationBonus;

        public Parameter BurnDamageBonus;

        public Parameter ToxicDamageBonus;

        public Parameter ShockDamageBonus;

        public Parameter StaggerResist;

        public Parameter KnockdownResist;

        public Parameter TrapDamageResist;

        public Parameter RabidResist;

        public Parameter RabidResistDurationBonus;

        public Parameter ToxicResist;

        public Parameter CastSkillDamageBonus;

        public Parameter ThrowSkillDamageBonus;

        public Parameter DeploySkillDamageBonus;

        public Parameter ElementalDamageBonus;

        public Parameter CastSkillStarBoost;

        public Parameter ThrowSkillStarBoost;

        public Parameter DeploySkillStarBoost;

        public Parameter BuffSkillStarBoost;

        public Parameter CastSkillCooldownBonus;

        public Parameter ThrowSkillCooldownBonus;

        public Parameter DeploySkillCooldownBonus;

        public Parameter BuffSkillCooldownBonus;

        public Parameter RollDistanceBonus;

        public Parameter LifeSteal;

        public Parameter BurnDurationBonus;

        public Parameter ToxicDurationBonus;

        public Parameter ShockDurationBonus;

        public Parameter SlowDurationBonus;

        public Parameter StaggerDurationBonus;

        public Parameter SkillEffectTimeBonus;

        public Parameter DamageReflection;

        public Parameter WeaponCriticalChance;

        public Parameter AllSkillStarBoost;

        public Parameter SkillCanCritical;

        public Parameter AddStarToFirstSkill;

        public Parameter RegenPerStage;

        public Parameter MeleeWeaponDamageBonus;

        public Parameter RangedWeaponDamageBonus;

        public Parameter PetDamageBonus;

        public Parameter ElementalDurationBonus;

        public Parameter FoodHealBonus;

        private Parameter[] parameters;

        private bool _isInitialized; 

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public void Initialize()
        {
            if(!_isInitialized)
            {
                _isInitialized = true;

            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }
    }

}
