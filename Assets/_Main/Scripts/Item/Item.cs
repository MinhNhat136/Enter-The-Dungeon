using UnityEngine;

public enum ItemType 
{ 
    Consume, 
    BuffStats, 
    Upgrade, 
    Equipment 
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]

public class Item : ScriptableObject
{
    //  Events ----------------------------------------


    //  Properties ------------------------------------


    //  Fields ----------------------------------------
    new public string name = "New Item";
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemType type;

    //  Other Methods ---------------------------------
    public virtual void Use()
    {
    }

    public virtual void Drop()
    {
    }


}