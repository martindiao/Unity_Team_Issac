using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 하트
public class ItemHeart : MonoBehaviour
{
	// 하트의 종류
	public enum HeartType
	{
		Red,
		HalfRed,
		Soul,
		HalfSoul,
	}

	// 하트 분별용 변수
	public HeartType Type;

	// 회복량
	int m_Heart;
	int m_Soul;

	// 시작 시 타입에 따라 회복량 설정
	private void Start()
	{
		switch(Type)
		{
			case HeartType.Red:
				m_Heart = 2;
				m_Soul = 0;
				break;

			case HeartType.HalfRed:
				m_Heart = 1;
				m_Soul = 0;
				break;

			case HeartType.Soul:
				m_Heart = 0;
				m_Soul = 2;
				break;

			case HeartType.HalfSoul:
				m_Heart = 0;
				m_Soul = 1;
				break;
		}	
	}

	// 충돌 검사
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어의 데이터 가져오기
		Player player;

		// 플레이어가 충돌하면
		if (collision.CompareTag("Player")|| collision.CompareTag("PlayerHead"))
		{
			
			if (collision.CompareTag("Player"))
				player = collision.GetComponent<Player>();
			else
				player = collision.transform.parent.GetComponent<Player>();

			if ((player.curRealHeart + m_Heart <= player.maxRealHeart) && m_Heart != 0)
			{
				player.curHeart += m_Heart;
				player.curRealHeart += m_Heart;
				
				// 자기자신 지우기
				Destroy(this.gameObject);
			}
			else if (player.curHeart + m_Soul <= player.maxHeart && m_Soul != 0)
			{
				player.curHeart += m_Soul;
				player.curSoulHeart += m_Soul;

				// 자기자신 지우기
				Destroy(this.gameObject);
			}
		}
	}
}
