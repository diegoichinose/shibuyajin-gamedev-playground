using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Item Data/Tool")]
public class ToolItemData : InventoryItemDataBreakable
{
    public Texture2D customCursor;
    public ToolType toolType;
    public float tickPower;
    public float tickSpeed;
    public int additionalHarvest;
    
    void Awake()
    {
        canStack = false;
    }

    public override InventoryItemToPersist CastToInventoryItemToPersist(int amount = 1) 
    {
        return new InventoryItemToolToPersist(this);
    }
}

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
    Hammer,
    Shovel,
    FloorScrapper
}