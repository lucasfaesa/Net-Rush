using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioPlayer))]
public class GlobalAudioPlayer : MonoBehaviour
{
    private static AudioPlayer audioPlayer;

    private void Awake() 
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }

    public static void PlaySFX(AudioClipSO SFX)
    {
        audioPlayer.PlaySFX(SFX);
    }
    
    public static void StopAllSFX()
    {
        audioPlayer.StopAllSFX();
    }
}