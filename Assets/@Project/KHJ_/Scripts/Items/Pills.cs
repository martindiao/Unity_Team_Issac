using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 알약 효과
// 최대한 많은 효과를 넣어보려 했으나
// 알약 스프라이트가 9장 뿐이고
// 난수를 넣는 것보다 효과를 고정시키는게
// 안전할 것 같다고 판단
public class Pills : MonoBehaviour
{
    // 알약 효과의 종류
    public enum PillType
    {
        HealthUp,       // max체력 1칸 증가
        HealthDown,     // max체력 1칸 감소
        PowerUp,        // 사거리 증가
        PowerDown,      // 사거리 감소
        SpeedUp,        // 이동속도 증가
        SpeedDown,      // 이동속도 감소
        TearsUp,        // 공격속도 증가
        TearsDown,      // 공격속도 감소
        BombsAreKey,    // 폭탄과 키의 수치 변환
    }

    public PillType Type;

    public GameObject playerObj;
    Player player;

	// 각 설정용 변수
	int MaxHp;
    float Power;
    float Speed;
    float Tears;
    // 시작 시 타입에 따라 알약의 효과 반영
    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");

		// 플레이어 데이터 가져오기
		player = playerObj.GetComponent<Player>();

		MaxHp = 0;
        Power = 0;
        Speed = 0;
        Tears = 0;

        switch (Type)
        {
            case PillType.HealthUp:
                MaxHp = 2;
                break;
            case PillType.HealthDown:
                MaxHp = -2;
                break;
            case PillType.PowerUp:
                Power = 0.2f;
                break;
            case PillType.PowerDown:
                Power = -0.2f;
                break;
            case PillType.SpeedUp:
                Speed = 0.15f;
                break;
            case PillType.SpeedDown:
                Speed = -0.15f;
                break;
            case PillType.TearsUp:
                Tears = 0.1f;
                break;
            case PillType.TearsDown:
                Tears = -0.1f;
                break;
        }
    }

    // 충돌검사
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 충돌 시 
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerHead"))
        {
            // 설정값 반영
            if (player.maxRealHeart + MaxHp > 2 && player.maxHeart + MaxHp < 20)
            {
                player.curHeart += MaxHp;
                player.curRealHeart += MaxHp;
                player.maxRealHeart += MaxHp;
            }
            if (player.curPower + Power > 0.2f && player.curPower + Power < 3f) player.curPower += Power;
            if (player.speed + Speed > 3f && player.speed + Speed < 9f) player.speed += Speed;
            if (player.curTears + Tears > 0.4f && player.curTears + Tears < 2f) player.curTears += Tears;

            // 타입이 교환일 경우 폭탄수와 키의 수를 교환
            if (Type == PillType.BombsAreKey)
            {
                int tmp = player.Bombs;

                player.Bombs = player.Keys;
                player.Keys = tmp;
            }

            // 해당 오브젝트 파괴
            Destroy(this.gameObject);
        }
    }
}
