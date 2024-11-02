using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Storage Chest Data")]
public class StorageChestData : ScriptableObject
{
    [SerializeField] private EveryExistingItemDataList _everyExistingItemDataList;
    private const int MAX_STORAGE_CHEST_CAPACITY = 10;
    [HideInInspector] public InventoryData inventoryDataInstance;

    public List<InventoryItemToPersist> GetRandomStorageChestContent(int amount = 0)
    {
        var itemOptions = _everyExistingItemDataList.GetEveryExistingItemData();
            itemOptions = itemOptions.Where(x => 
            {
                if (x.preventSpawningInsideChest)
                    return false;
                    
                 if (x.CanSpawn() == false)
                    return false;

                 return true;
            }).ToList();
        
        itemOptions.Shuffle();

        // IS SPECIFIC AMOUNT REQUESTED, JUST SHUFFLE
        if (amount != 0)
        {
            // IF AMOUNT REQUESTED IS HIGHER THAN AVAILABLE OPTIONS, RETURN ALL OPTIONS
            if (itemOptions.Count <= amount)
                return CastToInventoryItemToPersist(itemOptions);

            return CastToInventoryItemToPersist(itemOptions.GetRange(0, amount));
        }

        // USE THIS LOGIC TO MAKE SURE WE HAVE, FOR EXAMPLE:
        // 1 GUARANTEED CONSUMABLE
        // 1 GUARANTEED COLLECTABLE
        // 1 GUARANTEED RANDOM
        // 2 OPTIONAL RANDOM

        List<InventoryItemToPersist> result = new List<InventoryItemToPersist>();
        result.Add(itemOptions.First(x => x is ConsumableItemData).CastToInventoryItemToPersist());
        result.Add(itemOptions.First(x => x is CollectableItemData).CastToInventoryItemToPersist(amount: 5));

        var weightedRandom = InventoryManager.instance.GetWeightedRandom(itemOptions,
                                                                         minAmount: 1,
                                                                         additionalAmountToRandomize: 2);

        result.AddRange(CastToInventoryItemToPersist(weightedRandom));
        return result.ToList();
    }
    
    public InventoryData CreateInventoryDataInstance(List<InventoryItemToPersist> items)
    {
        InventoryData inventoryData = CreateInstance<InventoryData>();
        
        inventoryData.capacity = MAX_STORAGE_CHEST_CAPACITY;
        inventoryData.inventory = new List<InventoryItem>(MAX_STORAGE_CHEST_CAPACITY);
        
        items?.ForEach(item => 
        {
            var inventoryItemDataToAdd = _everyExistingItemDataList.Find(item.inventoryItemDataId);

            if (inventoryItemDataToAdd == null)
                Debug.LogWarning("InventoryItemData with ID " + item.inventoryItemDataId + " could not be found. You probably created it recently but forgot to include it inside the searchable list");

            inventoryData.TryAddOrIncrementItemToInventory(
                inventoryItem: item.CastToInventoryItem(inventoryItemDataToAdd),
                onFailureDropItemOnTheFloor: false, 
                playWarningTooltip: false);
        });

        inventoryDataInstance = inventoryData;
        return inventoryData;
    }

    public List<InventoryItemToPersist> GetItemListFromInventoryDataInstance()
    {
        var items = new List<InventoryItemToPersist>();
        
        if (inventoryDataInstance == null)
            return items;
            
        inventoryDataInstance.inventory.ForEach(inventoryItem => 
        {
            items.Add(inventoryItem.CastToInventoryItemToPersist());
        });

        return items;
    }
    
    private List<InventoryItemToPersist> CastToInventoryItemToPersist(List<InventoryItemData> inventoryItemDataList)
    {
        List<InventoryItemToPersist> result = new List<InventoryItemToPersist>();
        foreach (InventoryItemData inventoryItemData in inventoryItemDataList) 
        {
            result.Add(inventoryItemData.CastToInventoryItemToPersist());
        }
        return result.ToList();
    }
}