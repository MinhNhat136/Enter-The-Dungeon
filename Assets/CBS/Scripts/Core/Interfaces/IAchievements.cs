﻿using CBS.Models;
using System;

namespace CBS
{
    public interface IAchievements
    {
        /// <summary>
        /// Notify when player complete achievement.
        /// </summary>
        event Action<CBSTask> OnCompleteAchievement;
        /// <summary>
        /// Notify when player complete achievement tier.
        /// </summary>
        event Action<CBSTask> OnCompleteAchievementTier;
        /// <summary>
        /// Notify when player receive reward for achievement.
        /// </summary>
        event Action<GrantRewardResult> OnProfileRewarded;

        /// <summary>
        /// Get information for all achievements and their player state.
        /// </summary>
        /// <param name="result"></param>
        void GetAchievementsTable(Action<CBSGetAchievementsTableResult> result);

        /// <summary>
        /// Get information for all available achievements for player achievements
        /// </summary>
        /// <param name="result"></param>
        void GetActiveAchievementsTable(Action<CBSGetAchievementsTableResult> result);

        /// <summary>
        /// Get information for all completed achievements for player achievements
        /// </summary>
        /// <param name="result"></param>
        void GetCompletedAchievementsTable(Action<CBSGetAchievementsTableResult> result);

        /// <summary>
        /// Adds a point to an achievement. For Achievements "OneShot" completes it immediately, for Achievements "Steps" - adds one step
        /// </summary>
        /// <param name="achievementID"></param>
        /// <param name="result"></param>
        void AddAchievementPoint(string achievementID, Action<CBSModifyAchievementPointResult> result);

        /// <summary>
        /// Adds the points you specified to the achievement. More suitable for Steps achievements.
        /// </summary>
        /// <param name="achievementID"></param>
        /// <param name="points"></param>
        /// <param name="result"></param>
        void AddAchievementPoint(string achievementID, int points, Action<CBSModifyAchievementPointResult> result);

        /// <summary>
        /// Updates the achievement points you specified. More suitable for Steps achievements.
        /// </summary>
        /// <param name="achievementID"></param>
        /// <param name="points"></param>
        /// <param name="result"></param>
        void UpdateAchievementPoint(string achievementID, int points, Action<CBSModifyAchievementPointResult> result);

        /// <summary>
        /// Pick up a reward from a completed achievement if it hasn't been picked up before.
        /// </summary>
        /// <param name="achievementID"></param>
        /// <param name="result"></param>
        void PickupAchievementReward(string achievementID, Action<CBSPickupAchievementRewardResult> result);

        /// <summary>
        /// Resets the state of a player for a specific achievement.
        /// </summary>
        /// <param name="achievementID"></param>
        /// <param name="result"></param>
        void ResetAchievement(string achievementID, Action<CBSResetAchievementResult> result);

        /// <summary>
        /// Get the number of completed achievements that did not receive rewards
        /// </summary>
        /// <param name="result"></param>
        void GetAchievementsBadge(Action<CBSBadgeResult> result);
    }
}

