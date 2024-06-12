using CBS.Scriptable;
using UnityEngine;

namespace CBS.UI
{
    public class SubMenu : MonoBehaviour
    {
        public void ShowMission()
        {
            var prefabs = CBSScriptable.Get<ProfileTasksPrefabs>();
            var tasksWindow = prefabs.ProfileTasksWindow;
            UIView.ShowWindow(tasksWindow);
        }

        public void ShowLootBox()
        {
            var prefabs = CBSScriptable.Get<LootboxPrefabs>();
            var lootBoxPrefab = prefabs.LootBoxes;
            UIView.ShowWindow(lootBoxPrefab);
        }
        
        public void ShowRanking()
        {
            var prefabs = CBSScriptable.Get<LeaderboardPrefabs>();
            var leaderboardsPrefab = prefabs.LeaderboardsWindow;
            UIView.ShowWindow(leaderboardsPrefab);
        }
        
        public void ShowDailyBonus()
        {
            var prefabs = CBSScriptable.Get<CalendarPrefabs>();
            var dailyBonusPrefab = prefabs.CalendarWindow;
            UIView.ShowWindow(dailyBonusPrefab);
        }

        public void ShowFriend()
        {
            var prefabs = CBSScriptable.Get<FriendsPrefabs>();
            var friendPrefab = prefabs.FriendsWindow;
            UIView.ShowWindow(friendPrefab);
        }
        
        public void ShowCommunity()
        {
            var prefabs = CBSScriptable.Get<ChatPrefabs>();
            var chatPrefab = prefabs.ChatWindow;
            UIView.ShowWindow(chatPrefab);
        }
    }
}