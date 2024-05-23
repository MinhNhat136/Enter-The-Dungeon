using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "new data", menuName = "DataInventory")]
public class DataInventory : ScriptableObject
{
    public List<ItemBase.ItemData> inventoryData;
    public List<ItemBase.ItemData> equipmentData;

    public void ResetAllData()
    {
        inventoryData.Clear();
        equipmentData.Clear();
        File.Delete(Application.persistentDataPath + "/savedata.json");
    }
    public int GetAmount(ItemInfo info)
    {
        foreach (ItemBase.ItemData data in inventoryData)
        {
            if (data.info == info) //if already have
            {
                return data.amount;
            }
        }
        return 0;
    }
    public void AddInventoryData(ItemBase.ItemData newData)
    {
        foreach (ItemBase.ItemData data in inventoryData)
        {
            if (data.info == newData.info) //if already have
            {
                if (newData.info.prop.countable) //if item is countable, otherwise we always added item, so I write at below.
                {
                    data.amount = newData.amount;
                    return;
                }
            }
        }
        inventoryData.Add(newData);
    }
    public void AddEquipmentData(ItemBase.ItemData data)
    {
        equipmentData.Add(data);
    }
    public void RemoveInventoryData(int ID)
    {
        foreach (ItemBase.ItemData data in inventoryData)
        {
            if(data.ID == ID)
            {
                inventoryData.Remove(data);
                return;
            }
        }
    }
    public void RemoveEquipmentData(int ID)
    {
        foreach (ItemBase.ItemData data in equipmentData)
        {
            if (data.ID == ID)
            {
                equipmentData.Remove(data);
                return;
            }
        }
    }

    public void UpdateStatItemData(int ID, ItemBase.ItemData newData)
    {
        ItemBase.ItemData currentData = equipmentData.Find(i => i.ID == ID);
        currentData = newData;
    }

    public static void SaveData(DataInventory data) //To save the data to local
    {
        SaveData saveData = new SaveData();
        saveData.inventoryData = data.inventoryData;
        saveData.equipmentData = data.equipmentData;

        string json = JsonUtility.ToJson(saveData);
        string path = Application.persistentDataPath + "/savedata.json";
        File.WriteAllText(path, json);
    }
    public static void LoadData(DataInventory data)
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            data.inventoryData = saveData.inventoryData;
            data.equipmentData = saveData.equipmentData;
        }
    }

}

[System.Serializable]
public class SaveData
{
    public List<ItemBase.ItemData> inventoryData;
    public List<ItemBase.ItemData> equipmentData;
}
