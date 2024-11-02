using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Data Saver Loaders/Inventory Item Data Saver Loader")]
public class InventoryItemDataSaverLoader : IPersistentData
{
    [SerializeField] private EveryExistingItemDataList _everyExistingItemDataList;
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private InventoryData _inventoryDataBag;
    [SerializeField] private InventoryData _inventoryDataBackpack;

    public override void OnNewSave()
    {
        _inventoryData.ResetToDefault();
        _inventoryDataBag.ResetToDefault();
        _inventoryDataBackpack.ResetToDefault();
    }

    public override void SaveData(SaveFile saveFile)
    {
        base.SaveData(saveFile);

        saveFile.inventory = GetInventoryItemListToPersist(_inventoryData.inventory);
        saveFile.inventoryBag = GetInventoryItemListToPersist(_inventoryDataBag.inventory);
        saveFile.inventoryBackpack = GetInventoryItemListToPersist(_inventoryDataBackpack.inventory);
    }

    public override void LoadData(SaveFile saveFile)
    {
        base.LoadData(saveFile);

        SetInventoryDataFrom(saveFile.inventory, _inventoryData);
        SetInventoryDataFrom(saveFile.inventoryBag, _inventoryDataBag);
        SetInventoryDataFrom(saveFile.inventoryBackpack, _inventoryDataBackpack);
    }

    public List<InventoryItemToPersist> GetInventoryItemListToPersist(List<InventoryItem> inventoryItemList, int capacity = 10)
    {
        List<InventoryItemToPersist> list = new List<InventoryItemToPersist>(capacity);

        inventoryItemList.ForEach(inventoryItem => 
        {
            list.Add(inventoryItem.CastToInventoryItemToPersist());
        });
        
        return list.ToList();
    }

    public List<InventoryItem> GetInventoryItemList(List<InventoryItemToPersist> inventoryItemToPersistList)
    {
        List<InventoryItem> list = new List<InventoryItem>(10);
        inventoryItemToPersistList.ToList().ForEach(inventoryItemToPersist => 
        {
            InventoryItemData inventoryItemData = _everyExistingItemDataList.Find(id: inventoryItemToPersist.inventoryItemDataId);
            
            if (inventoryItemData != null)
                list.Add(new InventoryItem(inventoryItemData, inventoryItemToPersist.quantity));
        });
        return list.ToList();
    }

    public void SetInventoryDataFrom(List<InventoryItemToPersist> inventoryItemToPersistList, InventoryData inventoryData)
    {
        inventoryData.inventory = new List<InventoryItem>(10);
        inventoryItemToPersistList.ToList().ForEach(inventoryItemToPersist => 
        {
            InventoryItemData inventoryItemData = _everyExistingItemDataList.Find(id: inventoryItemToPersist.inventoryItemDataId);
            
            if (inventoryItemData == null)
            {
                Debug.LogWarning("You forgot to add item with id " + inventoryItemToPersist.inventoryItemDataId + " to the InventoryItemDataList, lil bro");
                return;
            }

            InventoryItem inventoryItem = inventoryItemToPersist.CastToInventoryItem(inventoryItemData);
            inventoryData.TryAddOrIncrementItemToInventory(inventoryItem);
        });

        inventoryData.OnInventoryChange?.Invoke();
    }
}

[Serializable]
public class InventoryItemToPersist
{    
    public SerializableGuid inventoryItemDataId;
    public int quantity;

    public InventoryItemToPersist(SerializableGuid inventoryItemDataId, int quantity)
    {
        this.inventoryItemDataId = inventoryItemDataId;
        this.quantity = quantity;
    }

    public InventoryItemToPersist(InventoryItem inventoryItem)
    {
        this.inventoryItemDataId = inventoryItem.inventoryItemData.id;
        this.quantity = inventoryItem.quantity;
    }

    public virtual InventoryItem CastToInventoryItem(InventoryItemData inventoryItemData) 
    {
        return new InventoryItem(inventoryItemData, quantity);
    }
}

[Serializable]
public class InventoryItemBreakableToPersist : InventoryItemToPersist
{    
    public float currentDurability;

    public InventoryItemBreakableToPersist(SerializableGuid inventoryItemDataId, int quantity) : base(inventoryItemDataId, quantity)
    { }
    
    public InventoryItemBreakableToPersist(InventoryItemBreakable inventoryItem) : base(inventoryItem)
    {
        this.currentDurability = inventoryItem.currentDurability;
    }
    
    public InventoryItemBreakableToPersist(InventoryItemDataBreakable inventoryItemData) : base(inventoryItemData.id, quantity: 1)
    {
        this.currentDurability = inventoryItemData.maxDurability;
    }
    
    public override InventoryItem CastToInventoryItem(InventoryItemData inventoryItemData)
    {
        return new InventoryItemBreakable(inventoryItemData, quantity, currentDurability);
    }
}

[Serializable]
public class InventoryItemToolToPersist : InventoryItemBreakableToPersist
{    
    public SerializableGuid prefixId;

    public InventoryItemToolToPersist(ToolItemData toolItemData) : base((InventoryItemDataBreakable) toolItemData)
    {
        var randomPrefix = InventoryManager.instance.GetRandomToolPrefix();
        
        this.prefixId = randomPrefix.id;
        this.currentDurability = toolItemData.maxDurability + (toolItemData.maxDurability * randomPrefix.durability);
    }
    
    public InventoryItemToolToPersist(InventoryItemTool inventoryItem) : base(inventoryItem)
    {
        this.prefixId = inventoryItem.prefixDataId;
    }
    
    public override InventoryItem CastToInventoryItem(InventoryItemData inventoryItemData) 
    {
        return new InventoryItemTool(inventoryItemData, prefixId, quantity, currentDurability);
    }
}