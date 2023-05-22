using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파이어 오브젝트
public class Fire : MonoBehaviour
{
	// 최대 체력
	public int MaxHp = 6;

	public int Hp;

	// 아이템 드랍 스크립트
	private DropItems drop;

	// 불꽃 애니메이션 
	private GameObject m_childFire;

	public Collider2D f_col;

	private void Start()
	{
		// 불꽃 애니메이션 가져오기
		m_childFire = (transform.gameObject);

		Hp = MaxHp;

		drop = this.gameObject.GetComponent<DropItems>();
	}

	// 충돌검사
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (Hp <= 0)
			return;

		// 눈물과 충돌 시
		if(collision.CompareTag("PlayerBullet"))
		{
			Hp--;
			m_childFire.transform.localScale += new Vector3(-0.1f, -0.1f, 0);
			m_childFire.transform.position += new Vector3(0, -0.1f, 0);
			if (Hp <= 0)
			{
				// 파괴 사운드
				InGameSFXManager.instance.CampFireOff();

				m_childFire.SetActive(false);
				drop.ItemDropEvent();

				f_col.enabled = false;
			}
		}

		// 폭탄에 닿을 시 바로 파괴
		if (collision.CompareTag("BombRange"))
		{
            // 파괴 사운드
            InGameSFXManager.instance.CampFireOff();
            Hp = 0;
			m_childFire.SetActive(false);
			drop.ItemDropEvent();
		}
	}

}
