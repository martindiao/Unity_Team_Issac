using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 길다란 돌 파괴
// 가로세로 길이에 맞게 2개의 파괴스프라이트 생성
public class LongRock : MonoBehaviour
{
	// 파괴된 돌 이미지 프리팹
	[SerializeField]
	GameObject m_DestroyRock_Prefab;

	// 프리팹의 클론을 관리하기 위한 변수배열
	private GameObject[] m_clone;

	// 생성할 위치
	float SpawnX;
	float SpawnY;

	// 스프라이트의 가로세로 체크
	bool m_trueXfalseY;

	// 최초 생성시 위치값 설정
	private void Start()
	{
		// 스프라이트의 가로 세로 길이값 설정
		SpawnX = this.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		SpawnY = this.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		SpawnX = SpawnX / 4;
		SpawnY = SpawnY / 4;

		// 세로값이 크다면 가로값은 0 / bool값은 false
		if (SpawnX < SpawnY)
		{
			SpawnX = 0;
			m_trueXfalseY = false;
		}
		// 가로값이 크다면 세로값은 0 / bool값은 true
		else if (SpawnY < SpawnX)
		{
			SpawnY = 0;
			m_trueXfalseY = true;
		}

		m_clone = new GameObject[2];

	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// 폭팔 범위에 닿을 경우
		if (collision.CompareTag("BombRange"))
		{
			// 파괴된 돌 이미지 생성
			m_clone[0] = Instantiate(m_DestroyRock_Prefab, this.transform.position,Quaternion.identity);
			m_clone[0].transform.localPosition = new Vector3(SpawnX, SpawnY, 0);

			// 확인한 가로세로 길이에 따라 x값 혹은 y값 변경
			if (m_trueXfalseY)
				SpawnX *= -1;
			else
				SpawnY *= -1;

			m_clone[1] = Instantiate(m_DestroyRock_Prefab, this.transform.position,Quaternion.identity);
			m_clone[1].transform.localPosition = new Vector3(SpawnX, SpawnY, 0);

			Debug.Log("Boom");

			// 돌 제거
			Destroy(this.gameObject);
		}
	}
}
