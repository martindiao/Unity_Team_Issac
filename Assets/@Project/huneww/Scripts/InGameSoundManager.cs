using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Unity.VisualScripting;

public class InGameSoundManager : MonoBehaviour
{
    public static InGameSoundManager instance;

    public List<AudioClip> audioClips;

    private AudioSource AudioManager;

    enum BGM
    {
        Cave,
        Boss
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        AudioManager = GetComponent<AudioSource>();

        AudioManager.volume = VolumeSaveLoad.BGMLoad();

        PlayCaveBGM();
    }

    public void StopBGM()
    {
        AudioManager.Stop();
    }

    public void PlayCaveBGM()
    {
        AudioManager.Stop();
        // BGM «√∑π¿Ã
        AudioManager.clip = audioClips[(int)BGM.Cave];
        AudioManager.Play();
    }

    public void PlayBossBGM()
    {
        AudioManager.Stop();
        AudioManager.clip = audioClips[(int)BGM.Boss];
        AudioManager.Play();
    }

}
