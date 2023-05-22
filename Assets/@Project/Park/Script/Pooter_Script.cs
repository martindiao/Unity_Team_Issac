using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pooter_Script : MonoBehaviour
{
     GameObject _Player;//플레이어 오브젝트
    public LayerMask player_layer; //플레이어의 레이어를 가져옴
     Player Player_stat; //플레이어 스텟

    //====================================================
    public GameObject _Death;//죽었을때 생성되는 오브젝트
    public GameObject Spawn_Effect;//몬스터 생성될때 나오는 이펙트
     _Stat stat; //몬스터 스텟
     Rigidbody2D rigid;//몬스터 리지드
    public float moveTimer;//움직임을 몇초에 한번 바꿀지 정한다.

    public GameObject Bullet;//총알 오브젝트
     Bullet_Script Shoot_Bullet;//총알을 발사하기위한 스크립트

     Animator anim;//몬스터 애니메이터
     Vector2 Direction; //몬스터의 이동 방향
    public float _Distance;//식별거리
     RaycastHit2D Find_player; //플레이어를 인식하는 레이캐스트


    public float AttackSpeed;//공격할 때 플레이어에게 이동하는 속도
     Collider2D col;//현 몬스터의 콜라이더를 로드

    public Transform Now_Position;//몬스터의 현재 위치
    private Vector2 _Position;//현재위치를 변환시키기위한 변수


     SpriteRenderer spriteRenderer;//스프라이트 렌더

    public float Attack_Timer;//몬스터의 공격속도

    private bool Attack = false;

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        //플레이어 스텟
        Player_stat = _Player.GetComponent<Player>();

        //몬스터
        stat = GetComponent<_Stat>(); //몬스터 스텟
        col = GetComponent<Collider2D>();//몬스터 콜라이더
        Now_Position = GetComponent<Transform>();//현재 위치
        rigid = GetComponent<Rigidbody2D>();//몬스터의 리지드

        anim = GetComponent<Animator>();//몬스터의 애니메이션 정보

        //총알
        Shoot_Bullet = Bullet.GetComponent<Bullet_Script>();//총알에 담겨진 스크립트를 가져옴

        Instantiate(Spawn_Effect, transform.position, Quaternion.identity);//스폰이펙트 애니메이션 소환

        _Position = transform.position;//현재 포지션
        spriteRenderer = GetComponent<SpriteRenderer>();//몬스터의 스프라이트

        //계속 반복될 코루틴
        StartCoroutine(Monster_Move());//몬스터가 공격할지 안할지 선택하는 코루틴
        StartCoroutine(Shooting_Bullet());//총알을 쏘는 코루틴
        StartCoroutine(Hit_Col());//쳐맞는 코루틴
        
        
        
        
    }

    private void Update()
    {
        //체력이 없으면 죽음
        if (stat.Hp <= 0)
        {
            Instantiate(_Death,transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    //푸터의 움직임을 정함
    private IEnumerator Monster_Move()
    {
        while (true)
        {
            int ChoiceMove = Random.Range(0, 10);//움직임을 정할 랜덤값

            //랜덤값에따라 공격과 랜덤이동이 나뉘어짐
            if (ChoiceMove < 2)
            {
                Attack = false;

                rigid.AddForce(Vector2.zero);

                rigid.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * AttackSpeed, ForceMode2D.Impulse);

            }

            else
            {
                Attack = true;

                //공격상태가 되면 플레이어에게 이동

                if (Attack)
                {
                    rigid.AddForce(Vector2.zero);

                    float r = Random.Range(0, 1f);

                    if (transform.position.x < _Player.transform.position.x)
                    {
                        spriteRenderer.flipX = false;
                        Direction = Vector2.right;


                        rigid.AddForce(Vector2.right * AttackSpeed * r, ForceMode2D.Impulse);
                    }
                    else
                    {
                        spriteRenderer.flipX = true;
                        Direction = Vector2.left;

                        rigid.AddForce(Vector2.left * AttackSpeed * r, ForceMode2D.Impulse);
                    }

                    if (transform.position.y < _Player.transform.position.y)
                    {


                        rigid.AddForce(Vector2.up * AttackSpeed * r, ForceMode2D.Impulse);

                    }
                    else
                    {

                        rigid.AddForce(Vector2.down * AttackSpeed * r, ForceMode2D.Impulse);

                    }

                }
            }
            //움직임을 판단하는 시간.
            yield return new WaitForSecondsRealtime(moveTimer * Random.Range(0, 1.0f));
        }

    }

    
    //총알쏘는 코루틴
    private IEnumerator Shooting_Bullet()
    {
        while (true)
        {

            //플레이어를 인식하는 레이캐스트
            Find_player = Physics2D.Raycast(Now_Position.position, Direction, _Distance, player_layer);
            Debug.DrawRay(Now_Position.position, Direction * _Distance, Color.red);
            int haa = 2;

            //플레이어를 인식했을때
            if (Find_player)
            {
            
                for(int i = 0; i < stat.Bullet_Num; i++)
                {
                    haa *= -1;
                    Shoot_Bullet.damage = stat.Atk;
                    Shoot_Bullet.Speed = new Vector2(Direction.x * 7, haa);

                    Instantiate(Bullet, Now_Position.position, Quaternion.identity);
                }

            anim.Play("Pooter_Attack");

            Debug.DrawRay(Now_Position.position, Direction * _Distance, Color.yellow);

            yield return new WaitForSecondsRealtime(Attack_Timer);


            }
        yield return null;
        }
    }


    private IEnumerator Hit_Col()
    {
        while (true)
        {
            if (col.tag =="PlayerBullet")
            {
                spriteRenderer.color = Color.red;


                yield return new WaitForSecondsRealtime(0.2f);
                spriteRenderer.color = Color.white;

                yield return new WaitForSecondsRealtime(0.2f);//무적타임
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            spriteRenderer.color = Color.red;


            Damage(Player_stat.curPower);

        }
        if ((collision.tag == "Wall"))
        {
            rigid.velocity = Vector2.Reflect(rigid.velocity.normalized, rigid.velocity.normalized);
        }

        if (collision.tag == "BombRange")
        {
            spriteRenderer.color = Color.red;

            Damage(20);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.tag == "Wall"))
        {
            if ((collision.tag == "Wall"))
            {
                rigid.AddForce(_Player.transform.position - transform.position);
            }
        }
    }

    void Damage(float Damage)
    {
        stat.Hp -= Damage;
        Invoke("colorreturn", 0.2f);

    }


    private void colorreturn()
    {

        spriteRenderer.color = Color.white;
    }










}
