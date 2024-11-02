using UnityEngine;

public class PauseResumeTimeManager : MonoBehaviour
{
    public static PauseResumeTimeManager instance = null;

    [Header("DO NOT TOUCH - FOR INSPECTION ONLY")]
    [SerializeField] private int timePausingLayers = 0;
    [field: SerializeField] public bool isPaused { get; private set; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void PauseTime() 
    {
        timePausingLayers++;
        // Time.timeScale = 0f;
        isPaused = true;
        GameEventsManager.instance.OnTimePaused?.Invoke();
    }

    public void TryResumeTime() 
    {
        if (timePausingLayers <= 1)
        {
            GameEventsManager.instance.OnTimeResumed?.Invoke();
            // Time.timeScale = 1f;
            isPaused = false;
        }
        
        timePausingLayers--;
        if (timePausingLayers < 0)
            timePausingLayers = 0;
    }
    
    public void ForceResumeTime() 
    {
        GameEventsManager.instance.OnTimeResumed?.Invoke();
        // Time.timeScale = 1f;
        timePausingLayers = 0;
        isPaused = false;
    }
}