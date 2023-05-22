using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 큰돌 파괴
// 파괴 오브젝트는 2개만 출력
public class DestroyBigRock : MonoBehaviour
{
	// 생성할 파괴오브젝트 프리팹
	[SerializeField]
	GameObject m_DestroyRock_Prefab;

	// 생성된 프리팹 클론을 관리할 변수배열 
	GameObject[] m_clone;

	// 생성할 위치값
	float SpawnX;
	float SpawnY;

	// 시작 시 최초 생성될 파괴오브젝트 위치값 조정
	private void Start()
	{
		SpawnX = this.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		SpawnX = SpawnX / 4;
		SpawnY = this.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		SpawnY = SpawnY / 4;

		m_clone = new GameObject[2];

	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// 폭팔 범위에 닿을 경우
		if (collision.CompareTag("BombRange"))
		{
			// 파괴 사운드 추가
			InGameSFXManager.instance.RockDestroy(Random.Range(0, 2));

			// 파괴된 돌 이미지1 생성
			m_clone[0] = Instantiate(m_DestroyRock_Prefab,transform.position,Quaternion.identity);
			m_clone[0].transform.localPosition = new Vector3(SpawnX,SpawnY,0);

			// 생성할 위치 반전
			SpawnX *= -1;
			SpawnY *= -1;

			// 파괴된 돌 이미지2 생성
			m_clone[1] = Instantiate(m_DestroyRock_Prefab, transform.position,Quaternion.identity);
			m_clone[1].transform.localPosition = new Vector3(SpawnX, SpawnY, 0);
			Debug.Log("Boom");

			// 돌 제거
			Destroy(this.gameObject);
		}
	}
}
