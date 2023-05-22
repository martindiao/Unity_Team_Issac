using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 아이템 장비 스크립트
public class GetBloodTear : MonoBehaviour
{
	// 혈액 눈물 스프라이트
	public Sprite[] Bloods;

	// renderer
	SpriteRenderer b_renderer;

	// 머리와 동기화
	public GameObject p_Head;
	PlayerHead head;

	// 플레이어 본체
	public GameObject playerObj;
	Player player;

	// 공격 딜레이
	float fireDelay;
	float maxDelay;
	float headDelay;

	// Start is called before the first frame update
	void Start()
	{
		// 각 컴포넌트 가져오기
		b_renderer = GetComponent<SpriteRenderer>();
		head = transform.parent.GetComponent<PlayerHead>();
		player = playerObj.GetComponent<Player>();
	}

	void Update()
	{
		idleBlood();
		shotBlood();
	}

	// 피눈물 스킨 적용
	void idleBlood()
	{
		if (!Input.GetButton("FireHorizontal") && !Input.GetButton("FireVertical"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				if (head.vAxis != 0)
				{
					// 대각 이동이 아닐 때
					if (head.hAxis == 0)
					{
						if (head.vAxis == -1)
						{
							// 정면 머리
							b_renderer.sprite = Bloods[0];
						}
						else
						{
							// 후면 머리
							b_renderer.sprite = Bloods[2];
						}
					}
					// 대각 이동일 때
					else if (head.hAxis == 1)
					{
						// 오른쪽 머리
						b_renderer.sprite = Bloods[4];
					}
					else if (head.hAxis == -1)
					{
						// 왼쪽 머리
						b_renderer.sprite = Bloods[6];
					}
				}
				// 좌우 이동일 때
				else if (head.hAxis == 1)
				{
					// 오른쪽 머리
					b_renderer.sprite = Bloods[4];
				}
				else if (head.hAxis == -1)
				{
					// 왼쪽 머리
					b_renderer.sprite = Bloods[6];
				}
				// Idle 상태일 때
				else
				{
					// 정면 머리
					b_renderer.sprite = Bloods[0];
				}
			}
		}
		else if (Input.GetButton("FireVertical") && !Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 제자리에서 위쪽 공격일 때
				if (head.vFire == 1)
				{
					// 후면 머리
					b_renderer.sprite = Bloods[2];
				}
				// 제자리에서 아래쪽 공격일 때
				else if (head.vFire == -1)
				{
					// 정면 머리
					b_renderer.sprite = Bloods[0];
				}
			}
		}
		else if (!Input.GetButton("FireVertical") && Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 제자리에서 좌우 공격일 때
				if (head.hFire == 1)
				{
					// 옆 머리
					b_renderer.sprite = Bloods[4];
				}
				// 제자리에서 좌우 공격일 때
				if (head.hFire == -1)
				{
					// 옆 머리
					b_renderer.sprite = Bloods[6];
				}
			}
		}
	}

	// 공격 중일 때 피눈물 스킨 적용
	void shotBlood()
	{
		// 공격 딜레이 본체에서 가져오기
		maxDelay = player.curTears;

		// 공격 딜레이 감소
		fireDelay -= Time.deltaTime;
		// 머리 딜레이 감소
		headDelay -= Time.deltaTime;

		// 공격 딜레이가 0일 때
		if (fireDelay < 0)
		{
			// 상하 공격일 때
			if (Input.GetButton("FireVertical"))
			{
				// 아래쪽 공격일 때
				if (head.vFire < 0)
				{
					// 정면 머리
					b_renderer.sprite = Bloods[1];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
				// 위쪽 공격일 때
				else if (head.vFire > 0)
				{
					// 후면 머리
					b_renderer.sprite = Bloods[3];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
			}
			// 좌우 공격일 때
			else if (Input.GetButton("FireHorizontal"))
			{
				// 우측 공격
				if (head.hFire > 0)
				{
					// 우측 머리
					b_renderer.sprite = Bloods[5];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
				// 좌측 공격
				else if (head.hFire < 0)
				{
					// 좌측 머리
					b_renderer.sprite = Bloods[7];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
			}
		}
	}
}
