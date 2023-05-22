using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
     GameObject _Player;//타겟이될 플레이어 오브젝트
     Player PlayerStat; //(임시)플레이어의 스텟
    public LayerMask Player_Layer;//플레이어 레이어

     Collider2D col; //충돌 확인용

    public GameObject DestroyAnim;//총알 충돌시 사라지는 애니메이션 재생

    public int damage;

    // 유사 중력
    public float gravity;
    // 바닥 판정에 사용할 변수
    float tearHigh;

    public Rigidbody2D rigid;
    public Vector2 Speed;//총알이 움직이는 방향

    public float DestroyTime =-2.0f;

    public LayerMask Block;

    bool isFall;

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
      
        if(_Player)
		{
        PlayerStat = _Player.GetComponent<Player>();//플레이어 스텟확인

		}
        
        InGameSFXManager.instance.MonsterTearFire(Random.Range(0, 3));

        rigid.AddForce(Speed,ForceMode2D.Impulse);

        // 최초 중력 가속도
        tearHigh = -0.007f;

    }

    private void Update()
    {
        High();
        onGround();
    }

    void High()
    {
        if (!gameObject.name.Contains("Monstro"))
        {
            // 중력 가속도에 따라 천천히 땅으로 낙하
            rigid.AddForce(Vector3.down * gravity, ForceMode2D.Impulse);
            tearHigh += -0.007f;
        }
        else
        {
            // 중력 가속도에 따라 천천히 땅으로 낙하
            rigid.AddForce(Vector3.down * gravity * 2, ForceMode2D.Impulse);
            tearHigh += -0.007f * 2;
        }
    }

    public void bulletDestroy()
    {
        // 오브젝트 삭제
        Destroy(gameObject);

        // 눈물 폭발? 애니메이션 출력
        GameObject tearpoofa = Instantiate(DestroyAnim, transform.position, Quaternion.identity);
    }

    void onGround()
    {
        if (!gameObject.name.Contains("Monstro"))
        {
            // 중력 가속도가 2가 되었을 때 바닥에 닿았다고 판정
            if (tearHigh < DestroyTime)
            {
                // 오브젝트 삭제
                Destroy(gameObject);

                // 눈물 폭발? 애니메이션 출력
                GameObject tearpoofa = Instantiate(DestroyAnim, transform.position, Quaternion.identity);
            }
        }
        else
        {
            // 중력 가속도가 2가 되었을 때 바닥에 닿았다고 판정
            if (tearHigh < -1f)
            {
                isFall = true;
            }
            if (tearHigh < DestroyTime)
            {
                // 오브젝트 삭제
                Destroy(gameObject);

                // 눈물 폭발? 애니메이션 출력
                GameObject tearpoofa = Instantiate(DestroyAnim, transform.position, Quaternion.identity);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DestroyAnim.transform.localScale = transform.localScale;
            // 오브젝트 삭제
            Destroy(gameObject);

            // 눈물 폭발? 애니메이션 출력
            GameObject tearpoofa = Instantiate(DestroyAnim, transform.position, Quaternion.identity);

            // 몸통 피격시 Hit함수 호출
            PlayerStat.Hit(damage);
        }
        else if (collision.tag == "PlayerHead")
        {
            // 오브젝트 삭제
            Destroy(gameObject);

            // 눈물 폭발? 애니메이션 출력
            GameObject tearpoofa = Instantiate(DestroyAnim, transform.position, Quaternion.identity);

            // 머리 피격시 PlayerHead 내부 코드를 이용해 몸통의 Hit함수 호출
            PlayerStat.Hit(damage);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            if (gameObject.name.Contains("Monstro") && isFall)
            {
				DestroyAnim.transform.localScale = transform.localScale;
				// 오브젝트 삭제
				Destroy(gameObject);

				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(DestroyAnim, transform.position, Quaternion.identity);
			}
            else if (!gameObject.name.Contains("Monstro"))
            {
				DestroyAnim.transform.localScale = transform.localScale;
				// 오브젝트 삭제
				Destroy(gameObject);

				// 눈물 폭발? 애니메이션 출력
				GameObject tearpoofa = Instantiate(DestroyAnim, transform.position, Quaternion.identity);
			}
            
        }
    }
   
     
   


}
