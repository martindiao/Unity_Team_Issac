using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCamerMove : MonoBehaviour
{
    // 카메라 움직임 속도
    public float MoveSpeed;

    // 지금 타이틀화면에 있는지 확인 변수
    private bool isTitle = true;

    // 지금 게임메뉴에 있는지 확인 변수
    private bool isGameMenu = false;
    // 게임 메뉴 포인터 위치 변수
    private int Gamepointer = 0;
    // 게임 메뉴 포인터 스프라이트
    public GameObject[] GamePointerSprite;

    // 지금 옵션에 있는지 확인 변수
    private bool isOption = false;
    // 옵션 포인터 위치 변수
    private int Optionpointer = 0;
    // 옵션 포인터 스프라이트
    public GameObject[] OptionPointerSprite;

    // 지금 캐릭터 선택창에 있는지 확이 변수
    private bool isCharacter = false;

    // 각종 메뉴 위치 저장 List
    private List<Vector3> MenuPos;
    // List 인덱스번호 변수
    enum MenuPosition
    {
        TitleMenu,
        GameMenu,
        CharacterMenu,
        OptionMenu
    }

    private void Start()
    {

        // list 초기화
        MenuPos = new List<Vector3>();

        // list에 위치 변수 추가
        MenuPos.Add(new Vector3(0, 0, -10));      // 타이틀
        MenuPos.Add(new Vector3(0, -10, -10));    // 게임 메뉴
        MenuPos.Add(new Vector3(0 - 20, -10));     // 캐릭터 선택창
        MenuPos.Add(new Vector3(15, -10, -10));   // 옵션

        isTitle = true;
        isGameMenu = false;
        isOption = false;
        isCharacter = false;
    }

    private void Update()
    {
        // 메인 메뉴에서 카메라 이동 메서드
        CamerMove();
    }

    private void CamerMove()
    {

        // 타이틀 화면일 때
        if (isTitle)
        {
            InTitle();
        }

        // 게임 메뉴일 떄
        else if (isGameMenu)
        {
            InGameMenu();
        }

        // 옵션일 때
        else if (isOption)
        {
            InOption();
        }

        // 캐릭터 선택창일 때
        else if (isCharacter)
        {
            InCharacter();
        }
    }

    private void InTitle()
    {

        // esc 클릭시 게임 종료
        if (Input.GetButtonDown("esc"))
        {
            Debug.Log("게임 종료");
            isTitle = !isTitle;
            Application.Quit();
        }

        // esc를 제외한 키를 입력하면
        else if (!Input.GetButtonDown("esc") && Input.inputString != "")
        {
            // isTitle 값 변경
            isTitle = !isTitle;
            // isGameMenu 값 변경
            isGameMenu = !isGameMenu;
            // 매 프레임마다 실행하는 코루틴 메서드
            StartCoroutine(TitleToGameMenu());
            Debug.Log("게임 메뉴로 이동");

            // 사운드 매니저의 오픈사운드 플레이
            SoundManager.OpenSound();
        }
    }

    private void InGameMenu()
    {
        // 게임 메뉴에서 ESC키를 누르면
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // isGameMenu 값 변경
            isGameMenu = !isGameMenu;
            // isTitle 값 변경
            isTitle = !isTitle;
            // 매 프레임마다 실행하는 코루틴 메서드
            StartCoroutine(GameMenuToTitle());

            Debug.Log("타이틀 화면으로 이동");

            // 사운드 매니저의 클로즈사운드 플레이
            SoundManager.CloseSound();
        }

        // 엔터키 입력
        if (Input.GetButtonDown("UseBoom"))
        {
            if (Gamepointer == 0)
            {
                StartCoroutine(GameMenuToCharacter());
                Debug.Log("캐릭터 선택창으로 이동");

                // 사운드 매니저의 오픈사운드 플레이
                SoundManager.OpenSound();

                // isGameMenu 값 변겅
                isGameMenu = !isGameMenu;
                // isCharacter 값 변경
                isCharacter = !isCharacter;
            }
            else if (Gamepointer == 1)
            {
                StartCoroutine(GameMenuToOption());
                Debug.Log("옵션으로 이동");

                // 사운드 매니저의 오픈사운드 플레이
                SoundManager.OpenSound();

                // isGameMenu 값 변경
                isGameMenu = !isGameMenu;
                // isOption 값  변경
                isOption = !isOption;
            }

            // 오픈 사운드 플레이
            SoundManager.OpenSound();
        }

        // 윗 방향 클릭
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Gamepointer++;
            if (Gamepointer > 1) Gamepointer = 0;

            // 선택 이동 사운드 플레이
            SoundManager.PoopSound();
        }
        // 아랫 방향 클릭
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Gamepointer--;
            if (Gamepointer < 0) Gamepointer = 1;

            // 선택 이동 사운드 플레이
            SoundManager.PoopSound();
        }

        // GamePointerSprite의 SetActive 변경
        GamePointerSprite[Gamepointer].SetActive(true);
        GamePointerSprite[Gamepointer == 0 ? 1 : 0].SetActive(false);
    }

    private void InOption()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // isGameMenu 값 변경
            isGameMenu = !isGameMenu;
            // isOption 값  변경
            isOption = !isOption;

            // 매 프레임마다 실행하는 코루틴 메서드
            StartCoroutine(OptionToGameMenu());

            Debug.Log("게임 메뉴로 이동");

            // 사운드 매니저의 클로즈사운드 플레이
            SoundManager.CloseSound();
        }

        // 윗 방향 클릭
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Optionpointer++;
            if (Optionpointer > 1) Optionpointer = 0;

            SoundManager.PoopSound();
        }
        // 아랫 방향 클릭
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Optionpointer--;
            if (Optionpointer < 0) Optionpointer = 1;

            SoundManager.PoopSound();
        }

        // OptionPointerSprite의 SetActive 변경
        OptionPointerSprite[Optionpointer].SetActive(true);
        OptionPointerSprite[Optionpointer == 0 ? 1 : 0].SetActive(false);

        // 오른쪽 방향 클릭
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // BGM 일때
            if (Optionpointer == 0)
            {
                // BGM 볼륨 증가 메서드 호출
                SoundManager.BGMUp();

                // bar 스프라이트 변경
                SoundManager.BgmBar();
            }
            // SFX 일때
            else if (Optionpointer == 1)
            {
                // SFX 볼륨 증가 메서드 호출
                SoundManager.SFXUp();

                // bar 스프라이트 변경
                SoundManager.SfxBar();
            }

            // 조절시 나오는 사운드 재생
            SoundManager.PoopSound();
        }
        // 왼쪽 방향 클릭
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // BGM 일때
            if (Optionpointer == 0)
            {
                // BGM 볼륨 감소 메서드 호출
                SoundManager.BGMDown();

                // bar 스프라이트 변경
                SoundManager.BgmBar();
            }
            // SFX 일때
            else if (Optionpointer == 1)
            {
                // SFX 볼륨 감소 메서드 호출
                SoundManager.SFXDown();

                // bar 스프라이트 변경
                SoundManager.SfxBar();
            }
            // 조절시 나오는 사운드 재생
            SoundManager.PoopSound();
        }
    }

    private void InCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // isGameMenu 값 변경
            isGameMenu = !isGameMenu;
            // isOption 값  변경
            isCharacter = !isCharacter;

            // 매 프레임마다 실행하는 코루틴 메서드
            StartCoroutine(CharacterToGameMenu());

            Debug.Log("게임 메뉴로 이동");

            // 사운드 매니저의 클로즈사운드 플레이
            SoundManager.CloseSound();
        }

        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("게임 시작");

            // BGM 사운드 정지
            SoundManager.BgmStop();

            // 로딩 UI 실행
            LoadinSceneController.Instance.LoadScene("Basement");
        }
    }

    /// <summary>
    /// 타이틀 화면에서 게임 메뉴 화면으로 카메라 이동
    /// </summary>
    /// <returns></returns>
    private IEnumerator TitleToGameMenu()
    {   
        // 현재 카메라 위치가 GameMenuPos보다 크면
        while (transform.position.y > MenuPos[(int)MenuPosition.GameMenu].y)
        {
            // 카메라 현재 위치
            var curpos = transform.position;
            // 카메라 이동 위치
            var nextpos = Vector3.down * MoveSpeed * Time.deltaTime;

            // 카메라 위치 변경
            transform.position = curpos + nextpos;

            // 매 프레임마다 코루틴 실행
            yield return null;
        }

        if (transform.position.y < MenuPos[(int)MenuPosition.GameMenu].y)
            transform.position = MenuPos[(int)MenuPosition.GameMenu];

        // while문이 끝나면 코루틴 해제
        yield break;
    }

    /// <summary>
    /// 게임 메뉴에서 타이틀 화면으로 카메라 이동
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameMenuToTitle()
    {
        while (transform.position.y < MenuPos[(int)MenuPosition.TitleMenu].y)
        {
            // 카메라 현재 위치
            var curpos = transform.position;
            // 카메라 이동 위치
            var nextpos = Vector3.up * MoveSpeed * Time.deltaTime;

            // 카메라 위치 변경
            transform.position = curpos + nextpos;

            // 매 프레임마다 코루틴 실행
            yield return null;
        }

        if (transform.position.y > MenuPos[(int)MenuPosition.TitleMenu].y)
            transform.position = MenuPos[(int)MenuPosition.TitleMenu];

        // while문이 끝나면 코루틴 해제
        yield break;
    }

    /// <summary>
    /// 게임 메뉴에서 옵션으로 카메라 이동
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameMenuToOption()
    {
        while (transform.position.x < MenuPos[(int)MenuPosition.OptionMenu].x)
        {
            // 카메라 현재 위치
            var curpos = transform.position;
            // 카메라 이동 위치
            var nextpos = Vector3.right * MoveSpeed * Time.deltaTime;

            // 카메라 위치 변경
            transform.position = curpos + nextpos;

            // 매 프레임마다 코루틴 실행
            yield return null;
        }

        if (transform.position.x > MenuPos[(int)MenuPosition.OptionMenu].x)
            transform.position = MenuPos[(int)MenuPosition.OptionMenu];

        // while문이 끝나면 코루틴 해제
        yield break;
    }

    /// <summary>
    /// 옵션에서 게임 메뉴로 카메라 이동
    /// </summary>
    /// <returns></returns>
    private IEnumerator OptionToGameMenu()
    {
        while (transform.position.x > MenuPos[(int)MenuPosition.GameMenu].x)
        {
            // 카메라 현재 위치
            var curpos = transform.position;
            // 카메라 이동 위치
            var nextpos = Vector3.left * MoveSpeed * Time.deltaTime;

            // 카메라 위치 변경
            transform.position = curpos + nextpos;

            // 매 프레임마다 코루틴 실행
            yield return null;
        }

        if (transform.position.x < MenuPos[(int)MenuPosition.GameMenu].x)
            transform.position = MenuPos[(int)MenuPosition.GameMenu];

        // while문이 끝나면 코루틴 해제
        yield break;
    }

    /// <summary>
    /// 게임 메뉴에서 캐릭터 선택창으로 카메라 이동
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameMenuToCharacter()
    {
        while (transform.position.y > -20.0f)
        {
            // 카메라 현재 위치
            var curpos = transform.position;
            // 카메라 이동 위치
            var nextpos = Vector3.down * MoveSpeed * Time.deltaTime;

            // 카메라 위치 변경
            transform.position = curpos + nextpos;

            // 매 프레임마다 코루틴 실행
            yield return null;
        }

        if (transform.position.y < -20.0f)
            transform.position = new Vector3(0, -20, -10);

        // while문이 끝나면 코루틴 해제
        yield break;
    }

    /// <summary>
    /// 캐릭터 선택창에서 게임 메뉴로 이동
    /// </summary>
    /// <returns></returns>
    private IEnumerator CharacterToGameMenu()
    {
        while (transform.position.y < MenuPos[(int)MenuPosition.GameMenu].y)
        {
            // 카메라 현재 위치
            var curpos = transform.position;
            // 카메라 이동 위치
            var nextpos = Vector3.up * MoveSpeed * Time.deltaTime;

            // 카메라 위치 변경
            transform.position = curpos + nextpos;

            // 매 프레임마다 코루틴 실행
            yield return null;
        }

        if (transform.position.y > MenuPos[(int)MenuPosition.GameMenu].y)
            transform.position = MenuPos[(int)MenuPosition.GameMenu];

        // while문이 끝나면 코루틴 해제
        yield break;
    }

}