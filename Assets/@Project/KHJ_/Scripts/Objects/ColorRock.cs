using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 색돌
// 색돌 파괴시 랜덤한 아이템 드랍
public class ColorRock : MonoBehaviour
{
	// 드랍시킬 아이템 목록 List
	public List<GameObject> m_drop_Prefab;

	// 파괴된 돌 이미지
	[SerializeField]
	GameObject m_DestroyRock_Prefab;

	bool a = true;

    private void OnTriggerEnter2D(Collider2D collision)
	{
		// 폭팔 범위에 닿을 경우
		if (collision.CompareTag("BombRange") && a)
		{
			a = false;

			// 파괴된 돌 이미지 생성
			Instantiate(m_DestroyRock_Prefab, this.transform.position,Quaternion.identity);
			Debug.Log("Boom");
			DropRandomItems();
			// 돌의 충돌박스 해제 및 이미지 해제
			Destroy(this.gameObject);
		}
	}

	private void DropRandomItems()
	{

		// 목록 랜덤
		int rand = Random.Range(0, m_drop_Prefab.Count);

		// 선택된 아이템 드랍
		Instantiate(m_drop_Prefab[rand], this.transform.position, Quaternion.identity);

	}

}
