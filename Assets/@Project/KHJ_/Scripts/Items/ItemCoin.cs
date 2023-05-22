using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코인 아이템 
public class ItemCoin : MonoBehaviour
{
	// 애니메이터
	private Animator animator;

	// 코인 획득 애니메이션
	[SerializeField]
	GameObject m_getCoinAction;

	// 획득 애니메이션 관리 변수
	private GameObject m_clone;

	// 반짝이는 효과를 주기 위한 bool
	private bool idleAction = false;

	// 코인 생성 시
	private void Start()
	{
		// 애니메이션 재생
		animator = GetComponent<Animator>();
		animator.Play("DropCoin");

		// 반짝이는 효과에 딜레이주기
		StartCoroutine(CoinActionDelay());
	}

	private void Update()
	{
		// 반짝이는 효과 주기
		if (idleAction)
		{
			idleAction = false;
			animator.Play("IdleCoin");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 충돌한 물체의 태그가 플레이어일때
		if(collision.CompareTag("Player")|| collision.CompareTag("PlayerHead"))
		{
			InGameSFXManager.instance.CoinGet();

			// 플레이어의 소지금 +1
			Player player;
			if(collision.CompareTag("Player"))
				player = collision.GetComponent<Player>();
			else
				player = collision.transform.parent.GetComponent<Player>();
			player.Coins++;

			// 없어지는 애니메이션 재생
			m_clone = Instantiate(m_getCoinAction,transform.position,Quaternion.identity);

			Destroy(m_clone, 0.3f);

			// 자기자신 삭제
			Destroy(this.gameObject);
		}
		
	}

	IEnumerator CoinActionDelay()
	{
		while (true)
		{
			// 반짝이는 효과 딜레이 시간
			yield return new WaitForSeconds(5f);
			idleAction = true;
		}
	}
}
