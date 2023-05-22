using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 열쇠 스크립트
public class ItemKey : MonoBehaviour
{
	// 충돌검사
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어 충돌
		if(collision.CompareTag("Player")|| collision.CompareTag("PlayerHead"))
		{
			// 획득 사운드 추가
			InGameSFXManager.instance.KeyGet();

			// 정보 받아오기
			Player player;
			if (collision.CompareTag("Player"))
				player = collision.GetComponent<Player>();
			else
				player = collision.transform.parent.GetComponent<Player>();

			// 플레이어가 소지한 열쇠 수 증가
			player.Keys++;


			// 오브젝트 파괴
			Destroy(this.gameObject);
		}

	}
}
