using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPropPersistencyStorageChest : SpawnedPropPersistency
{
    [SerializeField] private StorageChestData _storageChestData;
    public SpawnedPropStorageChest spawnedPropStorageChest => spawnedProp == null ? null : (SpawnedPropStorageChest) spawnedProp;

    public override SpawnedProp GetSpawnedProp()
    {
        return spawnedPropStorageChest;
    }

    public override SerializableGuid AddSelfToSaveDataFromCrafting(Transform transform) 
    {
        spawnedProp = new SpawnedPropStorageChest(Guid.NewGuid(), gameObject.GetPrefabName(), transform.position);

        _spawnedPropSaveData.saveFile.spawnedProps.Add(spawnedProp);
        return spawnedProp.id;
    } 

    public override SerializableGuid AddSelfToSaveData(Vector3 position)
    {
        if (_storageChestData == null)
            spawnedProp = new SpawnedPropStorageChest(Guid.NewGuid(), gameObject.GetPrefabName(), position)
            {
                items = new List<InventoryItemToPersist>()
            };
        else
            spawnedProp = new SpawnedPropStorageChest(Guid.NewGuid(), gameObject.GetPrefabName(), position)
            {
                items = _storageChestData.GetRandomStorageChestContent()
            };

        _spawnedPropSaveData.saveFile.spawnedProps.Add(spawnedProp);
        return spawnedProp.id;
    }
}