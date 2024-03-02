using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource fixedSource;
    [SerializeField] private bool useFixedSourceConfigs;

    private List<AudioSource> playingAudioSources = new List<AudioSource>();

    //Plays a sound in a specific audio source where only one sound can be played at a time
    public void PlaySingle(AudioClipSO musicClip)
    {
        fixedSource.clip = musicClip._AudioClip;
        if(!useFixedSourceConfigs)
        {
            fixedSource.volume = musicClip.clipVolume;
            fixedSource.spatialBlend = musicClip.spacialBlend;
            fixedSource.loop = musicClip.loopAudio;
        }
        fixedSource.Play();
    }

    public void StopSingle()
    {
        fixedSource.Stop();
    }

    //Plays an sfx in a new audio source in the gameobject this is attached to
    public void PlaySFX(AudioClipSO SFX)
    {
        AudioSource newAudioScource = gameObject.AddComponent<AudioSource>();
        newAudioScource.clip = SFX._AudioClip;
        newAudioScource.volume = SFX.clipVolume;
        newAudioScource.spatialBlend = SFX.spacialBlend;
        newAudioScource.loop = SFX.loopAudio;
        
        newAudioScource.Play();
        playingAudioSources.Add(newAudioScource);
    }

    public void PlayOneShot(AudioClipSO SFX)
    {
        fixedSource.PlayOneShot(SFX._AudioClip);
    }

    public void PlaySFXGlobal(AudioClipSO SFX)
    {
        GlobalAudioPlayer.PlaySFX(SFX);
    }

    public void StopAllSFX()
    {
        for (int i = 0; i < playingAudioSources.Count; i++)
        {
            AudioSource thisSource = playingAudioSources[i];
            playingAudioSources.Remove(thisSource);
            Destroy(thisSource);
            i--;
        }
    }

    private void Update()
    {
        for (int i = 0; i < playingAudioSources.Count; i++)
        {
            if (!playingAudioSources[i].isPlaying)
            {
                AudioSource thisSource = playingAudioSources[i];
                playingAudioSources.Remove(thisSource);
                Destroy(thisSource);
                i--;
            }
        }
    }
}
