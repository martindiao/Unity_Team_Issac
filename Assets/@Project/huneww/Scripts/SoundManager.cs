using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SoundManager : MonoBehaviour
{
    // SFX 사운드 매니저
    public AudioSource SFXPlayer;
    // BGM 사운드 매니저
    public AudioSource BGMPlayer;

    // SFX 사운드 클립
    public AudioClip Open;
    public AudioClip Close;
    // 옵션에서 볼륨 조절시 나는 사운드
    public AudioClip Poop;

    // 다른 스크립트에서 메서드 접근을 위한 변수
    public static Action OpenSound;
    public static Action CloseSound;
    public static Action PoopSound;
    public static Action BGMUp;
    public static Action BGMDown;
    public static Action SFXUp;
    public static Action SFXDown;
    public static Action BgmBar;
    public static Action SfxBar;
    public static Action BgmStop;

    // 현재 BGM 크기
    public static int bgmvolume;

    // 현재 SFX 크기
    public static int sfxvolume;

    // BGMbar 스프라이트
    public GameObject[] BGMBar;

    // SFXbar 스프라이트
    public GameObject[] SFXBar;

    private void Awake()
    {
        // 다른 스크립트에서 메서드 접근을 위한 람다식
        OpenSound = () => { PlayOpen(); };
        CloseSound = () => { PlayClose(); };
        PoopSound = () => { PlayPoop(); };
        BGMUp = () => { BGMVolumeUp(); };
        BGMDown = () => { BGMVolumeDown(); };
        BgmBar = () => { BGMBAR(); };

        SFXUp = () => {  SFXVolumeUp(); };
        SFXDown = () => { SFXVolumeDown(); };
        SfxBar = () => { SFXBAR(); };

        BgmStop = () => { BGMStop(); };

        // BGM 사운드 매니저 볼륨 조절
        BGMPlayer.volume = VolumeSaveLoad.BGMLoad();
        // bgm 볼륨 저장
        bgmvolume = (int)(BGMPlayer.volume * 100);
        // 스프라이트 Active 조정
        BGMBAR();

        // SFX 사운드 매니저 볼륨 조절
        SFXPlayer.volume = VolumeSaveLoad.SFXLoad();
        // sfx 볼륨 저장
        sfxvolume = (int)(SFXPlayer.volume * 10);

        // 스프라이트 Active 조정
        SFXBAR();
    }

    // 어떤 메뉴로 들어갈때 나는 소리
    private void PlayOpen()
    {
        SFXPlayer.PlayOneShot(Open);
    }

    // 어떤 메뉴에서 나올때 나는 소리
    private void PlayClose()
    {
        SFXPlayer.PlayOneShot(Close);
    }

    // 볼륨 조절시 나오는 소리
    private void PlayPoop()
    {
        SFXPlayer.PlayOneShot(Poop);
    }

    // BGM 볼륨 증가
    private void BGMVolumeUp()
    {
        bgmvolume++;

        if (bgmvolume > 10)
            bgmvolume = 10;

        BGMPlayer.volume += 0.01f;

        if (BGMPlayer.volume > 0.1f)
            BGMPlayer.volume = 0.1f;

        BGMBAR();

        // 볼륨 데이터 저장
        VolumeSaveLoad.BGMSave(BGMPlayer.volume);
    }

    // BGM 볼륨 감소
    private void BGMVolumeDown()
    {
        bgmvolume--;

        if (bgmvolume < 0)
            bgmvolume = 0;

        BGMPlayer.volume -= 0.01f;

        if (BGMPlayer.volume < 0)
            BGMPlayer.volume = 0;

        BGMBAR();

        // 볼륨 데이터 저장
        VolumeSaveLoad.BGMSave(BGMPlayer.volume);
    }

    // BAR 스프라이트 변경
    private void BGMBAR()
    {
        foreach (var bar in BGMBar)
        {
            bar.SetActive(false);
        }

        BGMBar[bgmvolume].SetActive(true);

        Debug.Log(BGMPlayer.volume);
    }

    // SFX 볼륨 증가
    private void SFXVolumeUp()
    {
        sfxvolume++;

        if (sfxvolume > 10)
            sfxvolume = 10;

        SFXPlayer.volume += 0.1f;

        if (SFXPlayer.volume > 1f)
            SFXPlayer.volume = 1f;

        SFXBAR();

        // 볼륨 데이터 저장
        VolumeSaveLoad.SFXSave(SFXPlayer.volume);
    }

    // SFX 볼륨 감소
    private void SFXVolumeDown()
    {
        sfxvolume--;

        if (sfxvolume < 0)
            sfxvolume = 0;

        SFXPlayer.volume -= 0.1f;

        if (SFXPlayer.volume < 0)
            SFXPlayer.volume = 0f;

        SFXBAR();

        // 볼륨 데이터 저장
        VolumeSaveLoad.SFXSave(SFXPlayer.volume);
    }

    // BAR 스프라이트 변경
    private void SFXBAR()
    {
        foreach (var bar in SFXBar)
        {
            bar.SetActive(false);
        }
        SFXBar[sfxvolume].SetActive(true);

        Debug.Log(SFXPlayer.volume);
    }

    // BGM 정지
    private void BGMStop()
    {
        BGMPlayer.Stop();
    }

}
