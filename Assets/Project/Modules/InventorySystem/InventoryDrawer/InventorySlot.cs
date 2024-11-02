using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryData inventoryData;
    private MyInputActions _input;
    private Tweener anim;

    public InventoryItem inventoryItem;
    public Image raycastTargetImage;
    public Image icon;
    public TextMeshProUGUI count;
    public Image selectionHighlight;
    public GameObject durabilityUI;
    public Image durabilitySlider;
    public int thisInventorySlotNumber;
    public bool isSelected = false;
    public Action<int> OnSelect;
    public Action OnBeginDrag;

    void OnEnable()
    {
        _input.PlayerActionMap.Drop.performed += DropItem;
    }

    void OnDisable()
    {
        _input.Disable();
        _input.PlayerActionMap.Drop.performed -= DropItem;
    }

    void Awake()
    {
        _input = new MyInputActions();
        _input.Disable();
    }

    public void ClearSlot()
    {
        icon.enabled = false;
        count.enabled = false;
        durabilityUI.SetActive(false);
        raycastTargetImage.raycastTarget = false;
    }

    public void DrawSlot(InventoryItem item)
    {
        inventoryItem = item;

        if (inventoryItem == null || inventoryItem.inventoryItemData == null)
        {
            ClearSlot();
            return;
        }
        
        icon.enabled = true;
        icon.sprite = item.inventoryItemData.icon;
        raycastTargetImage.raycastTarget = true;

        if (item.inventoryItemData.canStack)
        {
            count.enabled = true;
            count.text = item.quantity.ToString();
        }

        if (item is InventoryItemBreakable)
        {
            durabilityUI.SetActive(true);
            UpdateDurabilityUI();
        }
    }

    public void UpdateDurabilityUI() 
    {
        if (inventoryItem is not InventoryItemBreakable)
            return;

        durabilitySlider.fillAmount = ((InventoryItemBreakable) inventoryItem).currentDurability / ((InventoryItemDataBreakable)inventoryItem.inventoryItemData).maxDurability;
    }

    public void OnSelected()
    {
        inventoryData.SetSelectedSlotNumber(thisInventorySlotNumber);
        selectionHighlight.enabled = true;
        isSelected = true;
        
        inventoryData.OnInventorySlotSelected.Invoke(inventoryItem);

        if (inventoryItem == null)
            return;

        if (inventoryItem.inventoryItemData == null)
            return;
            
        if (anim == null || !anim.IsActive())
            anim = transform.DOShakeScale(0.3f, 0.5f, 10, 90, true);
            
        _input.Enable();
    }

    public void OnDeselect()
    {
        selectionHighlight.enabled = false;
        isSelected = false;

        _input.Disable();
        InventoryManager.instance.HideTooltip();
        inventoryData.OnInventorySlotDeselected.Invoke(inventoryItem);
    }

    private void DropItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        inventoryData.RemoveEntireItemStackFromInventory(inventoryItem);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectThis();
        AudioManager.instance.sfxLibrary.PlayInventorySlotSelectSound();

        if (eventData.button == PointerEventData.InputButton.Right)
            InventorySlotRightClickListener.instance.OnRightClickInventorySlotInvoke(inventoryItem, inventoryData);
    }

    public void SelectThis() => OnSelect?.Invoke(thisInventorySlotNumber);

    public void OnPointerEnter(PointerEventData eventData) => InventoryManager.instance.ShowTooltip(inventoryItem);
    public void OnPointerExit(PointerEventData eventData) => InventoryManager.instance.HideTooltip();
}