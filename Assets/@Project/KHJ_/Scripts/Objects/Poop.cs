using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 똥 오브젝트
public class Poop : MonoBehaviour
{
	// 체력(1대당 1씩 까임)
	[SerializeField]
	int Hp;

	// 아이템 드랍 스크립트
	private DropItems drop;

	public int HP { get { return Hp; } }

	// 체력당 출력할 이미지 List
	public List<Sprite> sprites = new List<Sprite>();

	// 최초 시작 시 Hp를 List의 크기만큼 설정
	private void Start()
	{
		Hp = sprites.Count - 1;
		drop = this.gameObject.GetComponent<DropItems>();
	}

	// 충돌 검사
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Hp가 0보다 작아진다면 0으로 설정
		if (Hp <= 0)
			return;
		// 눈물 충돌 시
		if(collision.CompareTag("PlayerBullet"))
		{
			// Hp -1
			Hp--;
			// 만약 Hp가 0보다 작아진다면 
			if (Hp <= 0)
			{
				// Hp를 0으로 설정
				Hp = 0;
				// 충돌검사용 BoxCollider 비활성화
				this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				drop.ItemDropEvent();

				// 파괴 사운드 추가
				InGameSFXManager.instance.Poop();
			}
			// 충돌때 마다 이미지 바꾸기
			this.transform.GetComponent<SpriteRenderer>().sprite = sprites[Hp];

			
		}

		if(collision.CompareTag("BombRange"))
		{
			// Hp --
			Hp -= 5;
			// 만약 Hp가 0보다 작아진다면 
			if (Hp <= 0)
			{
				// Hp를 0으로 설정
				Hp = 0;
				// 충돌검사용 BoxCollider 비활성화
				this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				drop.ItemDropEvent();
			}
			// 충돌때 마다 이미지 바꾸기
			this.transform.GetComponent<SpriteRenderer>().sprite = sprites[Hp];
		}
	}
}
