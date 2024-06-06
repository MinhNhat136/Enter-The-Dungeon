﻿namespace CBS.Utils
{
    public class ItemTXTHandler
    {
        public const string PurchaseTitle = "Success Purchase";
        public const string PurchaseBody = "Congratulations! you have successfully purchased an item.";

        public const string RequestConsumeTitle = "Use this item?";
        public const string RequestConsumeBody = "Are you sure you want to use this item?";

        public const string RequestEquipTitle = "Equip this item?";
        public const string RequestEquipBody = "Are you sure you want to equip this item?";

        public const string RequestUnEquipTitle = "Unequip this item?";
        public const string RequestUnEquipBody = "Are you sure you want to unequip this item?";

        public const string CraftItemTitle = "You are successfully craft item";

        public static string GetUpgradeTitle(int nextLevelIndex, bool isMax)
        {
            if (isMax)
            {
                return "Max level reached!";
            }
            else
            {
                return "Next upgrade level - " + nextLevelIndex.ToString();
            }
        }
    }
}
