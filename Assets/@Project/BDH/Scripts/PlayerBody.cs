using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
	// 플레이어 본체
	public GameObject playerObj;
	Player player;

	private void Start()
	{
		// 플레이어 컴포넌트 가져오기
		player = playerObj.GetComponent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 머리 제외 한 몸이 폭탄에 맞았을 시
		if (collision.tag == "BombRange")
		{
			// 히트
			player.Hit(2);
		}

		// 머리 제외 한 몸이 불에 닿았을 시
		if (collision.name.Contains("Fire"))
		{
			// 히트
			player.Hit(1);
		}
	}
}
