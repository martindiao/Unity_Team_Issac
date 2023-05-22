using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VolumeData
{
    public float BGM;
    public float SFX;
}

public class VolumeSaveLoad : MonoBehaviour
{
    private static VolumeData VD;

    private void Awake()
    {
        VD = new VolumeData();
    }

    /// <summary>
    /// BGM볼륨을 json파일로 저장
    /// </summary>
    /// <param name="bgmvolume">저장할 BGM 볼륨</param>
    public static void BGMSave(float bgmvolume)
    {
        // 인수값 저장
        VD.BGM = bgmvolume;

        // Json파일에 텍스트로 저장
        File.WriteAllText(Application.dataPath + "/Resources/JsonData/VolumeData.json", JsonUtility.ToJson(VD));
    }

    /// <summary>
    /// BGM볼륨을 Json파일에서 읽어와 반환
    /// </summary>
    /// <returns>현재 BGM 볼륨</returns>
    public static float BGMLoad()
    {
        // Json 파일 읽어오기
        string str = File.ReadAllText(Application.dataPath + "/Resources/JsonData/VolumeData.json");

        // VD에 읽어온 Json파일 텍스트 저장
        VD = JsonUtility.FromJson<VolumeData>(str);

        // 현재 BGM 볼륨 반환
        return VD.BGM;
    }

    /// <summary>
    /// SFX볼륨을 json파일로 저장
    /// </summary>
    /// <param name="sfxvolume">저장할 SFX볼륨</param>
    public static void SFXSave(float sfxvolume)
    {
        // 인수값 저장
        VD.SFX = sfxvolume;

        // Json파일에 텍스트로 저장
        File.WriteAllText(Application.dataPath + "/Resources/JsonData/VolumeData.json", JsonUtility.ToJson(VD));
    }

    /// <summary>
    /// SFX볼륨을 Json 파일에서 읽어와 반환
    /// </summary>
    /// <returns>현재 SFX 볼륨</returns>
    public static float SFXLoad()
    {
        // Json 파일 읽어오기
        string str = File.ReadAllText(Application.dataPath + "/Resources/JsonData/VolumeData.json");

        // VD에 읽어온 Json파일 값 저장
        VD = JsonUtility.FromJson<VolumeData>(str);

        // 현재 SFX 볼륨 반환
        return VD.SFX;
    }
}
