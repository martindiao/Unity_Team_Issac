using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	// 플레이어 오브젝트를 제어하기 위한 상위 오브젝트
	public GameObject parentObj;

    // 플레이어 이동 속도
    public float speed;

	// 플레이어 이동 관련 int 필드
	float hAxis;
	float vAxis;

	// 플레이어 폭탄 관련 int 필드
	float hBomb;
	float vBomb;

	// 플레이어의 이동 관련 좌표
	Vector3 moveVec;

	// 스프라이트 플립을 위한 bool 필드
	bool isFliped;

	// 폭탄 사용 bool필드
	bool bDown;
	// 폭탄 프리팹
	public GameObject Bomb;

	// 액티브 아이템 사용 bool 필드
	bool iDown;
	// 아이템 쿨다운
	public float i_coolDown;
	// 아이템 사용시 쿨다운 시작
	public float maxCool;
	// 현재 가지고 있는 액티브 아이템
	public Alter.Collection activeItem;

	// 최대 총 체력
	public int maxHeart;
	// 현재 총 체력
	public int curHeart;
	// 최대 체력
	public int maxRealHeart;
	// 현재 체력
	public int curRealHeart;
	// 현재 체력
	public int curSoulHeart;

	// 파워
	public float curPower;
	// 발사 속도
	public float curTears;
	// 발사 레인지
	public float fireRange;

	// 피격 중 무적
	bool nowHit;

	// 플레이어 보유 소모 아이템들
	public int Bombs;
	public int Keys;
	public int Coins;

	// 알약 획득 이미지
	public GameObject[] pillsImages;
	// 아이템 획득 이미지
	public GameObject[] itmeImages;

	// 아이템 보유 현황
	public Alter.Collection[] haveItems;

	// 외형 변화 적용을 위한 bool변수
	bool oneUpActive;
	bool brimstoneActive;
	bool LLLActive;

	// 외형 오브젝트
	public GameObject[] itemObj;

	// 스테이지 이동
	bool nextStage;
	// 이동 좌표 줄 collider
	Collider2D nextCol;
	// 스테이지 애니메이션 전용 딜레이
	float d_time = 0;
	// 1up 아이템 발동시 전용 애니메이션 딜레이
	float a_time = 0;

	// 현재 스테이지
	public int nowStage;

	// 플레이어 오브젝트 애니메이터
	Animator p_anim;
	// 플레이어 오브젝트 스프라이트 렌더러
	SpriteRenderer p_renderer;
	// 다양한 모션 출력시 사용할 머리 오브젝트
	public GameObject p_head;
	// 사망 프리팹
	public GameObject p_die;
	// 플레이어 UI 프리팹
	public GameObject p_UI;

	// 투사체 오브젝트
	public GameObject x1Tear;
	public GameObject x2Tear;
	// 혈액 투사체 오브젝트
	public GameObject x1Blood;
	public GameObject x2Blood;

	// Ray 판정을 위한 오프셋 Vec
	Vector3 upOffset;
	Vector3 downOffset;
	Vector3 leftOffset;
	Vector3 rightOffset;

	// 레이캐스트
	RaycastHit2D rayUp;
	RaycastHit2D rayDown;
	RaycastHit2D rayLeft;
	RaycastHit2D rayRight;

	private void Start()
	{
		// 각 컴포넌트 가져오기
		p_anim = GetComponent<Animator>();
		p_renderer = GetComponent<SpriteRenderer>();

		// 좌우 스프라이트 기본값
		p_renderer.flipX = false;

		// 스테이지 통과 애니메이션 기본 딜레이 값
		d_time = 0;
	}

	private void Update()
	{
		GetInput();
		RayShot();
		FlipX();
		Walk();
		UseBomb();
		heartCheck();
		useActiveItem();
		nowActive();
		plusThings();
	}

	// 버튼 입력 감지 함수
	void GetInput()
	{
		if (!nextStage)
		{
			// 이동 관련 버튼 감지
			hAxis = Input.GetAxisRaw("Horizontal");
			vAxis = Input.GetAxisRaw("Vertical");
		}
		
		// 폭탄 사용 버튼 감지
		hBomb = Input.GetAxisRaw("FireHorizontal");
		vBomb = Input.GetAxisRaw("FireVertical");

		// 폭탄 사용
		bDown = Input.GetButtonDown("UseBoom");
		// 액티브 아이템 사용
		iDown = Input.GetButtonDown("UseItem");
	}

	// 레이캐스트 발동 함수
	void RayShot()
	{
		// ray 오프셋
		upOffset = transform.position + Vector3.left * 0.1f + Vector3.up * 0.2f;
		downOffset = transform.position + Vector3.left * 0.1f + Vector3.down * 0.35f;
		leftOffset = transform.position + Vector3.left * 0.2f + Vector3.up * 0.15f;
		rightOffset = transform.position + Vector3.right * 0.2f + Vector3.up * 0.15f;

		// 레이캐스트
		rayUp = Physics2D.Raycast(upOffset, Vector3.right, 0.2f, LayerMask.GetMask("Block"));
		Debug.DrawRay(upOffset, Vector3.right * 0.2f, Color.green);
		rayDown = Physics2D.Raycast(downOffset, Vector3.right, 0.2f, LayerMask.GetMask("Block"));
		Debug.DrawRay(downOffset, Vector3.right * 0.2f, Color.green);
		rayLeft = Physics2D.Raycast(leftOffset, Vector3.down, 0.45f, LayerMask.GetMask("Block"));
		Debug.DrawRay(leftOffset, Vector3.down * 0.45f, Color.green);
		rayRight = Physics2D.Raycast(rightOffset, Vector3.down, 0.45f, LayerMask.GetMask("Block"));
		Debug.DrawRay(rightOffset, Vector3.down * 0.45f, Color.green);
	}

	// 이동 함수
	void Walk()
	{
		// 구덩이를 판별하기 위한 레이캐스트
		RaycastHit2D pitUp = Physics2D.Raycast(upOffset, Vector3.right, 0.2f, LayerMask.GetMask("Pit"));
		RaycastHit2D pitDown = Physics2D.Raycast(downOffset, Vector3.right, 0.2f, LayerMask.GetMask("Pit"));
		RaycastHit2D pitLeft = Physics2D.Raycast(leftOffset, Vector3.down, 0.45f, LayerMask.GetMask("Pit"));
		RaycastHit2D pitRight = Physics2D.Raycast(rightOffset, Vector3.down, 0.45f, LayerMask.GetMask("Pit"));

		// 위쪽 벽, 구덩이에 닿았을 시
		if ((rayUp || pitUp) && vAxis == 1)
		{
			// 벽 가장자리에서 맵 밖으로 나가지 않도록 예외 처리
			if ((rayLeft || pitLeft) && hAxis == -1)
			{
				// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
				moveVec = new Vector3(0, 0, 0).normalized;
			}
			else if ((rayRight || pitRight) && hAxis == 1)
			{
				// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
				moveVec = new Vector3(0, 0, 0).normalized;
			}
			else
			{
				// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
				moveVec = new Vector3(hAxis, 0, 0).normalized;
			}
		}
		// 아래쪽 벽, 구덩이에 닿았을 시
		else if ((rayDown || pitDown) && vAxis == -1)
		{
			// 벽 가장자리에서 맵 밖으로 나가지 않도록 예외 처리
			if ((rayLeft || pitLeft) && hAxis == -1)
			{
				// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
				moveVec = new Vector3(0, 0, 0).normalized;
			}
			else if ((rayRight || pitRight) && hAxis == 1)
			{
				// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
				moveVec = new Vector3(0, 0, 0).normalized;
			}
			else
			{
				// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
				moveVec = new Vector3(hAxis, 0, 0).normalized;
			}
		}
		// 왼쪽 벽, 구덩이에 닿았을 시
		else if ((rayLeft || pitLeft) && hAxis == -1)
		{
			// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
			moveVec = new Vector3(0, vAxis, 0).normalized;
		}
		// 오른쪽 벽, 구덩이에 닿았을 시
		else if ((rayRight || pitRight) && hAxis == 1)
		{
			// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
			moveVec = new Vector3(0, vAxis, 0).normalized;
		}
		// 방해물이 없을 시
		else
		{
			// 이동방향을 Vector3에 입력한 후 항상 동일한 값이 주어지도록 .normalize부여(대각 이동을 구현)
			moveVec = new Vector3(hAxis, vAxis, 0).normalized;
		}

		// 스테이지를 넘어가는 중이 아닐 때
		if (!nextStage)
		{
			// 이동 구현
			transform.position += moveVec * speed * Time.deltaTime;
		}

		// 애니메이션 구현
		// 상하 이동일 때
		if (vAxis != 0)
		{
			// 대각 이동이 아닐 때
			if (hAxis == 0)
			{
				// 위 아래 이동 애니메이션
				p_anim.SetBool("isHorizontal", false);
				p_anim.SetBool("isVertical", true);
			}
			// 대각 이동일 때
			else
			{
				// 좌 우 이동 애니메이션
				p_anim.SetBool("isVertical", false);
				p_anim.SetBool("isHorizontal", true);
			}
		}
		// 좌우 이동일 때
		else if (hAxis != 0)
		{
			// 좌 우 이동 애니메이션
			p_anim.SetBool("isHorizontal", true);
		}
		// Idle 상태일 때
		else
		{
			// 이동 애니메이션 정지
			p_anim.SetBool("isVertical", false);
			p_anim.SetBool("isHorizontal", false);
		}
	}

	// 스프라이트 좌우 플립 함수
	void FlipX()
	{
		// 좌 우 이동 버튼을 눌렀을 때
		if (Input.GetButton("Horizontal"))
		{
			// 왼쪽을 보고 있지 않을 때
			if (!isFliped && hAxis == -1 && hAxis != 0)
			{
				// 스프라이트 플립
				p_renderer.flipX = true;
				isFliped = true;
			}
			// 오른쪽을 보고 있지 않을 때
			else if (isFliped && hAxis == 1 && hAxis != 0)
			{
				// 스프라이트 플립
				p_renderer.flipX = false;
				isFliped = false;
			}
		}
	}

	// 폭탄 사용
	void UseBomb()
	{
		// 폭탄이 있고 사용 버튼을 눌렀을 경우
		if (Bombs > 0 && bDown)
		{
			// 좌측 벽에 닿아있을 시
			if (rayLeft)
			{
				// 폭탄 우측 생성
				GameObject bomb = Instantiate(Bomb, transform.position + Vector3.right * 0.5f, transform.localRotation);
				Bomb bombTrigger = bomb.GetComponent<Bomb>();
				// 폭탄 딜레이 설정
				bombTrigger.setDelay(1.5f);
				// 폭탄 소모
				Bombs--;
			}
			// 우측 벽에 닿아있을 시
			else if (rayRight)
			{
				// 폭탄 좌측 생성
				GameObject bomb = Instantiate(Bomb, transform.position + Vector3.left * 0.5f, transform.localRotation);
				Bomb bombTrigger = bomb.GetComponent<Bomb>();
				// 폭탄 딜레이 설정
				bombTrigger.setDelay(1.5f);
				// 폭탄 소모
				Bombs--;
			}
			// 윗 벽에 닿아있을 시
			else if (rayUp)
			{
				// 폭탄 아래 생성
				GameObject bomb = Instantiate(Bomb, transform.position + Vector3.down * 0.5f, transform.localRotation);
				Bomb bombTrigger = bomb.GetComponent<Bomb>();
				// 폭탄 딜레이 설정
				bombTrigger.setDelay(1.5f);
				// 폭탄 소모
				Bombs--;
			}
			// 아랫 벽에 닿아있을 시
			else if (rayDown)
			{
				// 폭탄 위 생성
				GameObject bomb = Instantiate(Bomb, transform.position + Vector3.up * 0.5f, transform.localRotation);
				Bomb bombTrigger = bomb.GetComponent<Bomb>();
				// 폭탄 딜레이 설정
				bombTrigger.setDelay(1.5f);
				// 폭탄 소모
				Bombs--;
			}
			// 슛 버튼을 누르고 있을 경우
			else if (hBomb != 0 || vBomb != 0)
			{
				// 왼쪽
				if (hBomb < 0 && vBomb == 0)
				{
					// 폭탄 좌측 생성
					GameObject bomb = Instantiate(Bomb, transform.position + Vector3.left * 0.5f, transform.localRotation);
					Bomb bombTrigger = bomb.GetComponent<Bomb>();
					// 폭탄 딜레이 설정
					bombTrigger.setDelay(1.5f);
					// 폭탄 소모
					Bombs--;
				}
				// 오른쪽
				else if (hBomb > 0 && vBomb == 0)
				{
					// 폭탄 우측 생성
					GameObject bomb = Instantiate(Bomb, transform.position + Vector3.right * 0.5f, transform.localRotation);
					Bomb bombTrigger = bomb.GetComponent<Bomb>();
					// 폭탄 딜레이 설정
					bombTrigger.setDelay(1.5f);
					// 폭탄 소모
					Bombs--;
				}
				// 아래쪽
				else if (hBomb == 0 && vBomb < 0)
				{
					// 폭탄 아래 생성
					GameObject bomb = Instantiate(Bomb, transform.position + Vector3.down * 0.5f, transform.localRotation);
					Bomb bombTrigger = bomb.GetComponent<Bomb>();
					// 폭탄 딜레이 설정
					bombTrigger.setDelay(1.5f);
					// 폭탄 소모
					Bombs--;
				}
				// 위쪽
				else if (hBomb == 0 && vBomb > 0)
				{
					// 폭탄 위 생성
					GameObject bomb = Instantiate(Bomb, transform.position + Vector3.up * 0.5f, transform.localRotation);
					Bomb bombTrigger = bomb.GetComponent<Bomb>();
					// 폭탄 딜레이 설정
					bombTrigger.setDelay(1.5f);
					// 폭탄 소모
					Bombs--;
				}
			}
			// 아무버튼도 누르지 않고 벽에도 안닿아 있을 시
			else
			{
				// 폭탄 아래 생성
				GameObject bomb = Instantiate(Bomb, transform.position + Vector3.down * 0.5f, transform.localRotation);
				Bomb bombTrigger = bomb.GetComponent<Bomb>();
				// 폭탄 딜레이 설정
				bombTrigger.setDelay(1.5f);
				// 폭탄 소모
				Bombs--;
			}
		}
	}

	// 체력을 확인하고 예외를 없애는 함수
	void heartCheck()
	{
		// 최대 체력보다 현재체력이 많을 수 없음
		if (maxRealHeart < curRealHeart)
		{
			curRealHeart = maxRealHeart;
		}
	}

	// 현재 가지고 있는 액티브 아이템
	void nowActive()
	{
		// 아이템들을 검사한 후 액티브 아이템 칸에 적용
		if (haveItems[5] != Alter.Collection.None)
		{
			activeItem = Alter.Collection.NineVolt;
		}
	}

	// 액티브 아이템 사용중일 때
	void useActiveItem()
	{
		// 액티브 아이템 쿨타임 감소
		i_coolDown -= Time.deltaTime;

		// 액티브 아이템 쿨타임이 0이하 일 때
		if (i_coolDown < 0 && iDown)
		{
			// 액티브 아이템 종류에 따라
			switch(activeItem)
			{
				// 아이템 발동
				case Alter.Collection.NineVolt:
					useNineVolt();
					break;
			}

			// 액티브 아이템 쿨타임 활성화
			i_coolDown = maxCool;
		}
	}

	// 나인 볼트 아이템 사용
	void useNineVolt()
	{
		// 아이템 사용 중 아이작이 애니메이션 시작
		StartCoroutine(scaleEff());

		// 아이템 획득 애니메이션 출력
		p_anim.SetBool("isGet", true);
		// 머리 비활성화
		p_head.SetActive(false);
		transform.position = transform.position + Vector3.up * 0.1f;

		// 나인 볼트 아이템 사용
		itmeImages[5].SetActive(true);
		itmeImages[5].transform.position = Vector3.Lerp(pillsImages[5].transform.position, pillsImages[5].transform.position + Vector3.up * 0.4f, 1f);

		Invoke("UseItemOut", 1);

		// 작은 눈물 일 때
		if (haveItems[0] == Alter.Collection.TheSadOnion)
		{
			// 전 방위 눈물 발사
			// 12
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + Vector3.up, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
			}

			// 1
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + new Vector3(0.5f, 1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(0.5f, 1, 0) * fireRange, ForceMode2D.Impulse);
			}

			// 3
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + Vector3.right, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
			}

			// 5
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + new Vector3(0.5f, -1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(0.5f, -1, 0) * fireRange, ForceMode2D.Impulse);
			}

			// 6
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + Vector3.down, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
			}

			// 7
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + new Vector3(-0.5f, -1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(-0.5f, -1, 0) * fireRange, ForceMode2D.Impulse);
			}

			// 9
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + Vector3.left, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
			}

			// 11
			{
				// 투사체 발사
				GameObject tear = Instantiate(x2Tear, transform.position + new Vector3(-0.5f, 1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(-0.5f, 1, 0) * fireRange, ForceMode2D.Impulse);
			}
		}
		// 큰 눈물일 때
		else if (haveItems[0] == Alter.Collection.None)
		{
			// 전방위 눈물 발사
			// 12
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + Vector3.up, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
			}

			// 1
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + new Vector3(0.5f, 1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(0.5f, 1, 0) * fireRange, ForceMode2D.Impulse);
			}

			// 3
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + Vector3.right, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
			}

			// 5
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + new Vector3(0.5f, -1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(0.5f, -1, 0) * fireRange, ForceMode2D.Impulse);
			}

			// 6
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + Vector3.down, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
			}

			// 7
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + new Vector3(-0.5f, -1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(-0.5f, -1, 0) * fireRange, ForceMode2D.Impulse);
			}

			// 9
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + Vector3.left, transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
			}

			// 11
			{
				// 투사체 발사
				GameObject tear = Instantiate(x1Tear, transform.position + new Vector3(-0.5f, 1, 0), transform.rotation);
				Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
				rigid.AddForce(new Vector3(-0.5f, 1, 0) * fireRange, ForceMode2D.Impulse);
			}
		}
	}

	// 외형 변화 검사 함수
	void plusThings()
	{
		// 1up 획득 시
		if ((haveItems[4] == Alter.Collection.OneUp) && !oneUpActive)
		{
			// 1번에 한 해 아이템 외형 추가
			oneUpActive = true;
			itemObj[0].SetActive(true);
		}
		// Brimstone 획득 시
		if ((haveItems[7] == Alter.Collection.Brimstone) && !brimstoneActive)
		{
			// 1번에 한 해 아이템 외형 추가
			brimstoneActive = true;

			// 눈물 변경
			x1Tear = x1Blood;
			x2Tear = x2Blood;
		}
		// LessThanThree 획득 시
		if ((haveItems[8] == Alter.Collection.LessThanThree) && !LLLActive)
		{
			// 1번에 한 해 아이템 외형 추가
			LLLActive = true;
			itemObj[1].SetActive(true);
		}
	}

	// 피격 시
	public void Hit(int damage)
	{
		// 피격 중 무적
		if (nowHit) return;

		// 피격음
		int r = Random.Range(0, 2);
		InGameSFXManager.instance.PlayerHit(r);

		// 소울 하트가 존재할 경우
		if (curSoulHeart > 0)
		{
			// 소울 하트 감소
			curSoulHeart -= damage;
			curHeart -= damage;
			// 데미지가 소울하트를 감소시키고 남을 경우
			if (curSoulHeart < 0)
			{
				curHeart -= curSoulHeart;
				// 하트 감소
				curRealHeart += curSoulHeart;
				// 소울 하트 0
				curSoulHeart = 0;
			}
		}
		// 소울 하트가 없을 경우
		else if (curSoulHeart <= 0)
		{
			// 하트 감소
			curRealHeart -= damage;
			if (curRealHeart < 0)
			{
				curRealHeart = 0;
			}
		}

		// 살아있을 때
		if (curRealHeart > 0)
		{
			// 머리 비활성화
			p_head.SetActive(false);

			// 피격 중 심장 비활성화
			if (haveItems[8] != Alter.Collection.None)
			{
				itemObj[1].SetActive(false);
			}
			
			// 히트 애니메이션 출력
			p_anim.SetBool("isHit", true);
			transform.position = transform.position + Vector3.up * 0.3f;
			// 코루틴 시작
			StartCoroutine(hitOut());
		}
		// 사망할 시
		else if (curRealHeart <= 0)
		{
			// 1up 아이템 보유시
			if (itemObj[0].activeInHierarchy && haveItems[4] == Alter.Collection.OneUp)
			{
				// 머리 비활성화
				p_head.SetActive(false);
				// 히트 애니메이션 출력
				p_anim.SetBool("isHit", true);
				transform.position = transform.position + Vector3.up * 0.3f;
				// 코루틴 시작
				StartCoroutine(hitOut());

				// 아이템 소멸
				upAngel();
				// 1회 부활
				curRealHeart = maxRealHeart;
				curHeart = maxRealHeart;
			}
			// 1up 미소유 시
			else
			{
				// 사망 처리
				Dead();
			}
		}
	}

	// 피격 판정 탈출
	IEnumerator hitOut()
	{
		nowHit = true;
		// 점멸 효과
		p_renderer.enabled = true;
		yield return new WaitForSeconds(0.1f);
		p_renderer.enabled = false;
		yield return new WaitForSeconds(0.1f);
		p_renderer.enabled = true;
		yield return new WaitForSeconds(0.1f);
		p_renderer.enabled = false;
		yield return new WaitForSeconds(0.1f);
		// 기본 스프라이트로 변경
		p_renderer.enabled = true;
		transform.position = transform.position + Vector3.down * 0.3f;
		p_anim.SetBool("isHit", false);
		p_head.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		p_renderer.enabled = false;
		p_head.SetActive(false);

		// 히트 종료
		yield return new WaitForSeconds(0.1f);
		p_renderer.enabled = true;
		// 머리 활성화
		p_head.SetActive(true);
		// 심장 재활성화
		if (haveItems[8] != Alter.Collection.None)
		{
			itemObj[1].SetActive(true);
		}
		nowHit = false;
	}

	// 외형 크기 효과
	IEnumerator scaleEff()
	{
		gameObject.transform.localScale = new Vector3(1, 0.75f, 0);
		yield return new WaitForSeconds(0.1f);
		gameObject.transform.localScale = new Vector3(1, 1.5f, 0);
		yield return new WaitForSeconds(0.1f);
		gameObject.transform.localScale = new Vector3(1, 1, 0);
	}

	// 사망시
	public void Dead()
	{
        // 사망 오브젝트 생성p_next
        GameObject Dead = Instantiate(p_die, transform.position, transform.rotation);
        
		// 최상위 오브젝트 컴포넌트 찾기
		TossScene dead = parentObj.GetComponent<TossScene>();

        // 플레이어 오브젝트 삭제
        dead.DestroySelf();
	}

	// 아이템 획득 효과
	IEnumerator getPillEff(Collider2D collision)
	{
		InGameSFXManager.instance.ItemGet();

		// 획득한 알약의 컴포넌트 가져오기
		Pills pill = collision.GetComponent<Pills>();

		// 아이템 목록 검사
		for (int i = 0; i < pillsImages.Length; i++)
		{
			// 알약 유형을 파악하기 위해 관련 컴포넌트 찾기
			PillType type = pillsImages[i].GetComponent<PillType>();

			// 알약 유형 판정
			if (pill.Type == type.pillType)
			{
				// 알약 효과 텍스트
				PlayerUI ui = p_UI.GetComponent<PlayerUI>();

				// 아이템 이미지 on
				ui.onPillsText(i);
				pillsImages[i].SetActive(true);
				yield return new WaitForSeconds(0.1f);
				pillsImages[i].transform.position = transform.position;
				pillsImages[i].transform.position = pillsImages[i].transform.position + Vector3.up * 0.4f;
				yield return new WaitForSeconds(0.8f);
				pillsImages[i].transform.position = pillsImages[i].transform.position + Vector3.down * 0.4f;
				// 아이템 이미지 off
				yield return new WaitForSeconds(0.1f);
				ui.offPillsText(i);
				pillsImages[i].SetActive(false);
			}
		}
	}

	// 아이템 획득 효과
	public IEnumerator getEff(Alter.Collection collection, Collider2D a_col)
	{
		InGameSFXManager.instance.ItemGet();

		// 아이템 목록 검사
		for (int i = 0; i < itmeImages.Length; i++)
		{
			// 획득한 아이템 유형을 판별
			if (collection == haveItems[i])
			{
				// 아이템 효과 텍스트
				PlayerUI ui = p_UI.GetComponent<PlayerUI>();

				// 아이템 이미지 on
				ui.onItemsText(i);
				itmeImages[i].SetActive(true);
				yield return new WaitForSeconds(0.1f);
				itmeImages[i].transform.position = transform.position;
				itmeImages[i].transform.position = itmeImages[i].transform.position + Vector3.up * 0.4f;
				yield return new WaitForSeconds(0.8f);
				itmeImages[i].transform.position = itmeImages[i].transform.position + Vector3.down * 0.4f;
				// 아이템 이미지 off
				yield return new WaitForSeconds(0.1f);
				ui.offItemsText(i);
				itmeImages[i].SetActive(false);

				// 제단 collider 제거하여 중복 획득 예외처리
				a_col.enabled = false;
			}
		}
	}

	// 아이템 획득 시 플레이 할 애니메이션 함수
	public void GetItem()
	{
		StartCoroutine(scaleEff());

		// 심장 스킨 비활성화
		if (haveItems[8] != Alter.Collection.None)
		{
			itemObj[1].SetActive(false);
		}

		// 아이템 획득 애니메이션 출력
		p_anim.SetBool("isGet", true);
		// 머리 비활성화
		p_head.SetActive(false);
		transform.position = transform.position + Vector3.up * 0.1f;

		Invoke("GetItemOut", 1);
	}

	// 알약 획득 시 플레이 할 애니메이션 함수
	public void GetPill(Collider2D collision)
	{
		StartCoroutine(scaleEff());
		StartCoroutine(getPillEff(collision));

		// 아이템 획득 애니메이션 출력
		p_anim.SetBool("isGet", true);
		// 머리 비활성화
		p_head.SetActive(false);
		transform.position = transform.position + Vector3.up * 0.1f;

		Invoke("GetItemOut", 1);
	}

	// 아이템 획득 애니메이션 탈출 함수
	void GetItemOut()
	{
		transform.position = transform.position + Vector3.down * 0.1f;
		// 머리 활성화
		p_head.SetActive(true);

		// 심장 스킨 활성화
		if (haveItems[8] != Alter.Collection.None)
		{
			itemObj[1].SetActive(true);
		}

		// 기본 애니메이션 출력
		p_anim.SetBool("isGet", false);
	}

	// 액티브 아이템 사용 애니메이션 탈출 함수
	void UseItemOut()
	{
		itmeImages[5].transform.position = Vector3.Lerp(itmeImages[5].transform.position, itmeImages[5].transform.position + Vector3.down * 0.4f, 1f);
		// 아이템 이미지 off
		itmeImages[5].SetActive(false);

		transform.position = transform.position + Vector3.down * 0.1f;
		// 머리 활성화
		p_head.SetActive(true);

		// 기본 애니메이션 출력
		p_anim.SetBool("isGet", false);
	}

	// 스테이지 통과 애니메이션 시작
	public void Next(Collider2D collision)
	{
		p_anim.SetBool("goNext", true);
		// 머리 비활성화
		p_head.SetActive(false);
		nextStage = true;

		// 심장 스킨 비활성화
		if (haveItems[8] != Alter.Collection.None)
		{
			itemObj[1].SetActive(false);
		}

		StartCoroutine(NextStage(collision));
	}

	// 스테이지 통과
	IEnumerator NextStage(Collider2D collision)
	{
		// 스테이지 통과 애니메이션
		upMove();

		yield return new WaitForSeconds(0.6f);

		// 스테이지 통과 애니메이션 2
		transform.localScale = new Vector3(0.8f, 1, 1);
		while (transform.position != collision.transform.position)
		{
			transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, 18 * Time.deltaTime);

			yield return new WaitForSeconds(0.01f);
		}

		yield return new WaitForSeconds(0.05f);

		// 스테이지 통과
		switch (nowStage)
		{
			// 현재 1스테이지 일 때
			case 1:
				// BGM 사운드 정지
				InGameSoundManager.instance.StopBGM();

                // 로딩 UI 실행, 2 스테이지로
                LoadinSceneController.Instance.LoadScene("Cave");
                break;
			// 현재 2스테이지 일 때
			case 2:
                // BGM 사운드 정지
                InGameSoundManager.instance.StopBGM();

                TossScene dead = parentObj.GetComponent<TossScene>();

                // 플레이어 오브젝트 삭제
                dead.DestroySelf();

                // 로딩 UI 실행, 메인 메뉴로
                LoadinSceneController.Instance.LoadScene("MainMenu");
                break;
		}

		// 외형 정상화
		transform.localScale = new Vector3(1, 1, 1);

		// 스테이지 이동 중을 판별할 bool 값 변경
		nextStage = false;
		// Idle 애니메이션 출력
		p_anim.SetBool("goNext", false);

		// 머리 활성화
		p_head.SetActive(true);

		// 심장 스킨 활성화
		if (haveItems[8] != Alter.Collection.None)
		{
			itemObj[1].SetActive(true);
		}

		// 스테이지 이동에 사용했던 필드 값 초기화
		d_time = 0;

		// 잠시 플레이어 오브젝트 비활성화
		gameObject.SetActive(false);
	}

	// 스테이지 통과 애니메이션 함수
	void upMove()
	{
		// 트랩도어 위로 이동
		transform.position = Vector3.Lerp(transform.position, nextCol.transform.position + Vector3.up, 18 * Time.deltaTime);
		d_time += Time.deltaTime;

		// 유사 반복문 구현 (재귀함수)
		if (d_time < 0.6f)
		{
			Invoke("upMove", Time.deltaTime);
		}
	}

	// 1up 아이템 사용 후 소멸 애니메이션 함수
	void upAngel()
	{
		itemObj[0].transform.position = Vector3.Lerp(itemObj[0].transform.position, itemObj[0].transform.position + Vector3.up, 15 * Time.deltaTime);
		a_time += Time.deltaTime;

		if (a_time < 2f)
		{
			Invoke("upAngel", Time.deltaTime);
		}
		else if (a_time >= 2f)
		{
			itemObj[0].SetActive(false);
		}
	}

	// 플레이어 삭제 함수
	public void DestroyObj()
	{
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 알약과 닿았을 경우
		if (collision.tag == "Pills")
		{
			// 아이템 획득 애니메이션 재생
			GetPill(collision);
			
		}

		// 아이템 제단과 닿았을 경우
		if (collision.tag == "Item")
		{
			Alter item = collision.GetComponent<Alter>();
			
			// 아이템 검사
			for (int i = 0; i < haveItems.Length; i++)
			{
				// 제단의 아이템 종류를 파악
				if (haveItems[i] == item.collection)
				{
					haveItem ishave = itmeImages[i].GetComponent<haveItem>();

					// 아이템을 가지고 있지 않을 때만 아이템 획득 모션
					if (!ishave.isHave)
					{
						// 아이템 획득 판정
						ishave.nowHave();

						// 아이템 획득 애니메이션 재생
						GetItem();
						//getEff(collision);
					}
				}
			}
		}

		// 다음 스테이지 구멍과 닿았을 경우
		if (collision.tag == "Next")
		{
			// 다음 스테이지 문(트랩도어) collider
			nextCol = collision;

			// 스테이지 통과 애니메이션 재생
			Next(collision);
		}
	}
}
