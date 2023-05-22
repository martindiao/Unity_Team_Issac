using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 아이템 장비 스크립트
public class GetV3 : MonoBehaviour
{
	// 머리와 동기화
	public GameObject p_Head;
	PlayerHead head;

	// 심장박동 애니메이션
	Animator h_anim;
	// 좌우 이미지 반전을 위한 renderer
	SpriteRenderer h_renderer;

	// 좌우 이미지 반전, 세부 위치 조정을 위한 필드
	bool isFliped;
	bool heartDown;

	// 플레이어 본체
	public GameObject playerObj;

	// Start is called before the first frame update
	void Start()
	{
		// 각 컴포넌트 가져오기
		h_anim = GetComponent<Animator>();
		h_renderer = GetComponent<SpriteRenderer>();
		head = p_Head.GetComponent<PlayerHead>();

		// 필드 초기값
		isFliped = false;
		heartDown = false;
	}

	private void Update()
	{
		animationControl();
		heartPos();
		FlipX();
	}

	// 심장 세부 위치 조정
	void heartPos()
	{
		// 좌우 이동 중
		if (h_anim.GetBool("isHorizontal"))
		{
			// 심장 위치 조정하지 않았을 때
			if (!heartDown)
			{
				// 심장 위치 조정
				transform.position = playerObj.transform.position + new Vector3(0.02f, -0.13f, 0);

				transform.position = transform.position + Vector3.down * 0.1f;
				transform.position = transform.position + Vector3.right * 0.02f;
				heartDown = true;
			}
		}
		// 좌우 이동 중이 아닐 때
		else
		{
			// 심장 위치 조정했을 때
			if (heartDown)
			{
				// 심장 위치 정상화
				transform.position = playerObj.transform.position + new Vector3(0.04f, -0.23f, 0);

				transform.position = transform.position + Vector3.up * 0.1f;
				transform.position = transform.position - Vector3.right * 0.02f;

				heartDown = false;
			}
		}
	}

	// 심장박동
	void animationControl()
	{
		// 애니메이션 구현

		// 상하 이동일 때
		if (head.vAxis != 0)
		{
			// 대각 이동이 아닐 때
			if (head.hAxis == 0)
			{
				// 위쪽 이동
				if (head.vAxis > 0)
				{
					// 후면 애니메이션
					h_anim.SetBool("isUp", true);
					h_anim.SetBool("isHorizontal", false);
				}
				// 아래쪽 이동
				else if(head.vAxis < 0)
				{
					// 정면 애니메이션
					h_anim.SetBool("isUp", false);
					h_anim.SetBool("isHorizontal", false);
				}
			}
			// 대각 이동일 때
			else
			{
				// 좌 우 이동 애니메이션
				h_anim.SetBool("isUp", false);
				h_anim.SetBool("isHorizontal", true);
			}
		}
		// 좌우 이동일 때
		else if (head.hAxis != 0)
		{
			// 좌 우 이동 애니메이션
			h_anim.SetBool("isUp", false);
			h_anim.SetBool("isHorizontal", true);
		}
		// Idle 상태일 때
		else
		{
			// 정면 외 애니메이션 정지
			h_anim.SetBool("isUp", false);
			h_anim.SetBool("isHorizontal", false);
		}
	}

	// 스프라이트 좌우 플립 함수
	void FlipX()
	{
		// 좌 우 이동 버튼을 눌렀을 때
		if (Input.GetButton("Horizontal"))
		{
			// 왼쪽을 보고 있지 않을 때
			if (!isFliped && head.hAxis == -1 && head.hAxis != 0)
			{
				// 스프라이트 플립
				h_renderer.flipX = true;
				isFliped = true;
			}
			// 오른쪽을 보고 있지 않을 때
			else if (isFliped && head.hAxis == 1 && head.hAxis != 0)
			{
				// 스프라이트 플립
				h_renderer.flipX = false;
				isFliped = false;
			}
		}
	}
}
