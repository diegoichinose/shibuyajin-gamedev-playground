using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Harvestable Prop")]
public class HarvestablePropData : ScriptableObject
{
    public AudioClip harvestTickSound;
    public AudioClip harvestCompleteSound;
    public ToolType requiredToolType;
    public float durabilityTicks = 3;
    public InventoryItemDataProbabilityList itemsToDrop;
    
    [Header("DO NOT TOUCH - FOR INSPECTION ONLY")]
    public InventoryItemTool requiredTool;
}