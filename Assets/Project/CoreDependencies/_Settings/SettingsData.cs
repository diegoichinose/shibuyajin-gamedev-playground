using System;
using UnityEngine;

[CreateAssetMenu(menuName = "My Scriptable Objects/Settings Data")]
public class SettingsData : ScriptableObject
{
    public Settings settings;
    public Action OnSettingsDataReset;
    public int AUDIO_MIXER_MIN = -40;
    public int AUDIO_MIXER_MAX = 0;
    
    public void ResetSettings()
    {
        settings = new Settings();
        OnSettingsDataReset?.Invoke();
    }
}

[Serializable]
public class Settings
{
    public int resolutionIndex;
    public FullScreenMode fullscreenMode;
    public GraphicsQuality graphicsQuality;
    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;
    public bool disableDamageNumbers;
    public bool disableScreenshake;
    public bool muteInBackground;
    public bool hideInstructions;
    public bool showTotemRange;

    public Settings()
    {
        fullscreenMode = FullScreenMode.FullScreenWindow;
        graphicsQuality = GraphicsQuality.Ultra;
        masterVolume = 0f;
        musicVolume = 0f;
        effectsVolume = 0f;
        disableDamageNumbers = false;
        disableScreenshake = false;
        muteInBackground = false;
        hideInstructions = false;
        showTotemRange = false;
    }
}

public enum GraphicsQuality
{
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh,
    Ultra
}