using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private ToolPrefixDataList _toolPrefixDataList;
    [SerializeField] private GameObject extraInventoryContainer;
    [SerializeField] private Image extraInventoryDrawerSprite;
    [SerializeField] private InventoryDrawerExtra _extraInventoryDrawer;
    [SerializeField] private InventoryDrawerExtra _storageChestDrawer;
    [SerializeField] private GameObject storageChestContainerUI;
    [SerializeField] private InventoryTooltip _inventoryTooltipUI;

    public void ShowTooltip(InventoryItemData inventoryItemData) => _inventoryTooltipUI.ShowTooltip(inventoryItemData);
    public void ShowTooltip(InventoryItem inventoryItem) => _inventoryTooltipUI.ShowTooltip(inventoryItem);
    public void HideTooltip() => _inventoryTooltipUI.HideTooltip();
    public ToolPrefixData GetRandomToolPrefix() => _toolPrefixDataList.GetRandom();
    public ToolPrefixData FindToolPrefix(SerializableGuid id) => _toolPrefixDataList.Find(id);

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
    
    void Start()
    {
        _inventoryData.OnInventoryItemRemoved += TryCloseExtraInventoryUI;
        GameEventsManager.instance.OnHarvestTickWithTool += TryReduceSelectedItemDurability;
    }

    void OnDestroy()
    {
        _inventoryData.OnInventoryItemRemoved -= TryCloseExtraInventoryUI;
        GameEventsManager.instance.OnHarvestTickWithTool -= TryReduceSelectedItemDurability;
    }

    public void OpenCloseExtraInventoryUI(StorageItemData storageItemData)
    {
        if (storageItemData.storageInventoryData != _extraInventoryDrawer.inventoryData)
        {
            OpenExtraInventoryUI(storageItemData);
            return;
        }

        if (extraInventoryContainer.activeSelf)
            CloseExtraInventoryUI();
        else
            OpenExtraInventoryUI(storageItemData);
    }

    public void OpenExtraInventoryUI(StorageItemData storageItemData) 
    {
       extraInventoryDrawerSprite.sprite = storageItemData.storageDrawerSprite;
       extraInventoryDrawerSprite.SetNativeSize();
       extraInventoryContainer.SetActive(true);

        _extraInventoryDrawer.SetInventoryData(storageItemData.storageInventoryData);

        AudioManager.instance.sfxLibrary.PlayAdditionalStorageOpenSound();
    }

    private void TryCloseExtraInventoryUI(InventoryItem inventoryItem)
    {
        InventoryItemData itemData = inventoryItem.inventoryItemData;

        if (itemData == null)
            return;
            
        if (itemData is not StorageItemData)
            return;

        CloseExtraInventoryUI();
    }

    public void CloseExtraInventoryUI()
    {
        extraInventoryContainer.SetActive(false);
        AudioManager.instance.sfxLibrary.PlayAdditionalStorageCloseSound();
    }

    public void OpenStorageChestUI(StorageChestData storageChestData, List<InventoryItemToPersist> items)
    {
        if (storageChestContainerUI.activeSelf)
            return;

        AudioManager.instance.sfxLibrary.PlayStorageChestOpenSound();
            
        InventoryData inventoryData = storageChestData.CreateInventoryDataInstance(items);
        _storageChestDrawer.SetInventoryData(inventoryData);
        storageChestContainerUI.SetActive(true);
    }

    public void CloseStorageChestUI(StorageChestData storageChestData, SpawnedPropStorageChest spawnedPropStorageChest)
    {
        storageChestContainerUI.SetActive(false);
        AudioManager.instance.sfxLibrary.PlayStorageChestCloseSound();
        
        spawnedPropStorageChest.items = storageChestData.GetItemListFromInventoryDataInstance();
    }
    
    private void TryReduceSelectedItemDurability()
    {
        InventoryItem item = _inventoryData.GetCurrentlySelectedSlotItem();

        if (item == null)
            return;

        if (item is not InventoryItemBreakable)
            return;

        InventoryItemBreakable breakableItem = (InventoryItemBreakable) item;

        breakableItem.currentDurability--;
        GameEventsManager.instance.OnDurabilityChanged?.Invoke();
        
        if (breakableItem.currentDurability <= 0)
        {
            _inventoryData.TryConsumeItemFromInventory(item.inventoryItemData, amount: 1);
            AudioManager.instance.sfxLibrary.PlayOnItemBreakSound();
            PlayerTooltipManager.instance.ShowPlayerTooltip("MY " + item.inventoryItemData.title.ToUpper() + " SHATTERED!", Color.red);
        }
    }

    public int TryGetCurrentToolPrefixAdditionalHarvest()
    {
        var selectedItem = _inventoryData.GetCurrentlySelectedSlotItem();

        if (selectedItem is InventoryItemTool inventoryItemTool)
            return inventoryItemTool.prefixData.additionalHarvest + inventoryItemTool.toolItemData.additionalHarvest;

        return 0;
    }
    
    public float TryGetCurrentToolPrefixSpeed()
    {
        var selectedItem = _inventoryData.GetCurrentlySelectedSlotItem();

        if (selectedItem is InventoryItemTool inventoryItemTool)
            return inventoryItemTool.prefixData.speed + inventoryItemTool.toolItemData.tickSpeed;

        return 0f;
    }
    
    public float TryGetCurrentToolPrefixTickPower()
    {
        var selectedItem = _inventoryData.GetCurrentlySelectedSlotItem();

        if (selectedItem is InventoryItemTool inventoryItemTool)
            return inventoryItemTool.prefixData.tickPower + inventoryItemTool.toolItemData.tickPower;

        return 0f;
    }
    
    public List<InventoryItemData> GetRandom(List<InventoryItemData> list, int amount = 1)
    {
        var result = new List<InventoryItemData>();
        var listToUse = list.FindAll(x => x.CanSpawn()).ToList();
        var randomIndex = 0;
        
        for (int i = 0; i < amount; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, listToUse.Count - 1);
            result.Add(listToUse[randomIndex]);
            listToUse.RemoveAt(randomIndex);
        }

        return result;
    }

    public List<InventoryItemData> GetWeightedRandom(List<InventoryItemData> list, int minAmount, int additionalAmountToRandomize)
    {
        var listToUse = list.FindAll(x => x.CanSpawn()).ToList();

        int amount = minAmount + UnityEngine.Random.Range(0, additionalAmountToRandomize);

        if (amount == 0)
            return new List<InventoryItemData>();
            
        int chanceTotal = 0;
        listToUse.ForEach(item => 
        {
            if (item.rarityWeight <= 0)
                item.rarityWeight = 1;

            chanceTotal += item.rarityWeight;
        });
        
        List<InventoryItemData> result = new List<InventoryItemData>();

        for (int i = 0; i < amount; i++)
        {
            int randomPercentageRoll = UnityEngine.Random.Range(1, chanceTotal);
            int chanceSumCounter = 0;
            
            listToUse.ForEach(item =>
            {
                if (randomPercentageRoll > chanceSumCounter)
                if (randomPercentageRoll <= chanceSumCounter + item.rarityWeight)
                {
                    result.Add(item);
                    item.OnSpawnConfirmed();
                }

                chanceSumCounter += item.rarityWeight;
            });
        }
        
        return result;
    }
}