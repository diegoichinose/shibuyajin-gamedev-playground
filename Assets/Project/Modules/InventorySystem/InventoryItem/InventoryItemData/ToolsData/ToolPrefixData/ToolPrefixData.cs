using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Item Prefix/Tool Prefix Data")]
public class ToolPrefixData : ScriptableObject
{
    public SerializableGuid id;
    public string title;

    
    [Header("USE DECIMAL PERCENTAGE (eg. 0.1 for +10%)")]
    public float speed;
    public float tickPower;
    public float durability;
    public int additionalHarvest;
    public float priceModifier;
}