using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlotDragAndDrop : MonoBehaviour, IDragHandler,  IBeginDragHandler, IEndDragHandler
{
    public InventoryData inventoryData;
    private InventorySlot _thisInventorySlot;
    private InventorySlot _targetInventorySlot;
    private InventoryItemDropZone _dropZone;
    private InventoryItemDropZone _dropZoneCache;
    private InventoryDrawerBase _targetInventoryDrawer;
    private CanvasGroup _thisCanvasGroup;
    private InputAction _inputMousePosition;
    private Vector3 originalPosition;

    private bool IsMouseOverInventoryDrawer(PointerEventData eventData) => eventData.pointerEnter && eventData.pointerEnter.TryGetComponent<InventoryDrawerBase>(out _targetInventoryDrawer);
    private bool IsMouseOverInventorySlot(PointerEventData eventData) => eventData.pointerEnter && eventData.pointerEnter.TryGetComponent<InventorySlot>(out _targetInventorySlot);
    private bool IsMouseOverDropZone(PointerEventData eventData) => eventData.pointerEnter && eventData.pointerEnter.TryGetComponent<InventoryItemDropZone>(out _dropZone);

    void Start()
    {
        _thisInventorySlot = GetComponent<InventorySlot>();
    }

    void OnEnable()
    { 
        _inputMousePosition = new MyInputActions().PlayerActionMap.Aim;
        _inputMousePosition.Enable(); 
    }

    void OnDisable()
    { 
        _inputMousePosition.Disable(); 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.localPosition;
        AudioManager.instance.sfxLibrary.PlayInventoryDragSound();

        if (_thisInventorySlot.inventoryItem.inventoryItemData is StorageItemData)
        if (_thisInventorySlot.inventoryData.isMainInventoryData)
            InventoryManager.instance.CloseExtraInventoryUI();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _thisInventorySlot.SelectThis();
        transform.position = CameraManager.instance.GetScreenToWorldPoint(_inputMousePosition.ReadValue<Vector2>());

        if (_thisCanvasGroup == null)
            _thisCanvasGroup = gameObject.AddComponent<CanvasGroup>();

        _thisCanvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        if (IsMouseOverInventoryDrawer(eventData))
        {
            if (inventoryData != _targetInventoryDrawer.inventoryData)
            if (_targetInventoryDrawer.inventoryData.TryAddOrIncrementItemToInventory(_thisInventorySlot.inventoryItem, playWarningTooltip: false, checkForStorageItems: false))
            {
                inventoryData.RemoveEntireItemStackFromInventory(_thisInventorySlot.inventoryItem, dropItem: false);
                AudioManager.instance.sfxLibrary.PlayInventoryDropSound();
                return;
            }
            ResetToOriginalPosition();
            return;
        }
        
        if (IsMouseOverInventorySlot(eventData))
        {
            if (_targetInventorySlot.inventoryData != _thisInventorySlot.inventoryData)
            {
                if (_targetInventorySlot.inventoryData.TryAddOrIncrementItemToInventory(_thisInventorySlot.inventoryItem, playWarningTooltip: false, checkForStorageItems: false))
                {
                    inventoryData.RemoveEntireItemStackFromInventory(_thisInventorySlot.inventoryItem, dropItem: false);
                    AudioManager.instance.sfxLibrary.PlayInventoryDropSound();
                    return;
                }
                
                ResetToOriginalPosition();
                return;
            }

            MoveItemToTargetSlot(_targetInventorySlot);
            AudioManager.instance.sfxLibrary.PlayInventoryDropSound();
            return;
        }


        if (IsMouseOverDropZone(eventData))
        {
            if (_dropZoneCache && _dropZoneCache.OnDrop(_thisInventorySlot.inventoryItem))
                inventoryData.TryConsumeItemFromInventory(_thisInventorySlot.inventoryItem.inventoryItemData, amount: 1);
            else
                ResetToOriginalPosition();

            return;
        }

        DropItem();
    }

    private void MoveItemToTargetSlot(InventorySlot targetInventoryslot) 
    {
        int oldIndex = _thisInventorySlot.thisInventorySlotNumber - 1;
        int newIndex = targetInventoryslot.thisInventorySlotNumber - 1;
        Destroy(_thisCanvasGroup);

        inventoryData.SetSelectedSlotNumber(targetInventoryslot.thisInventorySlotNumber);
        inventoryData.MoveInventoryItem(oldIndex, newIndex);
    }

    private void ResetToOriginalPosition() 
    {
        if (_thisCanvasGroup == null)
            return;

        transform.localPosition = originalPosition;
        AudioManager.instance.sfxLibrary.PlayInventoryDropFailSound();
        Destroy(_thisCanvasGroup);
    }

    private void DropItem() 
    {
        Destroy(_thisCanvasGroup);
        inventoryData.RemoveEntireItemStackFromInventory(_thisInventorySlot.inventoryItem);
    }
}