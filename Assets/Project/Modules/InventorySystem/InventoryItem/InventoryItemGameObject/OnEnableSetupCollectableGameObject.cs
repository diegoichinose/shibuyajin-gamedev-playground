using UnityEngine;

public class OnEnableSetupCollectableGameObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer collectableSprite;
    [SerializeField] private SpriteRenderer collectableSpriteOutline;
    [SerializeField] private CollectableInputTrigger _collectableInputTrigger;
    [SerializeField] private InventoryItem inventoryItem;

    void OnEnable()
    {
        if (inventoryItem == null || inventoryItem.inventoryItemData == null || inventoryItem.quantity == 0)
            return;

        SetupCollectable(inventoryItem);
    }

    void OnDisable()
    {
        inventoryItem = null;
    }

    public void SetupCollectable(InventoryItem inventoryItem)
    {
        _collectableInputTrigger.SetInventoryItem(inventoryItem);
        collectableSprite.sprite = inventoryItem.inventoryItemData.icon;
        collectableSpriteOutline.sprite = inventoryItem.inventoryItemData.icon;
    }
}
