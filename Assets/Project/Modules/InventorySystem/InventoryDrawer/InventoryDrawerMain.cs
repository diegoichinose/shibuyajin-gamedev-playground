using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDrawerMain : InventoryDrawerBase
{
    private MyInputActions _input;
    private bool IsSlotAlreadySelected(int slotIndex) => inventorySlots[slotIndex].isSelected;

    void Awake()
    {
        _input = new MyInputActions();
        _input.Enable();
    }

    void Start()
    {
        _input.PlayerActionMap.Numkeys.performed += SelectInventorySlotFromInput;
        SubscribeInputNavigation();

        GameEventsManager.instance.OnDurabilityChanged += UpdateSlotDurabilityUI;

        inventoryData.OnInventoryChange += DrawInventory;
    }
    
    void OnEnable()
    {
        drawInventoryInstance = null;
        DrawInventory();
    }

    void OnDestroy()
    {
        _input.Disable();
        _input.PlayerActionMap.Numkeys.performed -= SelectInventorySlotFromInput;
        UnsubscribeInputNavigation();

        GameEventsManager.instance.OnDurabilityChanged -= UpdateSlotDurabilityUI;
        
        inventoryData.OnInventoryChange -= DrawInventory;
        drawInventoryInstance = null;
    }

    private void SubscribeInputNavigation()
    {
        _input.PlayerActionMap.NavigateLeft.performed += SelectPreviousInventorySlotFromInput;
        _input.PlayerActionMap.NavigateRight.performed += SelectNextInventorySlotFromInput;
    }

    private void UnsubscribeInputNavigation()
    {
        _input.PlayerActionMap.NavigateLeft.performed -= SelectPreviousInventorySlotFromInput;
        _input.PlayerActionMap.NavigateRight.performed -= SelectNextInventorySlotFromInput;
    }

	protected override void RedrawInventorySlots() 
	{
        base.RedrawInventorySlots();
        SelectInventorySlot(slotNumber: inventoryData.currentlySelectedSlotNumber);
	} 

    protected override void InstantiateInventorySlots()
    {
        base.InstantiateInventorySlots();

        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab);
            newSlot.transform.SetParent(transform, false);
            
            InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();
            newSlotComponent.ClearSlot();
            newSlotComponent.thisInventorySlotNumber = i + 1;
            newSlotComponent.OnSelect += SelectInventorySlot;
            
            inventorySlots.Add(newSlotComponent);
        }
    }

    public void SelectInventorySlot(int slotNumber)
    {       
        if (slotNumber <= 0)
            slotNumber = inventorySlots.Count;

        if (slotNumber > inventorySlots.Count)
            slotNumber = 1;

        int slotIndex = slotNumber - 1;

        if (IsSlotAlreadySelected(slotIndex))
            return;
        
        inventorySlots[inventoryData.currentlySelectedSlotNumber - 1].OnDeselect();
        inventorySlots[slotIndex].OnSelected();
    }

    private void SelectInventorySlotFromInput(InputAction.CallbackContext context)
    {
        int.TryParse(context.control.name, out int numKeyValue);
        
        if (numKeyValue == 0)
            numKeyValue = 10;

        SelectInventorySlot(numKeyValue);
    }
    
    private void SelectPreviousInventorySlotFromInput(InputAction.CallbackContext context) 
    {
        SelectInventorySlot(inventoryData.currentlySelectedSlotNumber - 1);
        AudioManager.instance.sfxLibrary.PlayInventorySlotSelectSound();
    } 

    private void SelectNextInventorySlotFromInput(InputAction.CallbackContext context) 
    {
        SelectInventorySlot(inventoryData.currentlySelectedSlotNumber + 1);
        AudioManager.instance.sfxLibrary.PlayInventorySlotSelectSound();
    }
}