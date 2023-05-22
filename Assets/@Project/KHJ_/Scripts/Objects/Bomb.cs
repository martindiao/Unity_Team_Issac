using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	// 폭팔 딜레이
	public float m_boomDelay = -1;

	// 폭팔 애니메이션 프리팹
	[SerializeField]
	GameObject m_BombAction_Prefab;

	// 폭탄 효과
	SpriteRenderer b_renderer;

	// 폭탄 터진 이펙트 리스트
	public List<GameObject> m_bombEffect;

	// 생성한 프리팹의 정보를 담을 클론
	private GameObject m_Prefab_Clone;

	// 생성한 이펙트 정보를 담을 클론
	private GameObject m_Effect_Clone;

	private void Awake()
	{
		b_renderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		// 생성과 동시에 코루틴 시작
		StartCoroutine(DestroyBomb());
	}

	public void setDelay(float dt)
	{
		// 폭발 딜레이 설정
		m_boomDelay = dt;
	}

	// 폭탄 폭팔 딜레이 코루틴
	IEnumerator DestroyBomb()
	{
		// 딜레이가 설정되지 않았을 경우 기본값 부여
		if (m_boomDelay == -1)
		{
			setDelay(0);
		}
		// 딜레이가 존재한 상태라면 폭탄 효과 시작
		if (m_boomDelay > 0)
		{
			StartCoroutine(colorChange());
		}


		yield return new WaitForSeconds(m_boomDelay);

		m_boomDelay = 0;

		m_Prefab_Clone = Instantiate(m_BombAction_Prefab, this.transform.position + Vector3.up, Quaternion.identity);

		m_Effect_Clone = Instantiate(m_bombEffect[Random.Range(0,m_bombEffect.Count)],this.transform.position ,Quaternion.identity);

		InGameSFXManager.instance.Boom();

		this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		this.GetComponent<SpriteRenderer>().sprite = null;
		StartCoroutine(DestroyBombAction());
	}

	// 폭팔 후 오브젝트 삭제 코루틴
	IEnumerator DestroyBombAction()
	{
		yield return new WaitForSeconds(m_boomDelay + 0.5f);

		Debug.Log("boom");

		Destroy(m_Prefab_Clone);
		Destroy(this.gameObject);
	}

	// 폭탄 효과
	IEnumerator colorChange()
	{
		// 폭탄이 터지기 전까지 실감나는 효과 부여
		while(m_boomDelay != 0)
		{
			b_renderer.color = Color.yellow;

			yield return new WaitForSeconds(0.05f);
			b_renderer.color = Color.red;

			yield return new WaitForSeconds(0.05f);
			b_renderer.color = Color.white;

			yield return new WaitForSeconds(0.5f);
		}
	}

	// 충돌검사
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 진행방향에 무언가 있을 경우 처리
		if(collision != null)
		{

		}
	}

}
