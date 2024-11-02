using UnityEngine;

public abstract class ConsumableItemData : InventoryItemData
{
    protected InventoryData _inventoryDataSource;

    public void Consume(InventoryData inventoryDataSource)
    {
        _inventoryDataSource = inventoryDataSource;

        if (TryConsume())
            _inventoryDataSource.TryConsumeItemFromInventory(this, amount: 1);
    }

    protected abstract bool TryConsume();
}