using UnityEngine;

public class InventorySlotRightClickListener : MonoBehaviour
{
    public static InventorySlotRightClickListener instance = null;
    private InventoryData selectedInventoryData;
    
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
    
    public void OnRightClickInventorySlotInvoke(InventoryItem inventoryItem, InventoryData inventoryData)
    {
        selectedInventoryData = inventoryData;
        InventoryItemData inventoryItemData = inventoryItem.inventoryItemData;

        if (TryConsumeConsumable(inventoryItemData))
            return;
            
        if (TryOpenCloseExtraInventoryUI(inventoryItemData))
            return;
    }

    private bool TryConsumeConsumable(InventoryItemData inventoryItemData)
    {
        if (inventoryItemData is not ConsumableItemData)
            return false;
            
        ConsumableItemData consumableItemData = (ConsumableItemData) inventoryItemData;
        consumableItemData.Consume(selectedInventoryData);

        return true;
    }

    private bool TryOpenCloseExtraInventoryUI(InventoryItemData inventoryItemData)
    {
        if (inventoryItemData is not StorageItemData)
            return false;
            
        InventoryManager.instance.OpenCloseExtraInventoryUI((StorageItemData) inventoryItemData);

        return true;
    }
}