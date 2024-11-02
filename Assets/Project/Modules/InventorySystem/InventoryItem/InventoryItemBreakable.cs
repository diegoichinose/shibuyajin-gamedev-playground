using System;

[Serializable]
public class InventoryItemBreakable : InventoryItem
{
    public float currentDurability;
    public InventoryItemDataBreakable inventoryItemDataBreakable => (InventoryItemDataBreakable) inventoryItemData;

    // CALLED WHEN ADDING ITEM TO INVENTORY VIA EDITOR MENU
    public InventoryItemBreakable() : base()
    { 
        TrySetCurrentDurability();
    }
    
    // CALLED WHEN ADDING ITEM TO INVENTORY
    public InventoryItemBreakable(InventoryItemData inventoryItemData, int quantity = 1) : base(inventoryItemData, quantity)
    { 
        TrySetCurrentDurability();
    }

    private void TrySetCurrentDurability()
    {
        if (inventoryItemData == null)
            return; 
            
        currentDurability = ((InventoryItemDataBreakable) inventoryItemData).maxDurability;
    }
    
    // CALLED ON SAVEDATA LOAD
    public InventoryItemBreakable(InventoryItemData inventoryItemData, int quantity = 1, float currentDurability = 1) : base(inventoryItemData, quantity)
    { 
        this.currentDurability = currentDurability;
    }

    // CALLED ON SAVEDATA SAVE
    public override InventoryItemToPersist CastToInventoryItemToPersist() 
    {
        return new InventoryItemBreakableToPersist(this);
    }
}
