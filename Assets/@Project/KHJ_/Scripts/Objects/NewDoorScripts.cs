using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 문 스크립트 수정본
public class NewDoorScripts : MonoBehaviour
{
	// 이동할 지점
	public GameObject m_NextDoor;

	// 문 애니메이터
	private Animator m_animator;

	// 블럭레이어로 처리된 자식 오브젝트
	private GameObject m_childBlock;

	// 문의 상태체크
	public bool m_isOpen;

	// 네방향 레이캐스트 시작지점
	Vector3 upOffset;
	Vector3 downOffset;
	Vector3 leftOffset;
	Vector3 rightOffset;

	MapTrigger floor;

	private void Start()
	{
		// 블록레이어 지정된 자식오브젝트
		m_childBlock = transform.GetChild(5).gameObject;

		// 애니메이터 받아오기
		m_animator = transform.GetChild(3).GetComponent<Animator>();

		floor = gameObject.transform.parent.GetChild(1).GetComponent<MapTrigger>();

		// 시작지점 설정
		upOffset = transform.position + (Vector3.up*1.5f);
		downOffset = transform.position + (Vector3.down*1.5f);
		leftOffset = transform.position + (Vector3.left*1.5f);
		rightOffset = transform.position + (Vector3.right*1.5f);


		// 레이캐스트
		RaycastHit2D rayUp = Physics2D.Raycast(upOffset, Vector3.up, 1f, LayerMask.GetMask("Door"));
		RaycastHit2D rayDown = Physics2D.Raycast(downOffset, Vector3.down, 1f, LayerMask.GetMask("Door"));
		RaycastHit2D rayLeft = Physics2D.Raycast(leftOffset, Vector3.left, 1f, LayerMask.GetMask("Door"));
		RaycastHit2D rayRight = Physics2D.Raycast(rightOffset, Vector3.right, 1f, LayerMask.GetMask("Door"));

		// 레이캐스트 배열화
		RaycastHit2D[] rays = new RaycastHit2D[4]
			{rayUp,rayDown,rayLeft,rayRight};

		// 레이캐스트 검사
		for(int i = 0; i < rays.Length; i++)
		{
			if(rays[i])
			{
				m_NextDoor = rays[i].transform.GetChild(4).gameObject;
				break;
			}
		}
	}

	private void Update()
	{
		if (!floor.isClear && m_isOpen)
		{
			CloseTheDoor();
		}
		else if (floor.isClear && !m_isOpen)
		{
			OpenTheDoor();
		}
		
	}

	// 충돌검사 트리거
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어 충돌 시
		if (collision.CompareTag("Player"))
		{
			// 플레이어 해당 위치로 이동
			collision.gameObject.transform.position = m_NextDoor.transform.position;

			// 카메라 위치 이동
			var camera = GameObject.FindGameObjectWithTag("MainCamera");
			camera.transform.position = m_NextDoor.transform.parent.parent.transform.position + new Vector3(0,0,-10);
		}
	}

	// 문열림 메서드
	public void OpenTheDoor()
	{
		m_animator.Play("OpenDoor");

		m_isOpen = true;
		// 블럭 오브젝트 비활성화
		m_childBlock.SetActive(false);
	}

	// 문닫힘 메서드
	public void CloseTheDoor()
	{
		m_animator.Play("CloseDoor");

		m_isOpen = false;
		// 블럭 오브젝트 활성화
		m_childBlock.SetActive(true);
	}

}
