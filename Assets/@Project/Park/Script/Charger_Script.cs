using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger_Script : MonoBehaviour
{
    GameObject _Player;//플레이어 오브젝트
    Player Player_stat; //플레이어 스텟

    public LayerMask _Player_Layer; //플레이어 레이어
    public GameObject Spawn_Effect;//몬스터 생성될때 나오는 이펙트
    public GameObject _Death;//죽었을때 생성되는 오브젝트

     _Stat stat; //몬스터 스텟

     Animator anim;//몬스터 애니메이터
    Vector2 Direction; //몬스터의 이동 방향
    public float _Distance;//식별거리

    public LayerMask _Wall_Layer;//벽 레이어
    public LayerMask _Pit_Layer;//벽 레이어

    public Collider2D col;

    private RaycastHit2D Find_Player;//플레이어를 식별하는 레이캐스트


    private RaycastHit2D Find_Wall;//벽에 부딛혔는지 식벽하는 레이캐스트
    private RaycastHit2D Find_Pit;//돌에 부딛혔는지 식별하는 레이캐스트

     Transform Now_Position;//몬스터의 현재 위치
    private Vector2 _Position;//현재위치를 변환시키기위한 변수


     SpriteRenderer spriteRenderer;//스프라이트 렌더

    private bool Attack = false; //공격상태인지 아닌지 확인하는 변수
    public Vector2 AttackSpeed;//공격할 때 플레이어에게 이동하는 속도

    DropBloodEffect DropBlood;



    //차저는 플레이어를 추척하는것이 아닌 상하좌우를 움직이다가 정면에 플레이어를 발견하면 직선으로 돌진해서 공격한다.

    private void Start()
    {
        stat = GetComponent<_Stat>();
        _Player = GameObject.FindGameObjectWithTag("Player");
        Player_stat = _Player.GetComponent<Player>();

       


        Instantiate(Spawn_Effect,transform.position,Quaternion.identity);

        Now_Position = GetComponent<Transform>();//현재위치 데이터

        _Position = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();//스프라이트 렌더, 좌우 방향을 바꿀때 사용
        anim = GetComponent<Animator>();//몬스터의 애니메이터 정보
        col = GetComponent<Collider2D>();
        DropBlood =gameObject.AddComponent<DropBloodEffect>();



        StartCoroutine(Monster_Move());//몬스터가 공격할지 안할지 선택하는 코루틴
        StartCoroutine(Hit_Col());//총알을 맞았을때 사용될 충돌판정 코루틴
        

    }

    private void Update()
    {
        if(!Attack)
        {
            //플레이어를 찾는 레이캐스트
            Find_Player = Physics2D.Raycast(Now_Position.position, Direction, _Distance, _Player_Layer);
             Debug.DrawRay(Now_Position.position, Direction * _Distance, Color.red);
            if(Find_Player)
            {
                AttackSound();
            }
        }

       //플레이어를 찾는 레이캐스트
        if (Find_Player)
        {
            Attack = Find_Player; //플레이어를 발견하면  Attack상태를 true로 변경\
            Debug.DrawRay(Now_Position.position, Direction* _Distance, Color.yellow);
        }


        //벽에 충돌했는지 확인하는 레이캐스트
        Find_Wall = Physics2D.Raycast(Now_Position.position, Direction, 0.5f, _Wall_Layer);
        Debug.DrawRay(Now_Position.position, Direction * 0.5f, Color.green);

		Find_Pit = Physics2D.Raycast(Now_Position.position, Direction, 0.5f, _Pit_Layer);
        
		Debug.DrawRay(Now_Position.position, Direction * 0.5f, Color.green);

        if (Find_Wall)
        {

            Attack = false; //벽에 충돌하면  Attack상태를 false로 변경
            Direction *= -1;

            if (Direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (Direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            Debug.DrawRay(Now_Position.position, Direction * 0.5f, Color.blue);

        }
        if (Find_Pit)
        {

            Attack = false; //벽에 충돌하면  Attack상태를 false로 변경
            Direction *= -1;

            if (Direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (Direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            Debug.DrawRay(Now_Position.position, Direction * 0.5f, Color.blue);

        }

        //공격상태일때
        if (Attack)
        {

            if (Direction == new Vector2(0, 1))
            {
                anim.SetBool("Up", true);
                anim.SetBool("Down", false);
                anim.SetBool("Attack", true);

                _Position.y += AttackSpeed.y * Time.deltaTime * 5;
                spriteRenderer.flipX = true;



            }
            if (Direction == new Vector2(0, -1))
            {
                anim.SetBool("Down", true);
                anim.SetBool("Up", false);
                anim.SetBool("Attack", true);

                _Position.y -= AttackSpeed.y * Time.deltaTime * 5;
                spriteRenderer.flipX = true;




            }
            if (Direction == new Vector2(1, 0))
            {

                anim.SetBool("Down", false);
                anim.SetBool("Up", false);
                anim.SetBool("Attack", true);

                _Position.x += AttackSpeed.x * Time.deltaTime * 5;
                spriteRenderer.flipX = false;



            }
            if (Direction == new Vector2(-1, 0))
            {

                anim.SetBool("Down", false);
                anim.SetBool("Up", false);
                anim.SetBool("Attack", true);
                _Position.x -= AttackSpeed.x * Time.deltaTime * 5;
                spriteRenderer.flipX = true;


            }

        }

        //공격상태가 아닐때
        else
        {
            anim.SetBool("Attack", false);

            if (Direction ==new Vector2(0,1))
            {
                anim.SetBool("Up", true);
                anim.SetBool("Down", false);
                _Position.y += AttackSpeed.y * Time.deltaTime;


            }
            if (Direction == new Vector2(0, -1))
            {
                anim.SetBool("Down", true);
                anim.SetBool("Up", false);
                _Position.y -= AttackSpeed.y * Time.deltaTime;

            }
            if (Direction == new Vector2(1,0))
            {
                anim.SetBool("Down", false);
                anim.SetBool("Up", false);
                _Position.x += AttackSpeed.x * Time.deltaTime;

            }
            if (Direction == new Vector2(-1,0))
            {
                anim.SetBool("Down", false);
                anim.SetBool("Up", false);
                _Position.x -= AttackSpeed.x * Time.deltaTime;
            }
   

            

        }

        if(stat.Hp <=0)
        {
            Instantiate(_Death, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }    

        Now_Position.position = _Position;
    }

    void AttackSound()
    {
        InGameSFXManager.instance.ChargerAttack();
    }

  
    private IEnumerator Monster_Move()
    {
        while (true)
        {
            int ChoiceMove = UnityEngine.Random.Range(1, 5);//1~4까지로 어디로 움직일지 정한다.

            if (Attack ==false) 
            {
                //랜덤값에따라 공격과 랜덤이동이 나뉘어짐
                if (ChoiceMove == 1)
                {
                    Direction = new Vector2(0, 1);


                    spriteRenderer.flipX = true;
                }
                else if (ChoiceMove == 2)
                {


                    Direction = new Vector2(0, -1);


                    spriteRenderer.flipX = true;
                }
                else if (ChoiceMove == 3)
                {


                    Direction = new Vector2(-1, 0);

                    spriteRenderer.flipX = true;

                }
                else
                {
                    Direction = new Vector2(1, 0);
                    spriteRenderer.flipX = false;
                }
            }



            //움직임을 판단하는 시간.
            yield return new WaitForSecondsRealtime(2.5f);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)//임시
    {
        if (collision.tag == "Player")
        {
            Direction *= -1;
            if (Direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (Direction.x < 0)
            {
                spriteRenderer.flipX = true;

            }

            Player_stat.Hit(stat.Atk);

            Invoke("AttackFalse", 2.0f);
        }
        else if (collision.tag == "PlayerHead")
        {
            Direction *= -1;
            if (Direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (Direction.x < 0)
            {
                spriteRenderer.flipX = true;

            }
            Player_stat.Hit(stat.Atk);
            Invoke("AttackFalse", 2.0f);
        }

        if (collision.tag == "PlayerBullet")
        {
            spriteRenderer.color = Color.red;


            Damage(Player_stat.curPower);

        }
        if (collision.tag == "BombRange")
        {
            spriteRenderer.color = Color.red;

            Damage(20);
        }



    }

    private void AttackFalse()
    {
            Attack = false; //플레이어에 충돌하면  Attack상태를 false로 변경하고 방향전환


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


    private IEnumerator Hit_Col()
    {
        while (true)
        {
            if (col.tag == "PlayerBullet")
            {
                spriteRenderer.color = Color.red;


                yield return new WaitForSecondsRealtime(0.2f);
                spriteRenderer.color = Color.white;

                yield return new WaitForSecondsRealtime(0.2f);//무적타임
            }
            yield return null;
        }
    }
}

