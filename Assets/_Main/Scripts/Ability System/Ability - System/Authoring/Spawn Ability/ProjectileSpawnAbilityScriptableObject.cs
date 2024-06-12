using System.Collections;
using Atomic.Character;
using Atomic.Core;
using Atomic.Equipment;
using UnityEngine;
using UnityEngine.Pool;

namespace Atomic.AbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Abilities/Projectile")]
    public class ProjectileSpawnAbilityScriptableObject : AbstractAbilityScriptableObject
    {
        public MinMaxFloat projectileContains;
        public MinMaxFloat speedWeight;
        public MinMaxFloat distanceWeight;
        public MinMaxFloat radiusWeight;
        public MinMaxFloat areaOfEffectDistance;
        
        public ProjectileBase projectilePrefab;
        public GameplayEffectScriptableObject gameplayEffect;
        private ObjectPool<ProjectileBase> ProjectilePool { get; set; }

        public AbstractAbilitySpec CreateSpec(BaseAgent owner, float energyValue, Vector3 shootPosition, Vector3 shootDirection,
            Vector3 shootTarget)
        {
            var projectileAbilitySpec = (ProjectileAbilitySpec)CreateSpec(owner.AiAbilityController.abilitySystemController);
            projectileAbilitySpec.owner = owner;
            projectileAbilitySpec.energyValue = energyValue;
            projectileAbilitySpec.shootPosition = shootPosition;
            projectileAbilitySpec.shootDirection = shootDirection;
            projectileAbilitySpec.shootTarget = shootTarget;
            projectileAbilitySpec.Level = owner.AiAbilityController.abilitySystemController.Level;
            foreach (var eventOnActivate in eventOnActivates)
            {
                projectileAbilitySpec.OnApplyGameplayEffect += eventOnActivate.PreApplyEffectSpec;
            }
            return projectileAbilitySpec;
        }
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemController owner)
        {
            var spec = new ProjectileAbilitySpec(this, owner)
            {
                Level = owner.Level,
                projectilePool = ProjectilePool,
            };
            return spec;
        }

        public override void Initialize()
        {
            CreatePool();
        }

        public override void Reset()
        {
            ProjectilePool.Clear();
        }
        
        private void CreatePool()
        {
            ProjectilePool = new ObjectPool<ProjectileBase>
            (
                CreateProjectile,
                OnGetProjectile,
                OnReleaseProjectile,
                OnDestroyProjectile,
                true,
                (int)projectileContains.min,
                (int)projectileContains.max);
        }

        private ProjectileBase CreateProjectile()
        {
            ProjectileBase projectileInstance = Instantiate(projectilePrefab);
            projectileInstance
                .SetDistanceWeight(distanceWeight)
                .SetSpeedWeight(speedWeight);
            projectileInstance.ProjectilePool = ProjectilePool;
            return projectileInstance;
        }

        private void OnGetProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private void OnReleaseProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        private void OnDestroyProjectile(ProjectileBase projectile)
        {
            Destroy(projectile);
        }
        
        private class ProjectileAbilitySpec : AbstractAbilitySpec
        {
            public ObjectPool<ProjectileBase> projectilePool;
            public BaseAgent owner;
            public Vector3 shootPosition;
            public Vector3 shootDirection;
            public Vector3 shootTarget;
            public float energyValue;
            private GameplayEffectSpec _effectSpec;
            private ProjectileBase projectile;

            public ProjectileAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemController owner) : base(ability, owner)
            {
            }

            protected override bool CheckGameplayTags()
            {
                return AscHasAllTags(Owner, Ability.abilityTags.OwnerTags.requireTags)
                       && AscHasNoneTags(Owner, Ability.abilityTags.OwnerTags.ignoreTags)
                       && AscHasAllTags(Owner, Ability.abilityTags.SourceTags.requireTags)
                       && AscHasNoneTags(Owner, Ability.abilityTags.SourceTags.ignoreTags);
            }
            
            protected override IEnumerator PreActivate()
            {
                projectile = projectilePool.Get();
                projectile.Load(owner, shootPosition, shootDirection, shootTarget, energyValue);
                yield return null;
            }

            protected override IEnumerator ActivateAbility()
            {
                var cdSpec = Owner.MakeOutgoingSpec(Ability.cooldown);
                var costSpec = Owner.MakeOutgoingSpec(Ability.cost);
                
                Owner.ApplyGameplayEffectSpecToSelf(cdSpec);
                Owner.ApplyGameplayEffectSpecToSelf(costSpec);

                projectile.Shoot();
                _effectSpec = Owner.MakeOutgoingSpec(((ProjectileSpawnAbilityScriptableObject)Ability).gameplayEffect);
                Owner.ApplyGameplayEffectSpecToSelf(_effectSpec);
                yield return null;
            }
            
            public override void CancelAbility()
            {
            }
        }
    }
}