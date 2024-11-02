using TMPro;
using UnityEngine;

public class PlayerTooltipManager : MonoBehaviour
{
    [SerializeField] private GameObject playerTooltipPrefab;
    [SerializeField] private float tooltipCooldown = 0.5f;
    private TMP_Text playerTooltipPrefabText;
    private float nextPlayTooltipTime;
    private bool CanPlayTooltip() => Time.time >= nextPlayTooltipTime;
    public static PlayerTooltipManager instance = null;

    void Awake()
    {
        if (instance != null)
            Debug.LogError("Found more than one PlayerTooltipManager in the scene");

        instance = this; 

        playerTooltipPrefabText = playerTooltipPrefab.GetComponentInChildren<TMP_Text>();
    }
    
    public void ShowPlayerTooltip(string text) => ShowPlayerTooltip(text, Color.white);
    public void ShowPlayerTooltip(string text, Color color, bool playTooltipSound = true, bool respectTooltipCooldown = false)
    {   
        if (playTooltipSound)
            AudioManager.instance.sfxLibrary.PlayPlayerTooltipSound();

        if (respectTooltipCooldown)
            if (!CanPlayTooltip())
                return;
        
        nextPlayTooltipTime = Time.time + tooltipCooldown; 

        playerTooltipPrefabText.text = text;
        playerTooltipPrefabText.color = color;

        Instantiate(playerTooltipPrefab, transform).transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}