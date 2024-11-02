using System;

[Serializable]
public class InventoryItemTool : InventoryItemBreakable
{
    public SerializableGuid prefixDataId;
    public ToolPrefixData prefixData;
    public ToolItemData toolItemData => (ToolItemData) inventoryItemData;
    
    // CALLED WHEN ADDING ITEM TO INVENTORY
    public InventoryItemTool(InventoryItemData inventoryItemData, ToolPrefixData prefixData, int quantity = 1) : base(inventoryItemData, quantity)
    {
        this.prefixDataId = prefixData.id;
        this.prefixData = prefixData;
        SetCurrentDurabilityWithModifiers((InventoryItemDataBreakable) inventoryItemData);
    }

    // CALLED ON SAVEDATA LOAD
    public InventoryItemTool(InventoryItemData inventoryItemData, SerializableGuid prefixDataId, int quantity = 1, float durability = 1) : base(inventoryItemData, quantity, durability)
    {
        this.prefixDataId = prefixDataId;
        this.prefixData = InventoryManager.instance.FindToolPrefix(prefixDataId);
    }

    // CALLED ON SAVEDATA SAVE
    public override InventoryItemToPersist CastToInventoryItemToPersist() 
    {
        return new InventoryItemToolToPersist(this);
    }

    private void SetCurrentDurabilityWithModifiers(InventoryItemDataBreakable inventoryItemData)
    {
        currentDurability = inventoryItemData.maxDurability + (inventoryItemData.maxDurability * prefixData.durability);
    }
}