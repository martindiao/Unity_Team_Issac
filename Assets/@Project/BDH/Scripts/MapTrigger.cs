using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MonoBehaviour
{
	// 방의 활성화 상태
    public bool mapActivate;
	// 방이 활성화 될 시 1회 맵 트리거 발동
    public bool mapTrigger;
	// 맵에 몬스터가 없을 시
	public bool isClear;

	// 몬스터 존재 여부를 확인할 리스트
	List<GameObject> enemyList;
	// 보스 몬스터 존재 여부를 확인할 리스트
	List<GameObject> Boss;

	// 보스방 입장시 인트로
	BossIntroMove bossIntro;

	// 보스전 상태
	bool nowBoss;

	private void Start()
	{
		// 리스트 생성
		enemyList = new List<GameObject>();
		Boss = new List<GameObject>();

		// 보스 인트로 오브젝트 찾기
		bossIntro = FindObjectOfType<BossIntroMove>();

	}

	// 방의 종류
	public enum roomType
	{
		Start,
		Enemy,
		Shop,
		Gold,
		Boss
	}
	
	// 방 오브젝트의 유형
	public roomType r_type;

	// 스폰할 위치
	public GameObject[] spwanVec;

	// 스폰할 오브젝트
	public GameObject[] spwanObj;
	

	private void Update()
	{
		// 맵이 활성화 되지 않았고 맵 트리거가 발동했을 때 최초 1회에 한하여
		if (!mapActivate && mapTrigger)
		{
			// 방 유형에 따라서
			switch(r_type)
			{
				// 몬스터 스폰
				case roomType.Enemy:
					MonsterSpawn();
					// 문 닫기 위해 값 변경
					isClear = false;
					break;
				// 상점 아이템 스폰
				case roomType.Shop:
					ProductSpawn();
					break;
				// 아이템 제단 스폰
				case roomType.Gold:
					ItemSpawn();
					break;
				// 보스 스폰
				case roomType.Boss:
					InBoss();
					// 문 닫기 위해 필드 값 변경
					nowBoss = true;
					isClear = false;
					break;
			}

			// 방 활성화
			mapActivate = true;
		}

		// 방 안에 몬스터가 존재여부 확인
		if (enemyList.Count > 0)
		{
			// 만약 몬스터가 죽었을 경우
			for (int i = 0; i < enemyList.Count; i++)
			{
				// 몬스터 리스트에서 몬스터 삭제
				if (enemyList[i] == null)
				{
					enemyList.RemoveAt(i);
				}
			}
		}
		
		// 만약 몬스터가 방에 한 마리도 존재하지 않을 경우
		else if (enemyList.Count <= 0 && !nowBoss)
		{
			// 방 클리어
			isClear = true;
		}

		// 보스전이였고 보스가 죽었을 때
		if (nowBoss == true && Boss[0] == null)
		{
			// 방 클리어
			nowBoss = false;
			isClear = true;

			// 보스 클리어 아이템 생성
            GameObject spawnObj = Instantiate(spwanObj[1], spwanVec[1].transform.position, spwanVec[1].transform.rotation);
        }
	}

	// 몬스터 스폰 함수
	void MonsterSpawn()
	{
		// 스폰 갯수 만큼
		for (int i = 0; i < spwanVec.Length; i++)
		{
			// 랜덤 몬스터를
			int j = Random.Range(0, spwanObj.Length);
			
			// 스폰 위치에 소환
			GameObject spawnObj = Instantiate(spwanObj[j], spwanVec[i].transform.position, spwanVec[i].transform.rotation);

			// 몬스터 리스트에 추가
			enemyList.Add(spawnObj);
		}
	}

	// 상점 상품 스폰 함수
	void ProductSpawn()
	{
		// 상점 내 상품 갯수 만큼
		for (int i = 0; i < spwanVec.Length; i++)
		{
			// 상품 스폰
			GameObject spawnObj = Instantiate(spwanObj[0], spwanVec[i].transform.position, spwanVec[i].transform.rotation);
		}
	}

	// 아이템 제단 스폰 함수
	void ItemSpawn()
	{
		// 아이템 스폰
		GameObject spawnObj = Instantiate(spwanObj[0], spwanVec[0].transform.position, spwanVec[0].transform.rotation);
	}

	// 보스 스폰 함수
	void InBoss()
	{
		// 보스 스폰 위치에
		GameObject spawnObj = Instantiate(spwanObj[0], spwanVec[0].transform.position, spwanVec[0].transform.rotation);

		// 보스 스폰
		Boss.Add(spawnObj);

		// 보스 인트로 BGM 시작
		InGameSFXManager.instance.Castleport();
		Invoke("PlayBossBGM", 1.8f);
		// 보스 인트로
		bossIntro.StartIntro();
	}

	// 보스 인트로 BGM 함수
	void PlayBossBGM()
	{
		InGameSoundManager.instance.PlayBossBGM();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 방에 플레이어가 진입할 시
		if (collision.tag == "Player")
		{
			// 맵 트리거 발동
			mapTrigger = true;
		}

		// 방에 몬스터가 추가 생성, 부활 중일 시
		if (collision.tag == "Enemy")
		{
			// 몬스터 리스트에 추가
			enemyList.Add(collision.gameObject);
			isClear = false;
		}
	}
}
