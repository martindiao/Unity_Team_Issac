using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horf_Script : MonoBehaviour
{
     GameObject _Player;
     Player _Player_Stat;
    public LayerMask _Player_Layer;

    public GameObject _Destroy;
    public GameObject _SpawnEffect;

    public GameObject _Bullet;
     Bullet_Script _BulletStat;
     SpriteRenderer _Sprite;

    public float shotSpeed;

     Animator _Anim;
     _Stat _stat;

    private RaycastHit2D Find_Player_Up;//플레이어를 식별하는 레이캐스트
    private RaycastHit2D Find_Player_Down;//플레이어를 식별하는 레이캐스트
    private RaycastHit2D Find_Player_Left;//플레이어를 식별하는 레이캐스트
    private RaycastHit2D Find_Player_Right;//플레이어를 식별하는 레이캐스트

    private Vector2 Direction; //식별 방향
    public float _Distance;//식별거리

    bool Attack = false;


    private void Start()
    {

        _Player = GameObject.FindGameObjectWithTag("Player");
        _Player_Stat = _Player.GetComponent<Player>();

        _Anim = GetComponent<Animator>();
        _stat = GetComponent<_Stat>();

        _BulletStat = _Bullet.GetComponent<Bullet_Script>();
        _Sprite = GetComponent<SpriteRenderer>();

        Instantiate(_SpawnEffect, transform.position, Quaternion.identity);

        StartCoroutine(AttackShoot());
        StartCoroutine(BodyShake());
    }

	private void Update()
	{
        if (_stat.Hp <= 0)
		{
            Instantiate(_Destroy, transform.position, Quaternion.identity);

            Destroy(gameObject);
		}

	}
	IEnumerator AttackShoot()
    {
        while (true)
        {

            if (!Attack)
            {
                //플레이어를 찾는 레이캐스트
                Find_Player_Up = Physics2D.Raycast(transform.position, new Vector2(0, 1), _Distance, _Player_Layer);
                Find_Player_Down = Physics2D.Raycast(transform.position, new Vector2(0, -1), _Distance, _Player_Layer);
                Find_Player_Left = Physics2D.Raycast(transform.position, new Vector2(-1, 0), _Distance, _Player_Layer);
                Find_Player_Right = Physics2D.Raycast(transform.position, new Vector2(1, 0), _Distance, _Player_Layer);



                Debug.DrawRay(transform.position, Vector2.up * _Distance, Color.red);
                Debug.DrawRay(transform.position, Vector2.down * _Distance, Color.red);
                Debug.DrawRay(transform.position, Vector2.left * _Distance, Color.red);
                Debug.DrawRay(transform.position, Vector2.right * _Distance, Color.red);

                //상하좌우 레이캐스트
                if (Find_Player_Up)
                {
                    Attack = Find_Player_Up; //플레이어를 발견하면  Attack상태를 true로 변경\
                    Direction = new Vector2(0, 1);
                    Debug.DrawRay(transform.position, Vector2.up * _Distance, Color.yellow);

                    _Anim.SetBool("Shot", true);
                    yield return new WaitForSecondsRealtime(0.5f);
                    _Anim.SetBool("Shot", false);
                    _BulletStat.damage = _stat.Atk;

                    _BulletStat.Speed = Direction * shotSpeed;
                    Instantiate(_Bullet, transform.position, Quaternion.identity);


                    yield return new WaitForSecondsRealtime(1.0f);

                    Attack = false;


                }
                if (Find_Player_Down)
                {
                    Attack = Find_Player_Down; //플레이어를 발견하면  Attack상태를 true로 변경
                    Direction = new Vector2(0, -1);

                    Debug.DrawRay(transform.position, Vector2.down * _Distance, Color.yellow);

                    _Anim.SetBool("Shot", true);
                    yield return new WaitForSecondsRealtime(0.5f);
                    _Anim.SetBool("Shot", false);

                    _BulletStat.damage = _stat.Atk;

                    _BulletStat.Speed = Direction * shotSpeed;
                    Instantiate(_Bullet, transform.position, Quaternion.identity);

                    yield return new WaitForSecondsRealtime(1.0f);
                    Attack = false;


                }
                if (Find_Player_Left)
                {
                    Attack = Find_Player_Left; //플레이어를 발견하면  Attack상태를 true로 변경
                    Direction = new Vector2(-1, 0);

                    Debug.DrawRay(transform.position, Vector2.left * _Distance, Color.yellow);


                    _Anim.SetBool("Shot", true);
                    yield return new WaitForSecondsRealtime(0.5f);
                    _Anim.SetBool("Shot", false);
                    _BulletStat.damage = _stat.Atk;

                    _BulletStat.Speed = Direction * shotSpeed;
                    Instantiate(_Bullet, transform.position, Quaternion.identity);
                    yield return new WaitForSecondsRealtime(1.0f);
                    Attack = false;

                }

                if (Find_Player_Right)
                {
                    Attack = Find_Player_Right; //플레이어를 발견하면  Attack상태를 true로 변경
                    Direction = new Vector2(1, 0);

                    Debug.DrawRay(transform.position, Vector2.right * _Distance, Color.yellow);

                    _Anim.SetBool("Shot", true);
                    yield return new WaitForSecondsRealtime(0.5f);
                    _Anim.SetBool("Shot", false);

                    _BulletStat.damage = _stat.Atk;

                    _BulletStat.Speed = Direction * shotSpeed;
                    Instantiate(_Bullet, transform.position, Quaternion.identity);

                    yield return new WaitForSecondsRealtime(1.0f);
                    Attack = false;


                }
            }



            else
            {
                yield return new WaitForSecondsRealtime(1.0f);
            }

            yield return null;
        }

    }
    IEnumerator BodyShake()
    {
        float angle = 5;
        float timer = 0;
        while (true)
        {
            if(!Attack)
            { 
            timer += Time.deltaTime;
            angle *= -1;
            Vector3 shake = new Vector3(0, 0, angle);
            transform.eulerAngles = shake;
            }

            else
            {
                transform.eulerAngles = Vector3.zero;
            }
            yield return new WaitForSecondsRealtime(0.1f);


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            _Sprite.color = Color.red;

            Damage(_Player_Stat.curPower);
        }

        if (collision.tag == "BombRange")
        {
            _Sprite.color = Color.red;

            Damage(20);
        }
    }

    void Damage(float Damage)
    {
        _stat.Hp -= Damage;
        Invoke("colorreturn", 0.2f);

    }


    private void colorreturn()
    {

        _Sprite.color = Color.white;
    }


}
