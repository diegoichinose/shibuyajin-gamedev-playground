using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryItemDropZone : MonoBehaviour
{
    [SerializeField] private List<Image> imagesToChangeColor;
    [SerializeField] private TMP_Text textToChangeColor;
    [SerializeField] private Color originalColor;
    [SerializeField] private Color onHoverColor;

    public void OnDragOverZone()
    {
        imagesToChangeColor.ForEach(image => image.color = onHoverColor);
        
        if (textToChangeColor) 
            textToChangeColor.color = onHoverColor;
    }

    public void OnDragLeaveZone() => ReturnToOriginalColor();
    void OnDisable() => ReturnToOriginalColor();

    private void ReturnToOriginalColor()
    {
        imagesToChangeColor.ForEach(image => image.color = originalColor);
        
        if (textToChangeColor) 
            textToChangeColor.color = originalColor;
    }

    public bool OnDrop(InventoryItem inventoryItem)
    {
        ReturnToOriginalColor();
        return TryOnDrop(inventoryItem);
    }
    
    public abstract bool TryOnDrop(InventoryItem inventoryItem);
}