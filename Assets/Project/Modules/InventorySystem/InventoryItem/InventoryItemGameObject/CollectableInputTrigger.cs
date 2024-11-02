using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectableInputTrigger : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private GameObject TooltipPressInteract;
    public Action OnCollected;
    private MyInputActions _input;
    private InventoryItem inventoryItem;
    public void SetInventoryItem(InventoryItem inventoryItem) 
    {
        this.inventoryItem = inventoryItem;
    }

    void Awake()
    {
        _input = new MyInputActions();
    }

    public void OnDisable()
    {
        _input.Disable();
        _input.PlayerActionMap.Interact.performed -= Collect;
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        if(!other.CompareTag("Player"))
            return;

        _input.Enable();
        _input.PlayerActionMap.Interact.performed += Collect;
        TooltipPressInteract.SetActive(true);
    }
    
	void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
            return;

        _input.Disable();
        _input.PlayerActionMap.Interact.performed -= Collect;
        TooltipPressInteract.SetActive(false);
    }

    private void Collect(InputAction.CallbackContext context)
    {
        if(!_inventoryData.TryAddOrIncrementItemToInventory(inventoryItem)) 
            return;

        AudioManager.instance.sfxLibrary.PlayItemCollectedSound();
        OnCollected?.Invoke();

        Destroy(gameObject);
    }
}