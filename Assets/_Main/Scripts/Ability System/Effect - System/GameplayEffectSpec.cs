using System;

namespace Atomic.AbilitySystem
{
    [Serializable]
    public class GameplayEffectSpec
    {
        public GameplayEffectScriptableObject GameplayEffectScriptableObject { get; private set; }
        public float DurationRemaining { get; private set; }
        public float TotalDuration { get; private set; }
        public GameplayEffectPeriod PeriodDefinition { get; private set; }
        public float TimeUntilPeriodTick { get; private set; }
        public float Level { get; private set; }
        public AbilitySystemController Source { get; private set; }
        public AbilitySystemController Target { get; private set; }
        public int Index { get; private set; }
        public AttributeValue? SourceCapturedAttribute = null;

        public static GameplayEffectSpec CreateNew(GameplayEffectScriptableObject gameplayEffect, AbilitySystemController source, float level = 1)
        {
            return new GameplayEffectSpec(gameplayEffect, source, level);
        }

        private GameplayEffectSpec(GameplayEffectScriptableObject gameplayEffectScriptableObject, AbilitySystemController source, float level = 1)
        {
            GameplayEffectScriptableObject = gameplayEffectScriptableObject;
            Source = source;
            for (var i = 0; i < GameplayEffectScriptableObject.gameplayEffect.modifiers.Length; i++)
            {
                GameplayEffectScriptableObject.gameplayEffect.modifiers[i].modifierMagnitude.Initialise(this);
            }
            Level = level;
            if (GameplayEffectScriptableObject.gameplayEffect.durationModifier)
            {
                DurationRemaining = GameplayEffectScriptableObject.gameplayEffect.durationModifier.CalculateMagnitude(this).GetValueOrDefault() * GameplayEffectScriptableObject.gameplayEffect.durationMultiplier;
                TotalDuration = DurationRemaining;
            }

            TimeUntilPeriodTick = GameplayEffectScriptableObject.gameplayEffectPeriod.period;
            // By setting the time to 0, we make sure it gets executed at first opportunity
            if (GameplayEffectScriptableObject.gameplayEffectPeriod.executeOnApplication)
            {
                TimeUntilPeriodTick = 0;
            }
        }

        public GameplayEffectSpec SetTarget(AbilitySystemController target)
        {
            Target = target;
            return this;
        }
        
        public void SetTotalDuration(float totalDuration)
        {
            TotalDuration = totalDuration;
        }

        public GameplayEffectSpec SetIndex(int index)
        {
            Index = index;
            return this;
        }
        
        public GameplayEffectSpec SetDuration(float duration)
        {
            DurationRemaining = duration;
            return this;
        }

        public GameplayEffectSpec UpdateRemainingDuration(float deltaTime)
        {
            DurationRemaining -= deltaTime;
            return this;
        }

        public GameplayEffectSpec TickPeriodic(float deltaTime, out bool executePeriodicTick)
        {
            TimeUntilPeriodTick -= deltaTime;
            executePeriodicTick = false;
            if (TimeUntilPeriodTick <= 0)
            {
                TimeUntilPeriodTick = GameplayEffectScriptableObject.gameplayEffectPeriod.period;

                // Check to make sure period is valid, otherwise we'd just end up executing every frame
                if (GameplayEffectScriptableObject.gameplayEffectPeriod.period > 0)
                {
                    executePeriodicTick = true;
                }
            }

            return this;
        }

        public GameplayEffectSpec SetLevel(float level)
        {
            Level = level;
            return this;
        }

    }
}
