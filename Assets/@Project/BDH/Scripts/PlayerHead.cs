using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
	// 플레이어 이동 관련 int 필드
	public float hAxis;
	public float vAxis;

	// 스프라이트 플립을 위한 bool 필드
	bool isFliped;

	// 공격 감지 버튼
	public float vFire;
	public float hFire;

	// 스테이터스
	// 공격 딜레이
	float fireDelay;
	float maxDelay;
	float headDelay;
	// 사거리
	public float fireRange;

	// 투사체 오브젝트
	public GameObject x1Tear;
	public GameObject x2Tear;
	// 혈액 투사체 오브젝트
	public GameObject x1Blood;
	public GameObject x2Blood;

	// 방향에 따른 얼굴 스프라이트 출력을 위한 스프라이트 배열
	public Sprite[] Heades;

	// 아이템 획득 따른 다른 스프라이트 배열
	public Sprite[] onionHeades;

	// 스프라이트 렌더러
	SpriteRenderer h_renderer;

	// 플레이어 본체
	public GameObject playerObj;
	Player player;

	// 외형 변화용 오브젝트
	public GameObject[] itemObjs;

	// 외형 변화 적용을 위한 bool변수
	bool stigmaActive;
	bool crownActive;
	bool onionActive;
	bool brimstoneActive;

	private void Start()
	{
		// 각 컴포넌트 가져오기
		player = playerObj.GetComponent<Player>();
		h_renderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
		FlipX();
		HeadSide();
		Shot();
		plusFace();
	}

	// 버튼 입력 감지 함수
	void GetInput()
	{
		// 이동 관련 버튼 감지
		hAxis = Input.GetAxisRaw("Horizontal");
		vAxis = Input.GetAxisRaw("Vertical");
		// 공격 관련 버튼 감지
		hFire = Input.GetAxisRaw("FireHorizontal");
		vFire = Input.GetAxisRaw("FireVertical");
	}

	// 이동 시 머리 방향
	void HeadSide()
	{
		if (!Input.GetButton("FireHorizontal") && !Input.GetButton("FireVertical"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				if (vAxis != 0)
				{
					// 대각 이동이 아닐 때
					if (hAxis == 0)
					{
						// 아래쪽 이동
						if (vAxis == -1)
						{
							// 정면 머리
							h_renderer.sprite = Heades[0];
						}
						// 위쪽 이동
						else
						{
							// 후면 머리
							h_renderer.sprite = Heades[2];
						}
					}
					// 대각 이동일 때
					else
					{
						// 옆 머리
						h_renderer.sprite = Heades[1];
					}
				}
				// 좌우 이동일 때
				else if (hAxis != 0)
				{
					// 옆 머리
					h_renderer.sprite = Heades[1];
				}
				// Idle 상태일 때
				else
				{
					// 정면 머리
					h_renderer.sprite = Heades[0];
				}
			}
		}
		else if (Input.GetButton("FireVertical") && !Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 위쪽 공격일 때
				if (vFire == 1)
				{
					// 후면 머리
					h_renderer.sprite = Heades[2];
				}
				// 아래쪽 공격일 때
				else if (vFire == -1)
				{
					// 정면 머리
					h_renderer.sprite = Heades[0];
				}
			}
		}
		else if (!Input.GetButton("FireVertical") && Input.GetButton("FireHorizontal"))
		{
			// 공격 머리 딜레이
			if (headDelay < 0)
			{
				// 좌우 공격일 때
				if (hFire != 0)
				{
					// 옆 머리
					h_renderer.sprite = Heades[1];
				}
			}
		}
	}

	// 스프라이트 좌우 플립 함수
	void FlipX()
	{
		// 공격 시 얼굴 방향 변경 (공격이 가능할 경우)
		if (Input.GetButton("FireHorizontal") && headDelay < 0)
		{
			// 왼쪽을 보고 있지 않을 때 오르쪽을 보면
			if (!isFliped && hFire == -1)
			{
				// 스프라이트 플립
				h_renderer.flipX = true;
				isFliped = true;
			}
			// 오른쪽을 보고 있지 않을 때 왼쪽을 보면
			else if (isFliped && hFire == 1)
			{
				// 스프라이트 플립
				h_renderer.flipX = false;
				isFliped = false;
			}
		}

		// 좌 우 이동 버튼을 눌렀을 때 (공격하고 있지 않을 때)
		if (!Input.GetButton("FireHorizontal") && Input.GetButton("Horizontal"))
		{
			// 왼쪽을 보고 있지 않을 때
			if (!isFliped && hAxis == -1 && hAxis != 0)
			{
				// 스프라이트 플립
				h_renderer.flipX = true;
				isFliped = true;
			}
			// 오른쪽을 보고 있지 않을 때
			else if (isFliped && hAxis == 1 && hAxis != 0)
			{
				// 스프라이트 플립
				h_renderer.flipX = false;
				isFliped = false;
			}
		}
	}

	// 공격 시 머리 방향
	void Shot()
	{
		// 본체에서 슛 딜레이 가져오기
		maxDelay = player.curTears;

		// 공격 딜레이 감소
		fireDelay -= Time.deltaTime;
		// 머리 딜레이 감소
		headDelay -= Time.deltaTime;

		// 공격 딜레이가 0보다 작을 경우 공격 가능
		if (fireDelay < 0)
		{
			// 상 하 공격 중
			if (Input.GetButton("FireVertical"))
			{
				// 공격 방향이 아래일 경우
				if (vFire < 0)
				{
					InGameSFXManager.instance.PlayerTearFire();

					// 아래 공격 스프라이트
					h_renderer.sprite = Heades[3];
					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;

					// 큰 눈물 아이템 소지시
					if (player.haveItems[0] == Alter.Collection.TheSadOnion)
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear1 = Instantiate(x2Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid1 = tear1.GetComponent<Rigidbody2D>();
							rigid1.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x2Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
							rigid2.AddForce(Vector3.left * 0.5f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x2Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.right * 0.5f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x2Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
						}
					}
					// 작은 눈물일 때
					else
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x1Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
							rigid2.AddForce(Vector3.left * 0.5f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x1Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.right * 0.5f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.down, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.down * fireRange, ForceMode2D.Impulse);
						}
					}
				}
				// 공격 방향이 위일 경우
				else
				{
					InGameSFXManager.instance.PlayerTearFire();

					// 위 공격 스프라이트
					h_renderer.sprite = Heades[5];
					// 스프라이트 유지 딜레이
					headDelay = 0.1f;
					// 공격 딜레이 초기화
					fireDelay = maxDelay;

					// 큰 눈물 아이템 소지시
					if (player.haveItems[0] == Alter.Collection.TheSadOnion)
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear = Instantiate(x2Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x2Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
							rigid2.AddForce(Vector3.left * 0.5f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x2Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.right * 0.5f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x2Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
						}
					}
					// 작은 눈물일 때
					else
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x1Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
							rigid2.AddForce(Vector3.left * 0.5f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x1Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.right * 0.5f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.up, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.up * fireRange, ForceMode2D.Impulse);
						}
					}
				}
			}
			// 공격 방향이 좌우 일 경우
			else if (Input.GetButton("FireHorizontal"))
			{
				InGameSFXManager.instance.PlayerTearFire();

				// 측면 공격 스프라이트
				h_renderer.sprite = Heades[4];
				// 스프라이트 유지 딜레이
				headDelay = 0.1f;
				// 공격 딜레이 초기화
				fireDelay = maxDelay;
				// 공격 방향이 왼쪽일 경우
				if (hFire < 0)
				{
					// 큰 눈물 아이템 소지시
					if (player.haveItems[0] == Alter.Collection.TheSadOnion)
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear = Instantiate(x2Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
							rigid.AddForce(Vector3.up * 0.3f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x2Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x2Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.down * 0.3f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x2Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
						}

					}
					// 작은 눈물일 때
					else
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
							rigid.AddForce(Vector3.up * 0.3f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x1Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x1Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.down * 0.3f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.left, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.left * fireRange, ForceMode2D.Impulse);
						}
					}
				}
				// 공격 방향이 오른쪽
				else
				{
					InGameSFXManager.instance.PlayerTearFire();

					// 큰 눈물 아이템 소지시
					if (player.haveItems[0] == Alter.Collection.TheSadOnion)
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear = Instantiate(x2Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
							rigid.AddForce(Vector3.up * 0.3f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x2Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x2Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.down * 0.3f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x2Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
						}
					}
					// 작은 눈물일 때
					else
					{
						// 세 개 눈물 발사 아이템 소지시
						if (player.haveItems[3] == Alter.Collection.Stigmata)
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
							rigid.AddForce(Vector3.up * 0.3f * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear2 = Instantiate(x1Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid2 = tear2.GetComponent<Rigidbody2D>();
							rigid2.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);

							// 투사체 발사
							GameObject tear3 = Instantiate(x1Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid3 = tear3.GetComponent<Rigidbody2D>();
							rigid3.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
							rigid3.AddForce(Vector3.down * 0.3f * fireRange, ForceMode2D.Impulse);
						}
						// 눈물이 하나 일 때
						else
						{
							// 투사체 발사
							GameObject tear = Instantiate(x1Tear, transform.position + Vector3.right, transform.rotation);
							Rigidbody2D rigid = tear.GetComponent<Rigidbody2D>();
							rigid.AddForce(Vector3.right * fireRange, ForceMode2D.Impulse);
						}
					}
				}
			}
		}
	}

	// 머리 히트시 히트 판정 함수
	public void Hit(int damage)
	{
		player.Hit(damage);
	}

	// 외형 변화 함수
	void plusFace()
	{
		// Stigmata 획득 시
		if ((player.haveItems[3] == Alter.Collection.Stigmata) && !stigmaActive)
		{
			// 1회에 한 해 외형 변경
			stigmaActive = true;
			itemObjs[0].SetActive(true);
		}
		// BloodOfTheMartyr 획득 시
		if ((player.haveItems[1] == Alter.Collection.BloodOfTheMartyr) && !crownActive)
		{
			// 1회에 한 해 외형 변경
			crownActive = true;
			itemObjs[1].SetActive(true);
		}
		// TheSadOnion 획득 시
		if ((player.haveItems[0] == Alter.Collection.TheSadOnion) && !onionActive)
		{
			// 1회에 한 해 외형 변경
			onionActive = true;
			for (int i = 0; i < 6; i++)
			{
				Heades[i] = onionHeades[i];
			}
		}
		// Brimstone 획득 시
		if ((player.haveItems[7] == Alter.Collection.Brimstone) && !brimstoneActive)
		{
			// 1회에 한 해 외형 변경
			brimstoneActive = true;
			itemObjs[2].SetActive(true);

			// 1회에 한 해 피눈물로 눈물 변경
			x1Tear = x1Blood;
			x2Tear = x2Blood;
		}
	}
}
