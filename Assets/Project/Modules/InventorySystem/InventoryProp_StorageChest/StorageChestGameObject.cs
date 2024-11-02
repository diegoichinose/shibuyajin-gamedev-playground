using UnityEngine;
using UnityEngine.InputSystem;

public class StorageChestGameObject : MonoBehaviour
{
    [SerializeField] private StorageChestDataList _storageChestDataList;
    [SerializeField] private SpawnedPropPersistencyStorageChest _spawnedPropPersistency;
    [SerializeField] private Sprite closedChestSprite;
    [SerializeField] private Sprite openedChestSprite;
    [SerializeField] private StorageChestData storageChestData;
    [SerializeField] protected GameObject interactionTooltip;
    private MyInputActions _input;
    private bool isOpen;

    void Start()
    {
        isOpen = false;
        _storageChestDataList.isAnyStorageChestOpen = false;
    }

    void OnDestroy()
    {   
        if (_input != null)
        {
            _input.Disable();
            _input = null;
        }
    }	
    
    void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
            return;

        _input = new MyInputActions();
        _input.Enable();
        _input.PlayerActionMap.Interact.performed += TryOpen;
        
        interactionTooltip.SetActive(true);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
            return;

        if (_input != null)
        {
            _input.Disable();
            _input.PlayerActionMap.Interact.performed -= TryOpen;
            _input = null;
        }

        interactionTooltip.SetActive(false);
        TryClose();
    }

    protected void TryOpen(InputAction.CallbackContext context)
    {
        if (isOpen)
        {
            TryClose();
            return;
        }

        if (_storageChestDataList.isAnyStorageChestOpen)
            return;
        
        InventoryManager.instance.OpenStorageChestUI(storageChestData, _spawnedPropPersistency.spawnedPropStorageChest.items);
        GetComponentInChildren<SpriteRenderer>().sprite = openedChestSprite;
        _storageChestDataList.isAnyStorageChestOpen = true;
        isOpen = true;
    }

    private void TryClose()
    {
        if (_spawnedPropPersistency.spawnedPropStorageChest == null)
            return;
            
        if (isOpen == false)
            return;

        Close();
    }

    private void Close()
    {
        InventoryManager.instance.CloseStorageChestUI(storageChestData, _spawnedPropPersistency.spawnedPropStorageChest);
        GetComponentInChildren<SpriteRenderer>().sprite = closedChestSprite;

        isOpen = false;
        _storageChestDataList.isAnyStorageChestOpen = false;
    }
}