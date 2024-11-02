using UnityEngine;

public class InventoryDrawerExtra : InventoryDrawerBase
{   
    void OnDisable()
    {
        drawInventoryInstance = null;
        inventoryData.OnInventoryChange -= DrawInventory;
    }

    protected override void InstantiateInventorySlots()
    {
        base.InstantiateInventorySlots();

        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab);
            newSlot.transform.SetParent(transform, false);
            
            InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();
            newSlotComponent.ClearSlot();
            newSlotComponent.thisInventorySlotNumber = i + 1;

            inventorySlots.Add(newSlotComponent);
        }
    }
}