using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 폭탄 아이템
public class ItemBomb : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 몸통 충돌
		if(collision.CompareTag("Player")|| collision.CompareTag("PlayerHead"))
		{
			// 플레이어 컴포넌트에 폭탄++
			Player player;
			if(collision.CompareTag("Player"))
				player = collision.GetComponent<Player>();
			else
				player = collision.transform.parent.GetComponent<Player>();
			player.Bombs++;

			Debug.Log("GetBomb");

			// 자기자신 삭제
			Destroy(this.gameObject);
		}

	}
}
