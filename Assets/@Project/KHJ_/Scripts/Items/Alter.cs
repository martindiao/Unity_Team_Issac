using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 제단
public class Alter : MonoBehaviour
{
	// 등장 아이템 목록
	public enum Collection
	{
		None,
		TheSadOnion,
		BloodOfTheMartyr,
		MagicMushroom,
		Stigmata,
		OneUp,
		NineVolt,
		Abaddon,
		Brimstone,
		LessThanThree
	}

	float _tear = 0.1f;    // 공격 속도
	float _speed = 0.5f;    // 이동 속도
	float _power = 0.5f;    // 데미지
	int _heart = 2;     // 최대 체력

	// 아이템 랜덤 등장
	int m_randomCode;

	// 등장할 아이템
	public Collection collection;

	// 아이템 목록 List
	[SerializeField]
	List<GameObject> m_collection;

	// 생성된 아이템 관리용 
	private GameObject m_childClone;

	public Collider2D a_col;

	// 최초 생성 시
	private void Start()
	{
		// 목록 갱신
		//	m_collection = new List<GameObject>();


		// 등장 아이템 선택
		m_randomCode = Random.Range(1, m_collection.Count);

		// 현재 등장한 아이템 타입
		collection = (Collection)m_randomCode;

		// 아이템 생성후 관리하기위해 등록
		m_childClone = Instantiate(m_collection[m_randomCode], transform.GetChild(0).transform);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어 충돌 시
		if (collision.CompareTag("Player") || collision.CompareTag("PlayerHead"))
		{
			// 아이템에 따른 처리
			CollectionEffect();
		}
	}

	// 아이템에 따른 효과 부여
	private void CollectionEffect()
	{
		var player = FindObjectOfType<Player>();

		switch (collection)
		{
			case Collection.TheSadOnion:
				if (player.haveItems[0] != Collection.TheSadOnion)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[0] = Collection.TheSadOnion;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.TheSadOnion, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;
			case Collection.BloodOfTheMartyr:
				if (player.haveItems[1] != Collection.BloodOfTheMartyr)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[1] = Collection.BloodOfTheMartyr;

					// 플레이어에게 효과 적용
					player.curPower += _power;
					player.curPower += _power;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.BloodOfTheMartyr, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;
			case Collection.MagicMushroom:
				if (player.haveItems[2] != Collection.MagicMushroom)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[2] = Collection.MagicMushroom;

					// 플레이어에게 효과 적용
					player.curTears -= _tear;
					player.curPower += _power;
					player.speed += _speed;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.MagicMushroom, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;
			case Collection.Stigmata:
				if (player.haveItems[3] != Collection.Stigmata)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[3] = Collection.Stigmata;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.Stigmata, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;

			case Collection.OneUp:
				if (player.haveItems[4] != Collection.OneUp)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[4] = Collection.OneUp;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.OneUp, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;

			case Collection.NineVolt:
				if (player.haveItems[5] != Collection.NineVolt)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[5] = Collection.NineVolt;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.NineVolt, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;

			case Collection.Abaddon:
				if (player.haveItems[6] != Collection.Abaddon)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[6] = Collection.Abaddon;

					// 플레이어에게 효과 적용
					player.speed -= _speed;
					player.speed -= _speed;
					player.curTears -= _tear;
					player.curTears -= _tear;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.Abaddon, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;

			case Collection.Brimstone:
				if (player.haveItems[7] != Collection.Brimstone)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[7] = Collection.Brimstone;

					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.Brimstone, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;

			case Collection.LessThanThree:
				if (player.haveItems[8] != Collection.LessThanThree)
				{
					// 플레이어에게 아이템 부여
					player.haveItems[8] = Collection.LessThanThree;

					// 플레이어에게 효과 적용
					player.curHeart += _heart;
					player.maxRealHeart += _heart;
					player.curHeart += _heart;
					player.maxRealHeart += _heart;


					// 플레이어 애니메이션 재생
					player.GetItem();
					player.StartCoroutine(player.getEff(Collection.LessThanThree, a_col));

					// 아이템 삭제 메소드
					DestroyCollection();
				}
				break;
		}
	}

	// 아이템 삭제 메소드
	private void DestroyCollection()
	{
		// 아이템에 적용된 애니메이터 비활성호
		m_childClone.GetComponent<Animator>().enabled = false;

		// n초 후 삭제
		// 플레이어가 아이템 습득 시 손에 들고있는 애니매이션 재생 후
		// 없어지기에 Destroy의 2번쨰 인수의 값 변경 해야됨
		Destroy(m_childClone);
	}
}
