using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveFile
{
    public int saveSlotIndex;
    public float totalPlayTimeInSeconds;
    public string lastUpdated;
    public DateTime GetLastUpdated() => JsonUtility.FromJson<JsonDateTime>(lastUpdated);
    [SerializeReference] public List<InventoryItemToPersist> inventory;
    [SerializeReference] public List<InventoryItemToPersist> inventoryBag;
    [SerializeReference] public List<InventoryItemToPersist> inventoryBackpack;
    
    public SaveFile(int saveSlotIndex)
    {
        this.saveSlotIndex = saveSlotIndex;
        
        totalPlayTimeInSeconds = 0;
        lastUpdated = JsonUtility.ToJson((JsonDateTime) DateTime.Now);
        
        inventory = new List<InventoryItemToPersist>();
        inventoryBag = new List<InventoryItemToPersist>();
        inventoryBackpack = new List<InventoryItemToPersist>();
    }
}