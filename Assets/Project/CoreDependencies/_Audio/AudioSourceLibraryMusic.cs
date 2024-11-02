using UnityEngine;

public class AudioSourceLibraryMusic : AudioSourceLibraryBase
{   
    public void PlayMusic(AudioClip music) => PlaySound(music, AudioManager.instance.audioSourceMusic);
    public void StopMusic() => AudioManager.instance.audioSourceMusic.Stop();
}