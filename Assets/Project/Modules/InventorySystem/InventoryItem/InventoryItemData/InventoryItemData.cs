using System;
using UnityEngine;

[Serializable]
public abstract class InventoryItemData : ScriptableObject
{
    public SerializableGuid id;
    public string title;
    [TextArea] public string description;
    public Sprite icon;
    public bool canStack;
    public bool isMaterial;
    public bool preventSpawningInsideChest;
    public int tier;
    public float buyingPrice;
    public float sellingPrice;
    [Tooltip("Rarity weight from 0 to 100, use 50 for neutral chance")] public int rarityWeight = 50;
    public virtual bool CanSpawn() => true;
    public virtual void OnSpawnConfirmed(){}
    public virtual void OnNewRun(){}
    public virtual void RunThisOnDuskWhileInsideInventory(){}

    public virtual InventoryItemToPersist CastToInventoryItemToPersist(int amount = 1) 
    {
        return new InventoryItemToPersist(id, amount);
    }
}