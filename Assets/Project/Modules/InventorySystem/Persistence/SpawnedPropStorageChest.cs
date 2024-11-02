using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnedPropStorageChest : SpawnedProp
{
    [SerializeReference] public List<InventoryItemToPersist> items;

    public SpawnedPropStorageChest(SerializableGuid id, string prefabName, Vector3 position) : base(id, prefabName, position) 
    { }
}