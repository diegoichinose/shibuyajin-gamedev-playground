using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "My Scriptable Objects/Inventory Data")]
public class InventoryData : ScriptableObject
{
    [SerializeReference] public List<InventoryItem> inventory;
    public Action<InventoryItem> OnInventoryItemRemoved;
    public Action<InventoryItem> OnInventorySlotDeselected;
    public Action<InventoryItem> OnInventorySlotSelected;
    public Action OnInventoryChange;
    public bool isMainInventoryData;
    public int capacity;
    public int currentlySelectedSlotNumber = 1;
    public bool StackableItemAlreadyExistsInTheInventory(InventoryItemData itemData) => itemData.canStack && inventory.Exists(x => x.inventoryItemData == itemData);
    public bool isInventoryNotFull => inventory.Count < capacity;
    public bool isInventoryFull => !isInventoryNotFull;


    void OnEnable()
    {
        if (inventory == null)
            inventory = new List<InventoryItem>();

        inventory.Capacity = capacity;
        currentlySelectedSlotNumber = 1;
    }

    public void ResetToDefault()
    {
        InventoryItem selectedSlot = GetCurrentlySelectedSlotItem();
        
        if(selectedSlot != null)
            OnInventorySlotDeselected?.Invoke(selectedSlot);

        inventory = new List<InventoryItem>(capacity);
        currentlySelectedSlotNumber = 1;

        OnInventoryChange?.Invoke();
    }

    public bool TryAddOrIncrementItemToInventory(InventoryItemData inventoryItemData, int quantity = 1, bool onFailureDropItemOnTheFloor = false, bool playWarningTooltip = true)
    { 
        InventoryItem inventoryItem = CreateInventoryItemFromData(inventoryItemData, quantity);
        return TryAddOrIncrementItemToInventory(inventoryItem, onFailureDropItemOnTheFloor, playWarningTooltip);
    }

    public bool TryAddOrIncrementItemToInventory(InventoryItem inventoryItem, bool onFailureDropItemOnTheFloor = false, bool playWarningTooltip = true, bool checkForStorageItems = true)
    { 
        if (TryIncrementExistingItem(inventoryItem, checkForStorageItems))
            return true;
            
        if(TryAddNewItemTo(inventoryData: this, inventoryItem))
            return true;

        if (checkForStorageItems)
        if (TryAddItemToAdditionalStorage(inventoryItem))
            return true;

        if (onFailureDropItemOnTheFloor)
            GameEventsManager.instance.OnDropItemRequested.Invoke(inventoryItem);

        if (playWarningTooltip)
            PlayerTooltipManager.instance.ShowPlayerTooltip("MY POCKETS ARE FULL!");

        return false;
    }

    private bool TryIncrementExistingItem(InventoryItem inventoryItem, bool checkForStorageItems = true)
    {
        bool result = false;
        foreach (var inventorySlot in inventory)
        {
            if (checkForStorageItems)
            if (inventorySlot.inventoryItemData is StorageItemData storageItemData)
            if (TryIncrementExistingItemTo(inventoryData: storageItemData.storageInventoryData, inventoryItem))
            {
                result = true;
                break;
            }

            if (TryIncrementExistingItemTo(inventoryData: this, inventoryItem))
            {
                result = true;
                break;
            }
        }
        
        return result;
    }

    private bool TryIncrementExistingItemTo(InventoryData inventoryData, InventoryItem inventoryItem)
    {
        if (inventoryItem.inventoryItemData == null)
        {
            Debug.LogWarning("inventoryItem.inventoryItemData is null");
            return false;
        }

        InventoryItem matchingItem = inventoryData.inventory.Find(inventorySlot => inventorySlot.inventoryItemData.id == inventoryItem.inventoryItemData.id);
        
        if (matchingItem == null)
            return false;

        if (matchingItem.inventoryItemData.canStack == false)
            return false;
            
        matchingItem.IncrementQuantity(inventoryItem.quantity);
        inventoryData.OnInventoryChange?.Invoke();
        return true;
    }

    public bool TryAddNewItemTo(InventoryData inventoryData, InventoryItem inventoryItem)
    {
        if (inventoryData.isInventoryFull)
            return false; 
            
        inventoryData.inventory.Add(inventoryItem);
        inventoryData.OnInventoryChange?.Invoke();

        return true;
    }
    
    private bool TryAddItemToAdditionalStorage(InventoryItem inventoryItem)
    {
        bool result = false;
        foreach (var inventorySlot in inventory)
        {
            if (inventorySlot.inventoryItemData is not StorageItemData storageItemData)
                continue;

            if (TryAddNewItemTo(inventoryData: storageItemData.storageInventoryData, inventoryItem))
            {
                result = true;
                break;
            }
        }

        return result;
    }

    [ContextMenu(nameof(AddInventoryItem))] public void AddInventoryItem() => inventory.Add(new InventoryItem());
    [ContextMenu(nameof(AddInventoryItemBreakable))] public void AddInventoryItemBreakable() => inventory.Add(new InventoryItemBreakable());

