using CBS.Scriptable;
using UnityEngine;

namespace CBS.UI
{
    public class HomePackage : MonoBehaviour
    {
        public void ShowAdsReward()
        {
            
        }
        
        public void ShowNotification()
        {
            var prefabs = CBSScriptable.Get<NotificationPrefabs>();
            var notificationWindow = prefabs.NotificationWindow;
            UIView.ShowWindow(notificationWindow);
        }

        public void ShowEvents()
        {
            var prefabs = CBSScriptable.Get<EventsPrefabs>();
            var eventsWindow = prefabs.EventWindow;
            UIView.ShowWindow(eventsWindow);
        }
    }
    
}
