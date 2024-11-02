using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Data List/Inventory Item Data List/Every Existing Item Data List")]
public class EveryExistingItemDataList : ScriptableObject
{
    public InventoryItemDataList<CollectableItemData> collectableItemDataList;
    public InventoryItemDataList<KeyItemData> keyItemDataList;
    public InventoryItemDataList<ConsumableItemData> consumableItemDataListList;
    public InventoryItemDataList<StorageItemData> storageItemDataList;
    public InventoryItemDataList<ToolItemData> toolItemDataList;
    private List<InventoryItemData> everyExistingInventoryItemDataCache;

    public void OnNewRun() => GetEveryExistingItemData().ForEach(x => x.OnNewRun());

    public List<InventoryItemData> GetEveryExistingItemData()
    {
        if (everyExistingInventoryItemDataCache != null)
        if (everyExistingInventoryItemDataCache.Count > 0)
            return everyExistingInventoryItemDataCache.ToList(); 

        List<InventoryItemData> result = new List<InventoryItemData>();

        result.AddRange(collectableItemDataList.GetCombinedList());
        result.AddRange(keyItemDataList.GetCombinedList());
        result.AddRange(consumableItemDataListList.GetCombinedList());
        result.AddRange(storageItemDataList.GetCombinedList());
        result.AddRange(toolItemDataList.GetCombinedList());

        everyExistingInventoryItemDataCache = result.ToList();
        return everyExistingInventoryItemDataCache.ToList();
    }

    public InventoryItemData Find(SerializableGuid id)
    {
        return GetEveryExistingItemData().Find(inventoryItemData => inventoryItemData.id == id);
    }

    public void RefreshInventoryItemsDataListCache() 
    {
        everyExistingInventoryItemDataCache = null;
        
        collectableItemDataList.RefreshInventoryItemsDataListCache();
        keyItemDataList.RefreshInventoryItemsDataListCache();
        consumableItemDataListList.RefreshInventoryItemsDataListCache();
        storageItemDataList.RefreshInventoryItemsDataListCache();
        toolItemDataList.RefreshInventoryItemsDataListCache();

        GetEveryExistingItemData();
    }
}