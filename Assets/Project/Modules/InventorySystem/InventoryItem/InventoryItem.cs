using System;

[Serializable]
public class InventoryItem
{
    public InventoryItemData inventoryItemData;
    public int quantity;
    public Action OnQuantityUpdated;

    public InventoryItem()
    { 
        this.inventoryItemData = null;
        this.quantity = 1;
    }

    public InventoryItem(InventoryItemData inventoryItemData, int quantity = 1)
    { 
        this.inventoryItemData = inventoryItemData;
        
        this.quantity = quantity;
        OnQuantityUpdated?.Invoke();
    }

    public void ReduceQuantity(int amount = 1) => IncrementQuantity(-amount);
    public void IncrementQuantity(int amount = 1)
    {
        quantity += amount;

        if (quantity < 0)
            quantity = 0;
        
        OnQuantityUpdated?.Invoke();
    }

    public void SetQuantity(int amount)
    {
        quantity = amount;
        OnQuantityUpdated?.Invoke();
    }
    
    public virtual InventoryItemToPersist CastToInventoryItemToPersist() 
    {
        return new InventoryItemToPersist(this);
    }
}