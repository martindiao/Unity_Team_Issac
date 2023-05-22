using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 플레이어 아이템 장비 스크립트
public class GetStigma : MonoBehaviour
{
	// 제3의 눈 스프라이트
    public Sprite[] Eyes;

	// renderer
	SpriteRenderer e_renderer;

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

	// 눈 위치 조정
	bool eyeDown;

	private void Start()
	{
		// 각 컴포넌트 가져오기
		e_renderer = GetComponent<SpriteRenderer>();
		head = transform.parent.GetComponent<PlayerHead>();
		player = playerObj.GetComponent<Player>();
	}

	void Update()
    {
		idleEye();
		shotEye();
		eyePos();
	}

	// 이마의 눈 세부 위치 조정
	void eyePos()
	{
		// 눈 위치 조정하지 않았을 때
		if (!eyeDown)
		{
			// 좌우 머리일 때
			if (e_renderer.sprite == Eyes[2] || e_renderer.sprite == Eyes[3] || e_renderer.sprite == Eyes[4] || e_renderer.sprite == Eyes[5])
			{
				// 눈 위치 조정
				transform.position = transform.position + Vector3.down * 0.05f;
				eyeDown = true;
			}
		}
		// 눈 위치 조정했을 때
		else
		{
			// 좌우 머리가 아닐 때
			if (e_renderer.sprite == Eyes[0] || e_renderer.sprite == Eyes[1] || e_renderer.sprite == Eyes[6])
			{
				// 눈 위치 정상화
				eyeDown = false;
				transform.position = transform.position + Vector3.up * 0.05f;
			}
		}
	}

	// 제 3의 눈 스킨 적용
	void idleEye()
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
						// 아래 이동
						if (head.vAxis == -1)
						{
							// 정면 머리
							e_renderer.sprite = Eyes[0];
						}
						// 위 이동
						else
						{
							// 후면 머리
							e_renderer.sprite = Eyes[6];
						}
					}
					// 대각 이동일 때
					// 오른쪽 이동
					else if (head.hAxis == 1)
					{
						// 오른쪽 머리
						e_renderer.sprite = Eyes[2];
					}
					// 왼쪽 이동
					else if (head.hAxis == -1)
					{
						// 왼쪽 머리
						e_renderer.sprite = Eyes[4];
					}
				}
				// 좌우 이동일 때
				// 오른쪽 이동
				else if (head.hAxis == 1)
				{
					// 오른쪽 머리
					e_renderer.sprite = Eyes[2];
				}
				// 왼쪽 이동
				else if (head.hAxis == -1)
				{
					// 왼쪽 머리
					e_renderer.sprite = Eyes[4];
				}
				// Idle 상태일 때
				else
				{
					// 정면 머리
					e_renderer.sprite = Eyes[0];
				}
			}
		}
		// 공격 중일 때
		else if (Input.GetButton("FireVertical") && !Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 위쪽 공격일 때
				if (head.vFire == 1)
				{
					// 후면 머리
					e_renderer.sprite = Eyes[6];
				}
				// 아래쪽 공격일 때
				else if (head.vFire == -1)
				{
					// 정면 머리
					e_renderer.sprite = Eyes[0];
				}
			}
		}
		// 공격 중일 때
		else if (!Input.GetButton("FireVertical") && Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 좌우 공격일 때
				// 오른쪽 머리
				if (head.hFire == 1)
				{
					// 오른쪽 머리
					e_renderer.sprite = Eyes[2];
				}
				// 왼쪽 공격일 때
				if (head.hFire == -1)
				{
					// 왼쪽 머리
					e_renderer.sprite = Eyes[4];
				}
			}
		}
	}

	// 공격 중일 때 제 3의 눈 스킨 적용
	void shotEye()
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
				// 아래쪽 공격일 때
				if (head.vFire < 0)
				{
					// 정면 머리
					e_renderer.sprite = Eyes[1];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
				// 위쪽 공격일 때
				else if (head.vFire > 0)
				{
					// 후면 머리
					e_renderer.sprite = Eyes[6];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
			}
			// 좌우 공격
			else if (Input.GetButton("FireHorizontal"))
			{
				// 오른쪽 공격일 때
				if (head.hFire > 0)
				{
					// 오른쪽 머리
					e_renderer.sprite = Eyes[3];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
				// 왼쪽 공격일 때
				else if (head.hFire < 0)
				{
					// 왼쪽 머리
					e_renderer.sprite = Eyes[5];

					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;
				}
			}
		}
	}
}
