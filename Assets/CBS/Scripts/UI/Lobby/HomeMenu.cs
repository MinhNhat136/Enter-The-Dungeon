using CBS.Scriptable;
using UnityEngine;

namespace CBS.UI
{
    public class HomeMenu : MonoBehaviour
    {
         public void ShowStore()
        {
            var prefabs = CBSScriptable.Get<StorePrefabs>();
            var storePrefab = prefabs.StoreWindows;
            UIView.ShowWindow(storePrefab);
        }

        public void ShowInventory()
        {
            var prefabs = CBSScriptable.Get<InventoryPrefabs>();
            var inventoryPrefab = prefabs.Inventory;
            UIView.ShowWindow(inventoryPrefab);
        }

        public void ShowHero()
        {
            
        }
        
        public void ShowClan()
        {
            var prefabs = CBSScriptable.Get<ClanPrefabs>();
            var windowPrefab = prefabs.WindowLoader;
            UIView.ShowWindow(windowPrefab);
        }

        public void ShowForge()
        {
            var prefabs = CBSScriptable.Get<CraftPrefabs>();
            var craftWindow = prefabs.CraftWindow;
            UIView.ShowWindow(craftWindow);
        }
    }
}