using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Data List/Storage Chest Data List")]
public class StorageChestDataList : ScriptableObject
{
    public List<StorageChestData> chests;
    public bool isAnyStorageChestOpen;
}