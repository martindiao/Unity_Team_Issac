using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;
using System.IO;

public class LoadinSceneController : MonoBehaviour
{
    private AudioSource ad;

    // 정적 변수 instance;
    private static LoadinSceneController instance;
    // instance 갯수 확인후 인스턴스 생성
    public static LoadinSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadinSceneController>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }

            return instance;
        }
    }

    // 인스펙터창에 LoadingUI가 없다면 인스턴스 생성
    private static LoadinSceneController Create()
    {
        return Instantiate(Resources.Load<LoadinSceneController>("LoadingUI"));
    }

    private void Awake()
    {
        // Instance가 다르면 그 오브젝트를 파괴
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // 현재 있는 Instance를 로딩시 파괴되지 안도록설정
        DontDestroyOnLoad(gameObject);
    }

    // 로딩씬에서 사용한 UI그룹
    [SerializeField]
    private CanvasGroup canvasGroup;

    // 로딩씬 이름 저장 변수
    private string loadSceneName;
        
    /// <summary>
    /// 로딩씬 시작 메서드
    /// </summary>
    /// <param name="sceneName">불러올 씬의 이름</param>
    public void LoadScene(string sceneName)
    {
        // Activ값 true로 변경
        gameObject.SetActive(true);

        // 씬 로딩이 끝나면 OnSceneLoaded를 콜빽함수로 지정
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 불러올 씬 이름을 저장
        loadSceneName = sceneName;

        // 오디오 컴퍼넌트 받아오기
        ad = GetComponent<AudioSource>();

        // 볼륨 설정
        ad.volume = VolumeSaveLoad.BGMLoad();

        // 로딩씬 사운드 실행
        if (loadSceneName != "MainMenu")
            ad.Play();

        // 페이드 인 로딩씬 실행
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        // 페이드 인 실행후 코드실행
        yield return StartCoroutine(Fade(true));

        // 로딩씬을 비동기식으로 장면 로드
        var op = SceneManager.LoadSceneAsync(loadSceneName);
        
        // 로딩될 씬이 90%에서 정지
        op.allowSceneActivation = false;

        // 로딩씬이 전부됐다면 나머지 10% 로드
        if (op.progress < 0.9f)
        {
            op.allowSceneActivation = true;
        }
    }

    // 페이드 인,아웃 메서드
    private IEnumerator Fade(bool isFadeIn)
    {
        // 페이드 시간 변수
        float time = 0f;

        while (time <= 2f)
        {
            yield return null;
            // time변수에 경과 시간 더하기
            time += Time.unscaledDeltaTime;

            // FadeIn이 true면 페이드 인
            // FadeIn이 false면 페이드 아웃
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, time) : Mathf.Lerp(1f, 0f, time);
        }

        // FadeIn이 false라면 UI Active false로 변경
        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }

    // 로딩이 전부 완료되면 실행될 콜백 함수
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // 불러온 씬과 불러올려고한 씬의 이름이 같다면
        if (arg0.name == loadSceneName)
        {
            // 페이드 아웃 코루틴 실행
            StartCoroutine(Fade(false));

            // 콜백 함수 제거
            SceneManager.sceneLoaded -= OnSceneLoaded;

            // 일정시간후 UI 제거
            Destroy(gameObject, 1f);
        }
    }
}
