using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 비사용 스크립트
public class EnemyBullet : MonoBehaviour
{
	// 유사 중력
	public float gravity;
	// 바닥 판정에 사용할 변수
	float tearHigh;

	// rigidbody2D
	Rigidbody2D t_rigid;

	// 파괴 애니메이션
	public GameObject tearPoofa;

	private void Awake()
	{
		t_rigid = GetComponent<Rigidbody2D>();
		// 최초 중력 가속도
		tearHigh = -0.007f;
	}

	private void Update()
	{
		High();
		onGround();
	}

	void High()
	{
		// 중력 가속도에 따라 천천히 땅으로 낙하
		t_rigid.AddForce(Vector3.down * gravity, ForceMode2D.Impulse);
		tearHigh += -0.007f;
	}

	void onGround()
	{
		// 중력 가속도가 2가 되었을 때 바닥에 닿았다고 판정
		if (tearHigh < -2f)
		{
			// 오브젝트 삭제
			Destroy(gameObject);

			// 눈물 폭발? 애니메이션 출력
			GameObject tearpoofa = Instantiate(tearPoofa, transform.position, Quaternion.identity);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			// 오브젝트 삭제
			Destroy(gameObject);

			// 눈물 폭발? 애니메이션 출력
			GameObject tearpoofa = Instantiate(tearPoofa, transform.position, Quaternion.identity);

			// 몸통 피격시 Hit함수 호출
			Player player = collision.GetComponent<Player>();
			player.Hit(1);
		}
		else if (collision.tag == "PlayerHead")
		{
			// 오브젝트 삭제
			Destroy(gameObject);

			// 눈물 폭발? 애니메이션 출력
			GameObject tearpoofa = Instantiate(tearPoofa, transform.position, Quaternion.identity);

			// 머리 피격시 PlayerHead 내부 코드를 이용해 몸통의 Hit함수 호출
			PlayerHead player = collision.GetComponent<PlayerHead>();
			player.Hit(1);
		}
	}
}
