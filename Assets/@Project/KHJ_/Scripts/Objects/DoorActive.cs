using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================

//		사용 안하는 스크립트

//===================================

// 문 스크립트
// 두개의 문 오브젝트가 페어로 작동해야됨
public class DoorActive : MonoBehaviour
{
	// 플레이어가 이동할 위치 오브젝트
	// 좌표용
	public GameObject m_nextDoor;

	// 충돌검사
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어 충돌 시
		if(collision.CompareTag("Player"))
		{
			// 해당 위치로 이동
			collision.gameObject.transform.position = m_nextDoor.transform.position;
		}
	}
}
