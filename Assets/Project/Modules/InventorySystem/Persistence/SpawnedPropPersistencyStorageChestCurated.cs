using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnedPropPersistencyStorageChestCurated : SpawnedPropPersistencyStorageChest
{
    [SerializeField] private List<InventoryItemData> itemsToPrefillChestOnSpawn;

    public override SpawnedProp GetSpawnedProp()
    {
        return spawnedPropStorageChest;
    }
    
    public override SerializableGuid AddSelfToSaveData(Vector3 position)
    {
        var itemsForCuratedChest = itemsToPrefillChestOnSpawn.Where(x => x.CanSpawn()).ToList();

        spawnedProp = new SpawnedPropStorageChest(Guid.NewGuid(), gameObject.GetPrefabName(), position)
        {
            items = CastToInventoryItemToPersist(itemsForCuratedChest)
        };

        _spawnedPropSaveData.saveFile.spawnedProps.Add(spawnedProp);
        return spawnedProp.id;
    }

    private List<InventoryItemToPersist> CastToInventoryItemToPersist(List<InventoryItemData> inventoryItemDataList)
    {
        List<InventoryItemToPersist> result = new List<InventoryItemToPersist>();

        foreach (InventoryItemData inventoryItemData in inventoryItemDataList) 
        {
            result.Add(inventoryItemData.CastToInventoryItemToPersist());
        }

        return result;
    }
}