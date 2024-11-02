using System.Collections.Generic;
using UnityEngine;

public class AudioSourceLibraryBase : MonoBehaviour
{
    // COSMETIC SHORTCUT
    public AudioSource PlaySound(AudioClip audioClip, AudioSource audioSource = null, bool randomizePitch = false, float volume = 1f, float panning = 0f, float pitchIncrement = 0f, bool dontPlayIfAlreadyPlaying = false)
        => AudioManager.instance.PlaySound(audioClip, audioSource, randomizePitch, volume, panning, pitchIncrement, dontPlayIfAlreadyPlaying);
   
    // POSITION
    public AudioSource PlaySound(AudioClip audioClip, Vector3 position, AudioSource audioSource = null, bool randomizePitch = false, float volume = 1f, float panning = 0f, float pitchIncrement = 0f, bool dontPlayIfAlreadyPlaying = false)
        => AudioManager.instance.PlaySound(audioClip, position, setAsLocalPosition: false, audioSource, randomizePitch, volume, panning, pitchIncrement, dontPlayIfAlreadyPlaying, useSpatialSound: true);
   
    // PROGRESSIVE PITCH INCREASE
    public AudioSource PlaySound(AudioClip audioClip, float progressivelyIncrementPitchBy, float resetTimer, bool isResetTimerAdditive)
        => AudioManager.instance.PlaySound(audioClip, progressivelyIncrementPitchBy, resetTimer, isResetTimerAdditive);

    // STOP TIMER
    public void PlaySound(AudioClip audioClip, AudioSource audioSource, float stopTimer, bool randomizePitch = false)
        => AudioManager.instance.PlaySound(audioClip, audioSource, stopTimer, randomizePitch);
}