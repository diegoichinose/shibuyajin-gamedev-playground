public class InventoryItemDataBreakable : InventoryItemData
{
    public float maxDurability = 20;
    public override InventoryItemToPersist CastToInventoryItemToPersist(int amount = 1) 
    {
        return new InventoryItemBreakableToPersist(this);
    }
}