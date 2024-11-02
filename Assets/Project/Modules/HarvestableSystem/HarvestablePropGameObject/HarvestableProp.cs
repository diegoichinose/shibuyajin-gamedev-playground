using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HarvestableProp : MonoBehaviour
{
    [SerializeField] protected InventoryData _inventoryData;
    [SerializeField] protected HarvestablePropData _harvestablePropData;
    [SerializeField] protected PlayerData _playerData;
    [SerializeField] protected SpawnedPropPersistency _spawnedPropPersistency;
    public GameObject onHarvestDestroyTarget; // THIS CAN BE SETUP BY A LAZY LOADER (TO DESTROY LAZY LOAD PARENT ON HARVEST INSTEAD OF THIS PROP)

    public Action OnThisPropHarvested;
    public Action OnInteractionEnabled;
    public Action OnInteractionDisabled;

    [SerializeField] protected GameObject harvestablePropSprite;
    [SerializeField] protected GameObject progressBarTooltip;
    [SerializeField] protected Image progressBarFill;
    [SerializeField] protected GameObject emptyCollectableItemPrefab;
    [SerializeField] protected bool resetProgressOnTriggerExit;
    [SerializeField] protected float interactCooldown = 0.8f;

    protected MyInputActions _input;
    protected Tweener propAnimation;
    protected Tweener progressBarAnimation;
    protected float playerAnimationDuration;
    protected float nextInteractTime; 
    protected bool isHoldingInteract = false;
    protected bool isPlayerNearHarvestableProp = false;
    protected float durabilityTicksCompleted = 0;
	protected Coroutine timeStoppingCoroutine;

    protected bool CanInteract() => Time.time >= nextInteractTime; 
    protected void OnInteract(InputAction.CallbackContext input) => isHoldingInteract = !isHoldingInteract;
    protected void OnInteractRelease(InputAction.CallbackContext input)  => isHoldingInteract = false;
    protected bool IsProgressCompleted() => durabilityTicksCompleted >= _harvestablePropData.durabilityTicks;
    protected float GetProgressPercentageToCompletion() => durabilityTicksCompleted/_harvestablePropData.durabilityTicks;
    protected void SetProgressBarFill(float percentage, float delay = 0) 
    {
        progressBarAnimation = DOTween.To(() => progressBarFill.fillAmount, x => progressBarFill.fillAmount = x, percentage, 0.2f).SetDelay(delay);
    }
    
    void Awake()
    {
        _input = new MyInputActions();
        _harvestablePropData.requiredTool = null;
    }

    void OnDisable()
    {
        if (progressBarAnimation != null)
            progressBarAnimation.Kill();

        propAnimation.Kill();
        progressBarFill.DOKill();
        harvestablePropSprite.transform.DOKill();

        UnsubscribeInteractionTriggers();
    }

	void OnTriggerEnter2D(Collider2D other)
	{
        if (!other.CompareTag("Player"))
            return;

        SubscribeInteractionTriggers();
        isPlayerNearHarvestableProp = true;

        TryEnableInteraction();
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
            return;

        if (resetProgressOnTriggerExit)
            ResetProgress();

        DisableInteraction();
        UnsubscribeInteractionTriggers();
        isPlayerNearHarvestableProp = false;

        GameEventsManager.instance.OnHarvestableTriggerExit?.Invoke();
    }
    
    protected void TryEnableInteraction(InventoryItem inventoryitem) => TryEnableInteraction();
    protected void TryEnableInteraction()
    {
        if (!isPlayerNearHarvestableProp)   {DisableInteraction(); return;}
        if (IsProgressCompleted())          {DisableInteraction(); return;}
        if (!IsRequiredToolEquipped())      {DisableInteraction(); return;}
        
        if (progressBarTooltip.activeSelf)
            return;

        _input.Enable();
        SetCustomCursor();
        progressBarTooltip.SetActive(true);
        OnInteractionEnabled?.Invoke();
    }

    protected void DisableInteraction()
    {
        if (progressBarTooltip.activeSelf == false)
            return;
             
        _input.Disable();
        ResetCustomCursor();
        progressBarTooltip.SetActive(false);
        OnInteractionDisabled?.Invoke();
    }

    void Update()
    {
        if(!isHoldingInteract)
            return;
            
        if(!CanInteract())
            return;

        if (IsProgressCompleted())
            return;

        HarvestTick();

        var cooldown = GetInteractCooldownWithModifiers();

        if (cooldown <= 0f)
            cooldown = 0.1f;

        nextInteractTime = Time.time + cooldown; 
        
        if (IsProgressCompleted())
            CompleteHarvest();
    }

    protected virtual void HarvestTick()
    {    
        durabilityTicksCompleted = durabilityTicksCompleted + 1 + InventoryManager.instance.TryGetCurrentToolPrefixTickPower();

        PlayHarvestingAnimation();
        SetProgressBarFill(GetProgressPercentageToCompletion());

        if (_harvestablePropData.requiredTool != null)
            GameEventsManager.instance.OnHarvestTickWithTool.Invoke();
    }

    protected void PlayHarvestingAnimation()
    {
        PlayPropBouncingAnimation();
        CameraManager.instance.ShakeCameraModerate();
        AudioManager.instance.sfxLibrary.PlayHarvestableTickSound(_harvestablePropData.harvestTickSound);
    }

    protected virtual void CompleteHarvest()
    {
        CameraManager.instance.ShakeCameraStrong();

        DisableInteraction();
        TryDropItems();
        AudioManager.instance.sfxLibrary.PlayHarvestableCompleteSound(_harvestablePropData.harvestCompleteSound);

        GameEventsManager.instance.OnHarvested.Invoke();
        OnThisPropHarvested?.Invoke();

        harvestablePropSprite.SetActive(false);
        DestroySelf();

        UnsubscribeInteractionTriggers();
    }

    protected void PlayPropBouncingAnimation(float delay = 0)
    {
        if (propAnimation == null || !propAnimation.IsActive())
            propAnimation = harvestablePropSprite.transform.DOShakeScale(0.2f, new Vector3(0.5f, 0.5f, 0), 10, 90, false).SetDelay(delay);
    }

    protected void TryDropItems()
    {   
        PathHelper pathHelper = new PathHelper();
        
        emptyCollectableItemPrefab.SetActive(false);
        _harvestablePropData.itemsToDrop.GetWeightedRandom().ForEach(itemToDrop => 
        {
            if (itemToDrop == null)
                return;
                
            GameObject item = Instantiate(emptyCollectableItemPrefab, gameObject.transform.position, gameObject.transform.rotation); 
            var itemToDropCasted = itemToDrop.CastToInventoryItemToPersist().CastToInventoryItem(itemToDrop);
            item.GetComponent<OnEnableSetupCollectableGameObject>().SetupCollectable(itemToDropCasted);
            item.SetActive(true);
            
            item.transform
                .DOMove(pathHelper.GetRandomNearbyPosition(origin: transform.position), 0.3f)
                .SetEase(Ease.OutExpo);
        });
        emptyCollectableItemPrefab.SetActive(true);
    }
    
    protected bool IsRequiredToolEquipped() 
    {
        if (_harvestablePropData.requiredToolType is ToolType.None)
            return true;

        InventoryItem equippedTool = _inventoryData.GetCurrentlySelectedSlotItem();
        
        if (equippedTool == null)
            return false;

        if (equippedTool is not InventoryItemTool inventoryItemTool)
            return false;

        if (inventoryItemTool.toolItemData.toolType != _harvestablePropData.requiredToolType)
            return false;

        _harvestablePropData.requiredTool = inventoryItemTool;

        return true;
    }

    protected void SetCustomCursor()
    {
        if (_harvestablePropData.requiredTool != null)
            CursorManager.instance.SetCustomCursor(_harvestablePropData.requiredTool.toolItemData.customCursor);
        else
            CursorManager.instance.SetHandOpenedCursor();
    }
    
    protected void ResetCustomCursor()
    {
        if (_harvestablePropData.requiredTool != null)
            CursorManager.instance.ResetCursorIfThisCursorIsSet(_harvestablePropData.requiredTool.toolItemData.customCursor);
        else
            CursorManager.instance.ResetHandCursor();
    }

    protected void ResetProgress()
    {
        durabilityTicksCompleted = 0;
        SetProgressBarFill(GetProgressPercentageToCompletion());
    }
    
    protected void SubscribeInteractionTriggers()
    {
        _input.PlayerActionMap.Interact.performed += OnInteract;
        _input.PlayerActionMap.Interact.canceled += OnInteractRelease;
        _input.PlayerActionMap.Fire1.performed += OnInteract;
        _input.PlayerActionMap.Fire1.canceled += OnInteractRelease;
        
        _inventoryData.OnInventorySlotSelected += TryEnableInteraction;
        _inventoryData.OnInventoryItemRemoved += TryEnableInteraction;

        GameEventsManager.instance.OnHarvested += TryEnableInteraction;
        GameEventsManager.instance.OnItemDropped += TryEnableInteraction;
        GameEventsManager.instance.OnHarvestableTriggerExit += TryEnableInteraction;
    }

    protected void UnsubscribeInteractionTriggers() 
    {
        _input.Disable();
        _input.PlayerActionMap.Interact.performed -= OnInteract;
        _input.PlayerActionMap.Interact.canceled -= OnInteractRelease;
        _input.PlayerActionMap.Fire1.performed -= OnInteract;
        _input.PlayerActionMap.Fire1.canceled -= OnInteractRelease;

        _inventoryData.OnInventorySlotSelected -= TryEnableInteraction;
        _inventoryData.OnInventoryItemRemoved -= TryEnableInteraction;

        GameEventsManager.instance.OnHarvested -= TryEnableInteraction;
        GameEventsManager.instance.OnItemDropped -= TryEnableInteraction;
        GameEventsManager.instance.OnHarvestableTriggerExit -= TryEnableInteraction;
    }

    protected void DestroySelf()
    {
        if (_spawnedPropPersistency)
            _spawnedPropPersistency.RemoveSelfFromSaveData();
        
        if (onHarvestDestroyTarget)
            Destroy(onHarvestDestroyTarget, 0.1f);
        else
            Destroy(gameObject, 0.1f);
    } 
    
    protected float GetInteractCooldownWithModifiers() 
    {
        var result = interactCooldown;
        
        if (_harvestablePropData.requiredTool != null)
            result = result - (interactCooldown * (_harvestablePropData.requiredTool.prefixData.speed + _harvestablePropData.requiredTool.toolItemData.tickSpeed));

        return result;
    }
}