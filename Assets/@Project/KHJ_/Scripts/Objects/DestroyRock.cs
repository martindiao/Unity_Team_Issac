using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyRock : MonoBehaviour
{
	// 파괴된 돌 이미지
	[SerializeField]
	GameObject m_DestroyRock_Prefab;

	//GameObject m_ChildCollider;

	private void Start()
	{
		//m_ChildCollider = this.transform.GetChild(0).gameObject;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// 폭팔 범위에 닿을 경우
		if(collision.CompareTag("BombRange"))
		{
			// 파괴 사운드 추가
            InGameSFXManager.instance.RockDestroy(Random.Range(0, 2));

            // 파괴된 돌 이미지 생성
            Instantiate(m_DestroyRock_Prefab,this.transform.position,Quaternion.identity);
			Debug.Log("Boom");

			// 돌 제거
			Destroy(this.gameObject);
			//this.GetComponent<BoxCollider2D>().enabled = false;
			//this.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
