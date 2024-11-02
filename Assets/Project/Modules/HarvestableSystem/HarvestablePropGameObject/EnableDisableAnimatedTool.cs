using UnityEngine;

public class EnableDisableAnimatedTool : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private GameObject toolSpriteContainer;
    [SerializeField] private SpriteRenderer _toolSpriteRenderer;

    void OnEnable()
    {
        toolSpriteContainer.SetActive(false);
        _inventoryData.OnInventorySlotSelected += TryEnableToolSprite;
        _inventoryData.OnInventorySlotDeselected += TryDisableToolSprite;
        _inventoryData.OnInventoryItemRemoved += TryDisableToolSprite;
    }

    void OnDisable()
    {
        _inventoryData.OnInventorySlotSelected -= TryEnableToolSprite;
        _inventoryData.OnInventorySlotDeselected -= TryDisableToolSprite;
        _inventoryData.OnInventoryItemRemoved -= TryDisableToolSprite;
    }

    private void TryEnableToolSprite(InventoryItem inventoryItem)
    {
        toolSpriteContainer.SetActive(false);
        InventoryItemData itemData = inventoryItem.inventoryItemData;

        if (itemData == null)
            return;

        if (itemData is not ToolItemData)
            return;

        _toolSpriteRenderer.sprite = itemData.icon;
        toolSpriteContainer.SetActive(true);
    }

    private void TryDisableToolSprite(InventoryItem inventoryItem)
    {
        InventoryItemData itemData = inventoryItem.inventoryItemData;

        if (itemData == null)
            return;
            
        if (itemData is not ToolItemData)
            return;

        toolSpriteContainer.SetActive(false);
    }
}