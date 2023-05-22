using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================

//		사용 안하는 스크립트

//===================================

// 문관리 스크립트
public class DoorAniScripts : MonoBehaviour
{
	// 문 애니메이션 
	private Animator[] m_doorAni;
	
	// 문이 닫혀있을 떄의 콜라이더
	private GameObject[] m_rigidBox;

	private void Start()
	{
		m_doorAni = new Animator[2];
		m_rigidBox = new GameObject[2];

		for (int i = 0; i < 2; i++)
		{
			m_doorAni[i] = this.transform.GetChild(i).GetChild(3).GetComponent<Animator>();
			m_rigidBox[i] = this.transform.GetChild(i).GetChild(5).gameObject;

		}

		// 최초 생성 시 오픈상태로 생성
		// 바꿔도 됨
		OpenTheDoor();
	}

	// 열리는 애니메이션
	public void OpenTheDoor()
	{
		for (int i = 0; i < 2; i++)
		{
			m_doorAni[i].Play("OpenDoor");
			//if (m_rigidBox[i].active == true)
			m_rigidBox[i].SetActive(false);
		}
	}

	// 닫히는 애니메이션
	public void CloseTheDoor()
	{
		for (int i = 0; i < 2; i++)
		{
			m_doorAni[i].Play("CloseDoor");
			//if (m_rigidBox[i].active == false)
			m_rigidBox[i].SetActive(true);
		}

	}

	// 열린 후 또는 닫힌 후 대기 액션(수정해야될수도 있음)
	public void IdleTheDoor()
	{
		for (int i = 0; i < 2; i++)
			m_doorAni[i].Play("IdleDoor");
	}

}
