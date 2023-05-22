using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class InGameSFXManager : MonoBehaviour
{
    // 접근을 위한 정적 변수
    public static InGameSFXManager instance;

    // 오디오 클립 저장 List
    public List<AudioClip> audioClips;

    // 오디오 클립 인덱스
    public enum SFX
    {
        BossIntro,
        BossOutro,
        MonsterTearFire_1,
        MonsterTearFire_2,
        MonsterTearFire_3,
        MonsterTearFire_4,
        PlayerHit_1,
        PlayerHit_2,
        PlayerHit_3,
        KeyDrop,
        KeyGet,
        MenuOpen,
        MenuClose,
        CoinDrop,
        CoinGet,
        Plop,
        ItemGet,
        RockDestroy_1,
        RockDestroy_2,
        RockDestroy_3,
        CampFireOff,
        PlayerTearFire,
        TearDestroy,
        Unlock,
        MonsterDead_1,
        MonsterDead_2,
        MonsterDead_3,
        MonsterDead_4,
        MonsterDead_5,
        NestVoice,
        Boom,
        PillsDown,
        PillsUp,
        Castleport,
        Monstro_Atk_1,
        Monstro_Atk_2,
        Monstro_Atk_3,
        DoorOpen,
        DoorClose,
        FlySwam,
        GetBoom,
        BoosStomp,
        Chubber_Atk,
        Charger_Atk,
        NestRun,
        GloBinReSpawn,
        LowJump
    }

    // 생성자 접근 제한
    private InGameSFXManager() { }

    // 오디오 소스 컴퍼넌트
    private AudioSource AudioManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // 오디오 소스 컴퍼넌트 추가
        AudioManager = GetComponent<AudioSource>();

        // 읽어온 데이터 값 볼륨에 적용
        AudioManager.volume = VolumeSaveLoad.SFXLoad();
    }

    /// <summary>
    /// 보스 인트로 사운드
    /// </summary>
    public void BossIntro()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.BossIntro]);
    }

    /// <summary>
    /// 보스 아웃트로 사운드
    /// </summary>
    public void BossOuTro()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.BossOutro]);
    }

    /// <summary>
    /// 몬스터 눈물 발사 소리
    /// </summary>
    /// <param name="index">0 ~ 3의 값으로 랜덤 사운드 재생</param>
    public void MonsterTearFire(int index)
    {
        switch (index)
        {
            case 0:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterTearFire_1]);
                break;
            case 1:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterTearFire_2]);
                break;
            case 2:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterTearFire_3]);
                break;
            case 3:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterTearFire_4]);
                break;
            default:
                Console.WriteLine("인덱스 값이 이상합니다. MonsterTearFrie는 0 ~ 3 사이의 값을 전달해야합니다.");
                break;
        }
    }

    /// <summary>
    /// 플레이어 히트 사운드
    /// </summary>
    /// <param name="index">0 ~ 2의 값으로 랜덤 사운드 재생</param>
    public void PlayerHit(int index)
    {
        switch (index)
        {
            case 0:
                AudioManager.PlayOneShot(audioClips[(int)SFX.PlayerHit_1]);
                break;
            case 1:
                AudioManager.PlayOneShot(audioClips[(int)SFX.PlayerHit_1]);
                break;
            case 2:
                AudioManager.PlayOneShot(audioClips[(int)SFX.PlayerHit_1]);
                break;
            default:
                Debug.Log("인덱스 값이 이상합니다. PlayerHit는 0 ~ 2 사이의 값을 전달해야합니다.");
                break;
        }
    }

    /// <summary>
    /// 열쇠 획득 사운드
    /// </summary>
    public void KeyGet()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.KeyGet]);
    }

    /// <summary>
    /// 열쇠 드랍 사운드
    /// </summary>
    public void KeyDrop()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.KeyDrop]);
    }

    /// <summary>
    /// 일시 정지 메뉴 오픈 사운드
    /// </summary>
    public void PauseMenuOpen()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.MenuOpen]);
    }

    /// <summary>
    /// 일시 정지 메뉴 닫는 사운드
    /// </summary>
    public void PauseMenuClose()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.MenuClose]);
    }

    /// <summary>
    /// 코인 획득 사운드
    /// </summary>
    public void CoinGet()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.CoinGet]);
    }

    /// <summary>
    /// 코인 드랍 사운드
    /// </summary>
    public void CoinDrop()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.CoinDrop]);
    }

    /// <summary>
    /// 똥 오브젝트 파괴 사운드
    /// </summary>
    public void Poop()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.Plop]);
    }

    /// <summary>
    /// 아이템 획득 사운드
    /// </summary>
    public void ItemGet()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.ItemGet]);
    }

    /// <summary>
    /// 돌 오브젝트 파괴 사운드
    /// </summary>
    /// <param name="index"> 0 ~ 2의 값으로 랜덤 사운드 재생</param>
    public void RockDestroy(int index)
    {
        switch (index)
        {
            case 0:
                AudioManager.PlayOneShot(audioClips[(int)SFX.RockDestroy_1]);
                break;
            case 1:
                AudioManager.PlayOneShot(audioClips[(int)SFX.RockDestroy_1]);
                break;
            case 2:
                AudioManager.PlayOneShot(audioClips[(int)SFX.RockDestroy_1]);
                break;
            default:
                Debug.Log("인덱스 값이 이상합니다. RockDestroy는 0 ~ 2 사이의 값을 전달해야합니다.");
                break;
        }
    }

    /// <summary>
    /// 캠프파이어 불 꺼지는 사운드
    /// </summary>
    public void CampFireOff()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.CampFireOff]);
    }

    /// <summary>
    /// 플레이어 공격 사운드
    /// </summary>
    public void PlayerTearFire()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.PlayerTearFire]);
    }

    /// <summary>
    /// 눈물 파괴 사운드
    /// </summary>
    public void TearDestroy()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.TearDestroy]);
    }

    /// <summary>
    /// 열쇠 사용 사운드
    /// </summary>
    public void Unlock()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.Unlock]);
    }

    /// <summary>
    /// 몬스터 죽는 사운드
    /// </summary>
    /// <param name="index">0 ~ 4의 값으로 랜덤 사운드 재생</param>
    public void MonsterDead(int index)
    {
        switch (index)
        {
            case 0:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterDead_1]);
                break;
            case 1:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterDead_2]);
                break;
            case 2:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterDead_3]);
                break;
            case 3:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterDead_4]);
                break;
            case 4:
                AudioManager.PlayOneShot(audioClips[(int)SFX.MonsterDead_5]);
                break;
            default:
                Debug.Log("인덱스 값이 이상합니다. MonsterDead는 0 ~ 4 사이의 값을 전달해야합니다.");
                break;
        }
    }

    /// <summary>
    /// Nest 보이스
    /// </summary>
    public void NextVoice()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.NestVoice]);
    }

    /// <summary>
    /// 폭탄 터지는 사운드
    /// </summary>
    public void Boom()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.Boom]);
    }

    /// <summary>
    /// 알약이 능력치 다운일 경우
    /// </summary>
    public void PillsDown()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.PillsDown]);
    }

    /// <summary>
    /// 알약이 능력치 업일 경우
    /// </summary>
    public void PillsUp()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.PillsUp]);
    }

    /// <summary>
    /// 보스 방 입장시 문닫히는 소리
    /// </summary>
    public void Castleport()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.Castleport]);
    }

    /// <summary>
    /// 보스 몬스트로 공격 사운드
    /// </summary>
    /// <param name="index">0 ~ 3의 값으로 랜덤 사운드 재생</param>
    public void MonstroAtack(int index)
    {
        switch (index)
        {
            case 0:
                AudioManager.PlayOneShot(audioClips[(int)SFX.Monstro_Atk_1]);
                break;
            case 1:
                AudioManager.PlayOneShot(audioClips[(int)SFX.Monstro_Atk_2]);
                break;
            case 2:
                AudioManager.PlayOneShot(audioClips[(int)SFX.Monstro_Atk_3]);
                break;
            default:
                Debug.Log("인덱스 값이 이상합니다. MonstroAttack은 0 ~ 3사이의 값을 전달해야합니다.");
                break;
        }
    }

    /// <summary>
    /// 문 열리는 사운드
    /// </summary>
    public void DoorOpen()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.DoorOpen]);
    }

    /// <summary>
    /// 문 닫히는 사운드
    /// </summary>
    public void DoorClose()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.DoorClose]);
    }

    /// <summary>
    /// 파리 사운드
    /// </summary>
    public void FlySwam()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.FlySwam]);
    }

    /// <summary>
    /// 폭탄 획득 사운드
    /// </summary>
    public void GetBoom()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.GetBoom]);
    }

    /// <summary>
    /// 보스 내력찍는 사운드
    /// </summary>
    public void BoosStomp()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.BoosStomp]);
    }

    /// <summary>
    /// Chubber 공격 사운드
    /// </summary>``
    public void ChubberAttack()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.Chubber_Atk]);
    }

    /// <summary>
    /// Charger 공격 사운드
    /// </summary>
    public void ChargerAttack()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.Charger_Atk]);
    }

    /// <summary>
    /// Nest에 접근시 나오는 사운드
    /// </summary>
    public void NestRun()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.NestRun]);
    }

    /// <summary>
    /// 글로빈 소환 사운드
    /// </summary>
    public void GloBinSpawn()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.GloBinReSpawn]);
    }

    /// <summary>
    /// 로우 점프 사운드
    /// </summary>
    public void LowJump()
    {
        AudioManager.PlayOneShot(audioClips[(int)SFX.LowJump]);
    }

}
