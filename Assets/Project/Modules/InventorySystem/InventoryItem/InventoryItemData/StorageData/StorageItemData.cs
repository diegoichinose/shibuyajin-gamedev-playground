using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Item Data/Storage Item")]
public class StorageItemData : InventoryItemData
{
    public InventoryData storageInventoryData;
    public Sprite storageDrawerSprite;
}