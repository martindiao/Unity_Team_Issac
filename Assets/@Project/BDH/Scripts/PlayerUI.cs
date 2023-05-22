using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	// 하트 UI 프리팹
	public GameObject[] hearts;
	public GameObject[] halfHearts;
	public GameObject[] blankHearts;
	public GameObject[] soulHearts;
	public GameObject[] halfSoules;

	// 액티브 아이템 프리팹
	public GameObject[] activeItme;

	// 알약 텍스트
	public GameObject[] PillsText;
	// 아이템 텍스트
	public GameObject[] ItemsText;

	// 플레이어 본체
	public GameObject playerObj;
	Player player;

	// 액티브 아이템 이미지
	Image i_image;

	// 액티브 아이템 이미지 상태
	bool voltUp;

	private void Start()
	{
		// 각 컴포넌트 찾기
		playerObj = GameObject.FindGameObjectWithTag("Player");
		player = playerObj.GetComponent<Player>();

		// 액티브 아이템 이미지 상태 초기화
		voltUp = false;

		// 액티브 아이템 정보 가져오기
		i_image = activeItme[0].GetComponent<Image>();
	}

	private void Update()
	{
		heartUI();
		nineVolt();
		voltCool();
	}

	// 체력 검사 후 Ui 적용 함수
	void heartUI()
	{
		// 이미지가 겹치지 않게 모든 하트 비활성화
		for (int i = 0; i < 10; i++)
		{
			hearts[i].SetActive(false);
			halfHearts[i].SetActive(false);
			blankHearts[i].SetActive(false);
		}
		for (int i = 0; i < 9; i++)
		{
			soulHearts[i].SetActive(false);
			halfSoules[i].SetActive(false);
		}

		// 현재 하트의 현황을 파악하기 위한 int 변수들
		int heart = player.curRealHeart / 2;
		int halfHeart = player.curRealHeart % 2;
		int blankHeart = (player.maxRealHeart - player.curRealHeart) / 2;
		int soulHeart = player.curSoulHeart / 2;
		int HalfSoul = player.curSoulHeart % 2;

		// 총 체력 갯수
		int count;

		// 반쪽 짜리 하트를 반쪽으로 인식하기 위한 예외 처리
		if ((player.curHeart % 2 != 1) && (halfHeart + HalfSoul == 0))
		{
			count = player.curHeart / 2;
		}
		else if ((player.curHeart % 2 != 1) && (halfHeart + HalfSoul != 0))
		{
			count = player.curHeart / 2 + 1;
		}
		else
		{
			count = player.curHeart / 2 + 1;
		}

		
		for (int i = 0; i < count; i++)
		{
			// 온전한 빨간 하트 출력
			if (heart > 0)
			{
				hearts[i].SetActive(true);
				heart--;
			}
			// 반쪽짜리 빨간 하트 출력
			else if (halfHeart == 1)
			{
				halfHearts[i].SetActive(true);
				halfHeart--;
			}
			// 빈 하트 출력
			else if (blankHeart > 0)
			{
				blankHearts[i].SetActive(true);
				blankHeart--;
			}
			// 온전한 소울 하트 출력
			else if (soulHeart > 0)
			{
				soulHearts[(i - 1)].SetActive(true);
				soulHeart--;
			}
			// 반쪽짜리 소울 하트 출력
			else if (HalfSoul == 1)
			{
				halfSoules[(i - 1)].SetActive(true);
				HalfSoul--;
			}
		}
	}

	// 액티브 아이템 획득 시 액티브 아이템 ui에 표시
	void nineVolt()
	{
		// 액티브 아이템 소지 중이며 해당 아이템 이미지가 없을 시
		if ((player.activeItem == Alter.Collection.NineVolt) && !voltUp)
		{
			// 액티브 아이템 이미지 활성화
			activeItme[0].SetActive(true);
		}
	}

	// 액티브 아이템 쿨 타임 표시
	void voltCool()
	{
		// 액티브 아이템이 쿨타임일 때
		if (player.i_coolDown > 0)
		{
			// 회색 처리
			i_image.color = Color.gray;
		}
		// 액티브 아이템 사용 가능일 시 
		else if (player.i_coolDown <= 0)
		{
			// 색 정상화
			i_image.color = Color.white;
		}
	}

	// 현재 획득한 알약 이름, 효과 출력
	public void onPillsText(int i)
	{
		PillsText[i].SetActive(true);
	}
	// 현재 획득한 알약 이름, 효과 Active false
	public void offPillsText(int i)
	{
		PillsText[i].SetActive(false);
	}
	// 현재 획득한 아이템 이름, 효과 출력
	public void onItemsText(int i)
	{
		ItemsText[i].SetActive(true);
	}
	// 현재 획득한 아이템 이름, 효과 Active false
	public void offItemsText(int i)
	{
		ItemsText[i].SetActive(false);
	}
}
