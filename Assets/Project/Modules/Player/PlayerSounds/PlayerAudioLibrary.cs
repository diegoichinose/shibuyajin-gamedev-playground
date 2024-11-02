using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioLibrary : MonoBehaviour
{   
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private Transform audioSourcesPoolParent;
    [SerializeField] private List<AudioSource> audioSourcesPool;
    [SerializeField] private float pitchRandomizerMin = 0.5f;
    [SerializeField] private float pitchRandomizerMax = 1.5f;

    [SerializeField] private AudioClip dashSound;
    public void PlayDashSound() => PlaySound(dashSound, randomizePitch: true);
    
    public void PlaySound(AudioClip audioClip, bool randomizePitch = false)
    {
        var audioSourceToUse = audioSourcesPool.Find(x => x.isPlaying == false);
        if (audioSourceToUse == null)
        {
            audioSourceToUse = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity, parent: audioSourcesPoolParent).GetComponent<AudioSource>();
            audioSourcesPool.Add(audioSourceToUse);
        }

        if (randomizePitch)
            audioSourceToUse.pitch = Random.Range(pitchRandomizerMin, pitchRandomizerMax);

        audioSourceToUse.clip = audioClip;
        audioSourceToUse.Play();
    }


}