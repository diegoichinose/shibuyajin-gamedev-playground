using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance = null;
    public Action OnTimePaused;
    public Action OnTimeResumed;
    public Action OnHarvestableTriggerExit;
    public Action OnHarvested;
    public Action OnHarvestTickWithTool;
    public Action OnDurabilityChanged;
    public Action<InventoryItem> OnDropItemRequested;
    public Action OnItemDropped;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }
}