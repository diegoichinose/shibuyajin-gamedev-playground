using System.Collections.Generic;
using UnityEngine;

public class AudioSourceLibrarySfx : AudioSourceLibraryBase
{   
    [SerializeField] private AudioClip consumeHealthPotionSound;
    public void PlayConsumeHealthPotionSound() => PlaySound(consumeHealthPotionSound);

    [SerializeField] private AudioClip inventorySlotSelectSound;
    public void PlayInventorySlotSelectSound() => PlaySound(inventorySlotSelectSound, randomizePitch: true);

    [SerializeField] private AudioClip inventoryDragSound;
    public void PlayInventoryDragSound() => PlaySound(inventoryDragSound, randomizePitch: true, volume: 0.6f);

    [SerializeField] private AudioClip inventoryDropSound;
    public void PlayInventoryDropSound() => PlaySound(inventoryDropSound, randomizePitch: true, volume: 0.6f);

    [SerializeField] private AudioClip inventoryDropFailSound;
    public void PlayInventoryDropFailSound() => PlaySound(inventoryDropFailSound, randomizePitch: true, volume: 0.6f);
    
    [SerializeField] private AudioClip additionalStorageOpenSound;
    public void PlayAdditionalStorageOpenSound() => PlaySound(additionalStorageOpenSound, randomizePitch: true);

    [SerializeField] private AudioClip additionalStorageCloseSound;
    public void PlayAdditionalStorageCloseSound() => PlaySound(additionalStorageCloseSound, randomizePitch: true);
    
    [SerializeField] private AudioClip storageChestOpenSound;
    public void PlayStorageChestOpenSound() => PlaySound(storageChestOpenSound);

    [SerializeField] private AudioClip storageChestCloseSound;
    public void PlayStorageChestCloseSound() => PlaySound(storageChestCloseSound);
    
    [SerializeField] private AudioClip itemCollectedSound;
    public void PlayItemCollectedSound() => PlaySound(itemCollectedSound, randomizePitch: true);

    [SerializeField] private AudioClip onItemBreakSound;
    public void PlayOnItemBreakSound() => PlaySound(onItemBreakSound);

    [SerializeField] private AudioClip dropItemSound;
    public void PlayDropItemSoundSound() => PlaySound(dropItemSound, randomizePitch: true);
    
    [SerializeField] private List<AudioClip> playerTooltipSounds;
    public void PlayPlayerTooltipSound() => PlaySound(playerTooltipSounds.GetRandom(), randomizePitch: false);

    [SerializeField] private AudioClip genericTweenPositionSound;
    public void PlayGenericTweenPositionSound() => PlaySound(genericTweenPositionSound, randomizePitch: true);
    
    public void PlayHarvestableTickSound(AudioClip harvestableTickSound) => PlaySound(harvestableTickSound, progressivelyIncrementPitchBy: 0.05f, resetTimer: 1f, isResetTimerAdditive: true);
    public void PlayHarvestableCompleteSound(AudioClip harvestableCompleteSound) => PlaySound(harvestableCompleteSound, randomizePitch: true);
}