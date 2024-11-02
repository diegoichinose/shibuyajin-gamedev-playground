using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryDrawerBase : MonoBehaviour
{
    [field: SerializeField] public InventoryData inventoryData { protected set; get; }
    [SerializeField] protected GameObject slotPrefab;
    protected List<InventorySlot> inventorySlots;
    protected Coroutine drawInventoryInstance = null;

    public void SetInventoryData(InventoryData inventoryData)
    {
        if (this.inventoryData != null)
            this.inventoryData.OnInventoryChange -= DrawInventory;
            
        this.inventoryData = inventoryData;
        this.inventoryData.OnInventoryChange += DrawInventory;

        DrawInventory();
    }

    void OnEnable()
    {
        if (inventoryData == null)
            return;

        inventoryData.OnInventoryChange += DrawInventory;
        DrawInventory();
    }

    void OnDisable()
    {
        if (inventoryData == null)
            return;
            
        inventoryData.OnInventoryChange -= DrawInventory;
    }
    
    protected void DrawInventory()
    {
        if (gameObject.activeInHierarchy == false)
            return;

        if (drawInventoryInstance != null)
            return;

        // INFO: THIS IMPROVES PERFORMANCE WHEN PICKING 9999 ITEMS ON THE FLOOR AT ONCE (IT WILL UPDATE UI ONLY ONCE INSTEAD)
        drawInventoryInstance = StartCoroutine(DrawInventoryCoroutine());
    }

	protected IEnumerator DrawInventoryCoroutine() 
	{
		yield return new WaitForNextFrameUnit();
        RedrawInventorySlots();
	} 

	protected virtual void RedrawInventorySlots()
	{
        DestroyInventorySlots();
        InstantiateInventorySlots();

        for (int i = 0; i < inventoryData.inventory.Count; i++)
        {
            inventorySlots[i].DrawSlot(inventoryData.inventory[i]);
        }
        
        drawInventoryInstance = null;
	} 

    protected void DestroyInventorySlots()
    {
        foreach (Transform children in transform)
        {
            DOTween.Kill(children);
            Destroy(children.gameObject);
        }

        inventorySlots = new List<InventorySlot>(inventoryData.capacity);
    }
    
    protected void UpdateSlotDurabilityUI() => inventorySlots[inventoryData.currentlySelectedSlotNumber - 1].UpdateDurabilityUI();

    protected virtual void InstantiateInventorySlots()
    {
        slotPrefab.GetComponent<InventorySlot>().inventoryData = inventoryData;
        slotPrefab.GetComponent<InventorySlotDragAndDrop>().inventoryData = inventoryData;
    }
}