using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	// 유사 중력
	public float gravity;
	// 바닥 판정에 사용할 변수
	float tearHigh;

	// rigidbody2D
	Rigidbody2D t_rigid;

	// 파괴 애니메이션
	public GameObject tearPoofa;
	public GameObject bigTearPoofa;

	private void Start()
	{
		t_rigid = GetComponent<Rigidbody2D>();
		// 최초 중력 가속도
		tearHigh = -0.022f;
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
		tearHigh += -0.022f;
	}

	void onGround()
	{
		// 중력 가속도가 1.9f가 되었을 때 바닥에 닿았다고 판정
		if (tearHigh < -1.9f)
		{
			// 오브젝트 삭제
			Destroy(gameObject);
			InGameSFXManager.instance.TearDestroy();

			if (gameObject.name.Contains("x01Tear") || gameObject.name.Contains("x01Blood"))
			{
				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(tearPoofa, transform.position, Quaternion.identity);
			}

			if (gameObject.name.Contains("x02Tear") || gameObject.name.Contains("x02Blood"))
			{
				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(bigTearPoofa, transform.position, Quaternion.identity);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 눈물이 벽에 닿거나 적에게 닿았을 때
		if (collision.gameObject.layer == LayerMask.NameToLayer("Block") || collision.tag == "Enemy")
		{
			// 장작더미는 위쪽을 통과
			if (!collision.name.Contains("Base"))
			{
				// 오브젝트 삭제
				Destroy(gameObject);
				InGameSFXManager.instance.TearDestroy();

				if (gameObject.name.Contains("x01Tear") || gameObject.name.Contains("x01Blood"))
				{
					// 눈물 폭발? 애니메이션 출력
					GameObject tearpoofa = Instantiate(tearPoofa, transform.position, Quaternion.identity);
				}

				if (gameObject.name.Contains("x02Tear") || gameObject.name.Contains("x02Blood"))
				{
					// 눈물 폭발? 애니메이션 출력
					GameObject tearpoofa = Instantiate(bigTearPoofa, transform.position, Quaternion.identity);
				}
			}
		}

		// 눈물이 불이나 똥무더기에 닿았을 때
		if (collision.name.Contains("Fire") || collision.tag.Contains("Poop"))
		{
			// 오브젝트 삭제
			Destroy(gameObject);
			InGameSFXManager.instance.TearDestroy();

			if (gameObject.name.Contains("x01Tear") || gameObject.name.Contains("x01Blood"))
			{
				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(tearPoofa, transform.position, Quaternion.identity);
			}

			if (gameObject.name.Contains("x02Tear") || gameObject.name.Contains("x02Blood"))
			{
				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(bigTearPoofa, transform.position, Quaternion.identity);
			}
		}

		// 피 눈물 일시 적 공격 상쇄
		if (collision.tag == "EnemyBullet")
		{
			// 피 눈물일 때만 발동
			if (gameObject.name.Contains("x01Blood"))
			{
				// 적 눈물 삭제
				Bullet_Script e_bullet = collision.GetComponent<Bullet_Script>();
				e_bullet.bulletDestroy();

				// 오브젝트 삭제
				Destroy(gameObject);
				InGameSFXManager.instance.TearDestroy();

				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(tearPoofa, transform.position, Quaternion.identity);
			}

			// 피 눈물일 때만 발동
			if (gameObject.name.Contains("x02Blood"))
			{
				// 적 눈물 삭제
				Bullet_Script e_bullet = collision.GetComponent<Bullet_Script>();
				e_bullet.bulletDestroy();

				// 오브젝트 삭제
				Destroy(gameObject);
				InGameSFXManager.instance.TearDestroy();

				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(bigTearPoofa, transform.position, Quaternion.identity);
			}
		}
	}
}