    public bool TryConsumeItemFromInventory(InventoryItemData inventoryItemData, int amount)
    {
        InventoryItem matchingItem = inventory.Find(x => x.inventoryItemData == inventoryItemData);

        if (matchingItem == null)
            return TryConsumeItemInsideBackpack(inventoryItemData, amount);

        if (amount > matchingItem.quantity)
            TryConsumeItemInsideBackpack(inventoryItemData, amount - matchingItem.quantity);

        matchingItem.ReduceQuantity(amount);
        
        if (matchingItem.quantity == 0) 
        {
            inventory.Remove(matchingItem);
            OnInventoryItemRemoved?.Invoke(matchingItem);
        }

        OnInventoryChange?.Invoke();
        return true;
    }

    public bool TryConsumeItemInsideBackpack(InventoryItemData itemData, int amount)
    {
        var matchingItem = GetInventoryItemInsideBackpack(itemData, out InventoryData backpackInventoryData);

        if (matchingItem == null)
            return false;
            
        matchingItem.ReduceQuantity(amount);

        if (matchingItem.quantity == 0) 
        {
            backpackInventoryData.inventory.Remove(matchingItem);
            OnInventoryItemRemoved?.Invoke(matchingItem);
        }

        backpackInventoryData.OnInventoryChange?.Invoke();
        OnInventoryChange?.Invoke();
        return true;
    }


    public void RemoveEntireItemStackFromInventory(InventoryItem inventoryItem, bool dropItem = true)
    { 
        inventory.Remove(inventoryItem);

        if (currentlySelectedSlotNumber > inventory.Count)
            SetSelectedSlotNumber(inventory.Count);

        OnInventoryItemRemoved?.Invoke(inventoryItem);
        OnInventoryChange?.Invoke();
        
        if (dropItem)
            GameEventsManager.instance.OnDropItemRequested.Invoke(inventoryItem);
            
    }

    public int GetItemAmount(InventoryItemData inventoryItemData, bool includeBackpacks = true)
    {
        if (includeBackpacks == false)
        {
            var matchingItem = inventory.Find(x => x.inventoryItemData == inventoryItemData);
            return matchingItem == null ? 0 : matchingItem.quantity; 
        }

        InventoryItem result = null;
        GetInventoryDataListIncludingBackpacks().ForEach(inventoryData => 
        {
            InventoryItem matchingItem = inventoryData.inventory.Find(inventoryItem => inventoryItem.inventoryItemData == inventoryItemData);

            if (matchingItem == null)
                return;

            if (result == null)
                result = new InventoryItem(matchingItem.inventoryItemData, matchingItem.quantity);
            else
                result.quantity += matchingItem.quantity;
        });

        if (result == null)
            return 0;

        return result.quantity;
    }
    
    public void SetSelectedSlotNumber(int slotNumber)
    {
        currentlySelectedSlotNumber = slotNumber;
        
        if (currentlySelectedSlotNumber == 0)
            currentlySelectedSlotNumber = 1;
    }

    public void MoveInventoryItem(int oldIndex, int newIndex)
    {
        InventoryItem item = inventory[oldIndex];
        inventory.RemoveAt(oldIndex);
        inventory.Insert(newIndex, item);
        OnInventoryChange?.Invoke();
    }

    public InventoryItem GetInventoryItemInsideBackpack(InventoryItemData inventoryItemData, out InventoryData inventoryItemSource)
    {
        InventoryItem result = null;
        InventoryData resultSource = null;

        inventory.Find(inventoryItem => 
        {
            if (inventoryItem.inventoryItemData is not StorageItemData)
                return false;

            var storageItemData = (StorageItemData) inventoryItem.inventoryItemData;

            result = storageItemData.storageInventoryData.inventory.Find(itemInsideBackpack => itemInsideBackpack.inventoryItemData == inventoryItemData);
            resultSource = storageItemData.storageInventoryData;

            return result != null;
        });

        inventoryItemSource = resultSource;
        return result;
    }

    public List<InventoryData> GetInventoryDataListIncludingBackpacks()
    {
        var result = new List<InventoryData> { this };

        inventory.ForEach(inventoryItem => 
        {
            if (inventoryItem.inventoryItemData is not StorageItemData)
                return;

            StorageItemData storageItemData = (StorageItemData) inventoryItem.inventoryItemData;
            result.Add(storageItemData.storageInventoryData);
        });

        return result;
    }
    
    public InventoryItem GetCurrentlySelectedSlotItem() 
    {
        if (currentlySelectedSlotNumber == 0)
            return null;

        if (inventory.Count == 0)
            return null;

        if (currentlySelectedSlotNumber > inventory.Count)
            return null;
            
        return inventory[currentlySelectedSlotNumber - 1];
    }
    
    private InventoryItem CreateInventoryItemFromData(InventoryItemData inventoryItemData, int quantity)
    {
        if (inventoryItemData is ToolItemData)
            return new InventoryItemTool(inventoryItemData, InventoryManager.instance.GetRandomToolPrefix(), quantity);

        if (inventoryItemData is InventoryItemDataBreakable)
            return new InventoryItemBreakable(inventoryItemData, quantity);
            
        return new InventoryItem(inventoryItemData, quantity);
    }
    
    public bool ItemExistsInsideInventoryOrBags(InventoryItemData itemData) 
    {
        if (inventory.Exists(x => x.inventoryItemData == itemData))
            return true;
            
        var result = false;
        foreach (var inventorySlot in inventory)
        {
            if (inventorySlot.inventoryItemData is not StorageItemData storageItemData)
                continue;

            if (storageItemData.storageInventoryData.inventory.Exists(x => x.inventoryItemData == itemData))
            {
                result = true;
                break;
            }
        }
        
        return result;
    }
}