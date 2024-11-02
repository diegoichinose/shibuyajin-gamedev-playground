using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryItemDropOnTheFloorListener : MonoBehaviour
{
    [SerializeField] private GameObject _emptyCollectableItemPrefab;
    [SerializeField] private List<Vector2> dropItemTowardsOneOfTheseLocalPositions;

    void Start()
    {
        GameEventsManager.instance.OnDropItemRequested += DropItem; 
    }
    
    void OnDestroy() 
    {
        GameEventsManager.instance.OnDropItemRequested -= DropItem; 
    }

    public void DropItem(InventoryItem inventoryItem)
    {
        GameObject item = Instantiate(_emptyCollectableItemPrefab, transform.position, transform.rotation); 
        item.GetComponent<OnEnableSetupCollectableGameObject>().SetupCollectable(inventoryItem);
        
        var targetDropPosition = dropItemTowardsOneOfTheseLocalPositions.GetRandom();
        
        item.transform
            .DOMove(new Vector2(transform.position.x + targetDropPosition.x, transform.position.y + targetDropPosition.y), 0.3f)
            .SetEase(Ease.OutExpo);
            
        GameEventsManager.instance.OnItemDropped?.Invoke();
        AudioManager.instance.sfxLibrary.PlayDropItemSoundSound();
    }
}