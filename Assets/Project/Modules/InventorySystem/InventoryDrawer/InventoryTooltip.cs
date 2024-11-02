using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TMP_Text itemTitle;
    [SerializeField] private TMP_Text itemSubtitle;
    [SerializeField] private TMP_Text itemDurability;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text prefixModifiers;

    void Start()
    {
        tooltip.SetActive(false);
    }

    public void ShowTooltip(InventoryItemData inventoryItemData)
    {
        ShowTooltip(inventoryItemData.CastToInventoryItemToPersist().CastToInventoryItem(inventoryItemData), inventoryItemData, isRealInventoryItem: false);
    }

    public void ShowTooltip(InventoryItem inventoryItem, InventoryItemData inventoryItemData = null, bool isRealInventoryItem = true)
    {
        if (inventoryItem == null)
            return;
            
        if (inventoryItem.inventoryItemData == null)
            return;

        if (inventoryItemData == null)
            inventoryItemData = inventoryItem.inventoryItemData;

        tooltip.SetActive(true);
        ResetTooltip();
        
        itemTitle.text = inventoryItemData.title.ToUpper();
        itemSubtitle.text = "";
        itemDescription.text = inventoryItemData.description;
        itemDurability.text = "";

        if (inventoryItemData.isMaterial)
            itemSubtitle.text = "MATERIAL";

        if (inventoryItemData is KeyItemData)
            itemSubtitle.text = "KEY ITEM";

        if (inventoryItemData is ConsumableItemData)
            itemSubtitle.text = isRealInventoryItem ? "CONSUMABLE (right-click)" : "CONSUMABLE";

        if (isRealInventoryItem)
        if (inventoryItem is InventoryItemBreakable breakableItem)
        {
            itemDurability.gameObject.SetActive(true);
            itemDurability.text = "DURABILITY " + breakableItem.currentDurability + "/" + ((InventoryItemDataBreakable) breakableItem.inventoryItemData).maxDurability + "<br>";
        }

        if (inventoryItemData is ToolItemData)
            SetupToolTooltip((InventoryItemTool) inventoryItem, isRealInventoryItem);

        if (inventoryItemData is StorageItemData)
            itemSubtitle.text = isRealInventoryItem ? "STORAGE (right-click)" : "STORAGE";
    }

    private void ResetTooltip()
    {
        itemDurability.gameObject.SetActive(false);

        if (prefixModifiers != null)
            prefixModifiers.gameObject.SetActive(false);
    }

    private void SetupToolTooltip(InventoryItemTool inventoryItem, bool isRealInventoryItem)
    {
        if (inventoryItem.prefixData == null)
            inventoryItem.prefixData = InventoryManager.instance.FindToolPrefix(inventoryItem.prefixDataId);

        if (isRealInventoryItem)
        {
            itemTitle.text = inventoryItem.prefixData.title.ToUpper() + " " + itemTitle.text;
            itemSubtitle.text = "TOOL (left-click)";
            prefixModifiers.gameObject.SetActive(true);
            prefixModifiers.text = GetPrefixDataDescription(inventoryItem.prefixData);
        }
        else 
        {
            itemTitle.text = itemTitle.text;
            itemSubtitle.text = "TOOL";
        }

    }

    public void HideTooltip() => tooltip.SetActive(false);
    public void OnPointerEnter(PointerEventData eventData) => tooltip.SetActive(true);
    public void OnPointerExit(PointerEventData eventData) => HideTooltip();

    public string GetPrefixDataDescription(ToolPrefixData prefix)
    {
        string description = "";

        if (prefix.speed > 0)                       description += "<br><color=green>+"  + prefix.speed                     * 100f + "% SPEED</color>";
        if (prefix.speed < 0)                       description += "<br><color=red>"     + prefix.speed                     * 100f + "% SPEED</color>";
        if (prefix.tickPower > 0)                   description += "<br><color=green>+"  + prefix.tickPower                 * 100f + "% TICK POWER</color>";
        if (prefix.tickPower < 0)                   description += "<br><color=red>"     + prefix.tickPower                 * 100f + "% TICK POWER</color>";
        if (prefix.durability > 0)                  description += "<br><color=green>+"  + prefix.durability                * 100f + "% DURABILITY</color>";
        if (prefix.durability < 0)                  description += "<br><color=red>"     + prefix.durability                * 100f + "% DURABILITY</color>";
        if (prefix.additionalHarvest > 0)           description += "<br><color=green>+"  + prefix.additionalHarvest + " HARVEST DROP</color>";
        if (prefix.additionalHarvest < 0)           description += "<br><color=red>"     + prefix.additionalHarvest + " HARVEST DROP</color>";

        return description;
    }
}