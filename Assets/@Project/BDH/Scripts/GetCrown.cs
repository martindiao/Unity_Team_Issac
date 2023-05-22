using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 아이템 장비 스크립트
public class GetCrown : MonoBehaviour
{
	// 가시왕관 스프라이트
	public Sprite[] Crowns;

	// renderer
	SpriteRenderer c_renderer;

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
		c_renderer = GetComponent<SpriteRenderer>();
		head = transform.parent.GetComponent<PlayerHead>();
		player = playerObj.GetComponent<Player>();
	}

	void Update()
	{
		idleCorwn();
		shotCorwn();
	}

	// 가시왕관 스킨 적용
	void idleCorwn()
	{
		if (!Input.GetButton("FireHorizontal") && !Input.GetButton("FireVertical"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 위아래 공격일 때
				if (head.vAxis != 0)
				{
					// 대각 이동이 아닐 때
					if (head.hAxis == 0)
					{
						if (head.vAxis != 0)
						{
							// 위 아래 머리
							c_renderer.sprite = Crowns[0];
						}
					}
					// 대각 이동일 때
					else
					{
						// 옆 머리
						c_renderer.sprite = Crowns[2];
					}
				}
				// 좌우 이동일 때
				else if (head.hAxis != 0)
				{
					// 옆 머리
					c_renderer.sprite = Crowns[2];
				}
				// Idle 상태일 때
				else
				{
					// 위 아래 머리
					c_renderer.sprite = Crowns[0];
				}
			}
		}
		// 공격 중일 때
		else if (Input.GetButton("FireVertical") && !Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 제자리에서 위쪽 공격일 때
				if (head.vFire != 0)
				{
					// 위 아래 머리
					c_renderer.sprite = Crowns[0];
				}
			}
		}
		// 공격 중일 때
		else if (!Input.GetButton("FireVertical") && Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 제자리에서 좌우 공격일 때
				if (head.hFire != 0)
				{
					// 옆 머리
					c_renderer.sprite = Crowns[2];
				}
			}
		}
	}

	// 공격 중일 때 가시왕관 스킨 적용
	void shotCorwn()
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
			if (Input.GetButton("FireVertical"))
			{
				// 위 아래 공격일 때
				if (head.vFire != 0)
				{
					// 위 아래 머리
					c_renderer.sprite = Crowns[1];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
			}
			else if (Input.GetButton("FireHorizontal"))
			{
				// 좌우 공격일 때
				if (head.hFire != 0)
				{
					// 좌우 머리
					c_renderer.sprite = Crowns[3];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
			}
		}
	}
}
