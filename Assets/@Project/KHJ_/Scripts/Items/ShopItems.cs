using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 샵 
public class ShopItems : MonoBehaviour
{
	// 판매 할 아이템 이미지
	public List<Sprite> ItemSprite;
	
	// 판매할 아이템 이미지에 맞는 아이템
	public List<GameObject> Items;

	// 숫자 오브젝트
	public List<GameObject> NumberList;

	// 아이템의 가격
	[SerializeField]
	private int Price;

	// 판매할 아이템 랜덤 선정
	private int rand;

	// 아이템 이미지 적용하기 위한 변수
	private SpriteRenderer m_itemSprite;

	// 가격표시용(1의 자리) 포지션
	private Transform UnitsPosition;
	// 가격표시용(10의 자리) 포지션
	private Transform TensPosition;
	// 달러표시용 오브젝트
	private GameObject DollarObject;

	// 가격표시용으로 소환할 오브젝트(1의 자리)
	private GameObject m_unitsClone;
	// 가격표시용으로 소환할 오브젝트(10의 자리)
	private GameObject m_tensClone;

	// 판매할 오브젝트 생성 변수
	private GameObject m_itemClone;
	

	// 생성 시
	private void Start()
	{
		// 판매할 오브젝트 선정
		rand = Random.Range(0, ItemSprite.Count);
		// 가격 선정(현재 1~ 15)
		Price = Random.Range(0, 8) + 1;

		// 선정된 아이템의 이미지 적용
		m_itemSprite = transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
		m_itemSprite.sprite = ItemSprite[rand];
		
		// 선정된 아이템 미리 생성 후 비활성화
		m_itemClone = Instantiate(Items[rand], transform.GetChild(0).position, Quaternion.identity);
		m_itemClone.SetActive(false);

		// 숫자 표시위치 설정
		UnitsPosition = transform.GetChild(2);
		TensPosition = transform.GetChild(3);
		DollarObject = transform.GetChild(1).gameObject;

		// 설정된 가격 이미지로 변환
		m_unitsClone = Instantiate(NumberList[Price % 10],UnitsPosition);
		if(Price >=10)
			m_tensClone = Instantiate(NumberList[Price / 10],TensPosition);

	}

	// 충돌검사
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 이미지가 없다면 반환
		if (m_itemSprite == null)
			return; 

		// 플레이어 또는 플레이어의 머리와 충돌 시
		if(collision.CompareTag("Player")||collision.CompareTag("PlayerHead"))
		{
			// 플레이어 값 받아오기
			Player player;
			if (collision.CompareTag("Player"))
				player = collision.GetComponent<Player>();
			else
				player = collision.transform.parent.GetComponent<Player>();

			// 플레이어가 가진 소지금과 가격 비교
			if(player.Coins >= Price)
			{
				// 소지금에서 가격만큼 빼기
				player.Coins -= Price;

				// 비활성화 된 아이템 활성화
				m_itemClone.SetActive(true);

				// 확인용 Debug.Log
				Debug.Log("Buy Item");

				// 콜라이더 비활성화
				this.transform.GetComponent<BoxCollider2D>().enabled = false;

				// 생성된 아이템을 제외한 그외 오브젝트들 파괴
				Destroy(m_itemSprite);
				DollarObject.SetActive(false);
				Destroy(m_tensClone);
				Destroy(m_unitsClone);

			}
		}
	}

}
