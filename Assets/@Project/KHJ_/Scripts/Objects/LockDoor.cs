using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : MonoBehaviour
{
    public GameObject m_unLockAnim_Prefab;

	private GameObject m_lockAnim_Clone;

	private bool m_isLocked = true;
	private bool m_startUnLock = false;

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

	private void Start()
	{
		// 블록레이어 지정된 자식오브젝트
		m_childBlock = transform.GetChild(5).gameObject;

		// 애니메이터 받아오기
		m_animator = transform.GetChild(3).GetComponent<Animator>();

		CloseTheDoor();

		// 시작지점 설정
		upOffset = transform.position + (Vector3.up * 1.5f);
		downOffset = transform.position + (Vector3.down * 1.5f);
		leftOffset = transform.position + (Vector3.left * 1.5f);
		rightOffset = transform.position + (Vector3.right * 1.5f);


		// 레이캐스트
		RaycastHit2D rayUp = Physics2D.Raycast(upOffset, Vector3.up, 1f, LayerMask.GetMask("Door"));
		RaycastHit2D rayDown = Physics2D.Raycast(downOffset, Vector3.down, 1f, LayerMask.GetMask("Door"));
		RaycastHit2D rayLeft = Physics2D.Raycast(leftOffset, Vector3.left, 1f, LayerMask.GetMask("Door"));
		RaycastHit2D rayRight = Physics2D.Raycast(rightOffset, Vector3.right, 1f, LayerMask.GetMask("Door"));

		// 레이캐스트 배열화
		RaycastHit2D[] rays = new RaycastHit2D[4]
			{rayUp,rayDown,rayLeft,rayRight};

		// 레이캐스트 검사
		for (int i = 0; i < rays.Length; i++)
		{
			if (rays[i])
			{
				m_NextDoor = rays[i].transform.GetChild(4).gameObject;
				break;
			}
		}
	}

	// 충돌검사 트리거
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어 충돌 시
		if (collision.CompareTag("Player"))
		{
			var player = collision.GetComponent<Player>();
			if (m_isLocked && !m_startUnLock&& player.Keys >= 1)
			{
				player.Keys -= 1;

				m_lockAnim_Clone = Instantiate(m_unLockAnim_Prefab, transform);
			//	m_lockAnim_Clone.transform.localPosition = new Vector2(0f,0f);
				StartCoroutine(CheckFinishAnim());
				m_startUnLock = true;
			}

			if (!m_isLocked)
			{
				// 플레이어 해당 위치로 이동
				collision.gameObject.transform.position = m_NextDoor.transform.position;

				// 카메라 위치 이동
				var camera = GameObject.FindGameObjectWithTag("MainCamera");
				camera.transform.position = m_NextDoor.transform.parent.parent.transform.position + new Vector3(0, 0, -10);
			}
		}
	}

	IEnumerator CheckFinishAnim()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.01f);
			
			var animator = m_lockAnim_Clone.GetComponent<Animator>();

			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				Destroy(m_lockAnim_Clone);
				OpenTheDoor();
				var collider = transform.GetChild(3).GetComponent<BoxCollider2D>();
				collider.offset = new Vector2(0, 0.2f);
				collider.size = new Vector2(1f, 0.55f);
				break;
			}
		}
		m_isLocked = false;
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
