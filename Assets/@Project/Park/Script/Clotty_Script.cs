using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clotty_Script : MonoBehaviour
{
     GameObject _Player;
     Player _Player_Stat;
    public LayerMask _Player_Layer;

    public GameObject _Death;//죽으면 생성되는 오브젝트
    public GameObject Spawn_Effect;//몬스터 생성될때 나오는 이펙트

    public GameObject Bullet;//총알 프리팹
    public Bullet_Script Bullet_;//총알 스크립트
     Animator anim;//애니메이터
     _Stat stat;//몬스터 스탯
     SpriteRenderer sprite; //몬스터의 스프라이트 랜더

    //사운드 관리
    public AudioSource playSound;//오디오를 재생
    public AudioClip[] Sound;//재생시킬 소리

    public Transform Now_Position;//몬스터의 현재 위치
    private Vector2 _Position;//현재위치를 변환시키기위한 변수
    public float move_Speed;//이동속도
     Rigidbody2D rigid;//몬스터 물리

     Collider2D col;//현 몬스터의 콜라이더를 로드

    public float move_Timer;//몬스터 이동방향을 결정하는 속도(애니메이션에 맞춘다.)

    DropBloodEffect_Monster DropBlood;
    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Player_Stat = _Player.GetComponent<Player>();

        rigid= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        stat = GetComponent<_Stat>();
        Now_Position = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        Bullet_ = Bullet.GetComponent<Bullet_Script>();

        DropBlood = GetComponent<DropBloodEffect_Monster>();


        move_Timer = anim.GetCurrentAnimatorStateInfo(0).length;

        Instantiate(Spawn_Effect, transform.position, Quaternion.identity);

        StartCoroutine(move_Coroutine());
        StartCoroutine(Hit_Col());
        
    }

    private IEnumerator move_Coroutine()
    {
        while (true)
        {
 
            anim.SetBool("Move", true);
            rigid.AddForce(new Vector2(Random.Range(-1,1) * move_Speed, Random.Range(-1, 1) * move_Speed));
          

            yield return new WaitForSecondsRealtime(move_Timer);
            anim.SetBool("Move", false);

            Shoot_Bullet();

            yield return new WaitForSeconds(3.0f);

        }
    }

    private void Update()
    {
        if (transform.position.x < _Player.transform.position.x)
        {
            sprite.flipX = false;



        }
        else
        {
            sprite.flipX = true;

        }

        if (stat.Hp <=0)
        {
            Instantiate(_Death, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void Shoot_Bullet()
    {
        for(int i=0; i<stat.Bullet_Num; i++)
        {
            //anim.SetTrigger("Attack"); //이상해서 뺌
            Bullet_.damage = stat.Atk;
            if (i==0)
            {
                 Bullet_.Speed = new Vector2(1, 0)*10;
            }
            else if (i == 1)
            {
                Bullet_.Speed = new Vector2(0, -1) * 10;
            }
            else if (i == 2)
            {
                Bullet_.Speed = new Vector2(-1, 0) * 10;
            }
            if (i == 3)
            {
                Bullet_.Speed = new Vector2(0, 1) * 10;
            }
            Instantiate(Bullet, transform.position, Quaternion.identity);
        }

    }
    

    private IEnumerator Hit_Col()
    {
        while(true)
        {
            if (col.tag == "PlayerBullet")
            {
                 sprite.color = Color.red;


                yield return new WaitForSecondsRealtime(0.2f);
                sprite.color = Color.white;

                yield return new WaitForSecondsRealtime(0.2f);//무적타임
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//임시
    {


        if (collision.tag == "PlayerBullet")
        {
            sprite.color = Color.red;



            Damage(_Player_Stat.curPower);

        }
        if ((collision.tag == "Wall"))
        {
            rigid.velocity = Vector2.Reflect(rigid.velocity.normalized, rigid.velocity.normalized);
        }

        if ((collision.tag == "Rock"))
        {
            rigid.velocity = Vector2.Reflect(rigid.velocity.normalized, rigid.velocity.normalized);
        }
        if (collision.tag == "BombRange")
        {
            sprite.color = Color.red;

            
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

        DropBlood.DropEffect();

        sprite.color = Color.white;
    }


}
