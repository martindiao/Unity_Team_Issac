using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chubber_Move : MonoBehaviour
{
    public GameObject _Player;
    public Player _Player_Stat;
    public LayerMask _Player_Layer;

    public GameObject Headobj;
    public Rigidbody2D _Headrigid;
    public SpriteRenderer HeadSprite;
    public SpriteRenderer Sprite;

    public Animator _Anim;
    public Animator Head_Anim;

    public GameObject _Destroy;
    public GameObject _Spawn;


    public DropBloodEffect_Monster Dropblood;
    public _Stat stat;

    public Chubber_Script ChubberScr;
    public Rigidbody2D rigid;

    public GameObject Attack_Up;
    public GameObject Attack_Down;
    public GameObject Attack_Left;
    public GameObject Attack_Right;

    private RaycastHit2D Find_Player_Up;//플레이어를 식별하는 레이캐스트
    private RaycastHit2D Find_Player_Down;//플레이어를 식별하는 레이캐스트
    private RaycastHit2D Find_Player_Left;//플레이어를 식별하는 레이캐스트
    private RaycastHit2D Find_Player_Right;//플레이어를 식별하는 레이캐스트

    private Vector2 Direction; //몬스터의 이동 방향
    public float _Distance;//식별거리

    public float AttackSpeed;
    public bool Attack = false;

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Player_Stat = _Player.GetComponent<Player>();

        stat = GetComponent<_Stat>();
        Sprite = GetComponent<SpriteRenderer>();

        _Anim = GetComponent<Animator>();

        HeadSprite = Headobj.GetComponent<SpriteRenderer>();
        ChubberScr = Headobj.GetComponent<Chubber_Script>();
        _Headrigid = Headobj.GetComponent<Rigidbody2D>();
        Head_Anim = Headobj.GetComponent<Animator>();




    Dropblood =GetComponent<DropBloodEffect_Monster>();

        rigid = GetComponent<Rigidbody2D>();

        Instantiate(_Spawn, transform.position, Quaternion.identity);

        StartCoroutine(Move());
        StartCoroutine(AttackShoot());
        StartCoroutine(MoveStop());
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
                Head_Anim.SetBool("Attack_Up", true);


                yield return new WaitForSecondsRealtime(1.0f);
                Attack_Up.SetActive(true);

                    InGameSFXManager.instance.ChubberAttack();
            }
            if (Find_Player_Down)
            {
                Attack = Find_Player_Down; //플레이어를 발견하면  Attack상태를 true로 변경
                Direction = new Vector2(0, -1);

                Debug.DrawRay(transform.position, Vector2.down * _Distance, Color.yellow);
                Head_Anim.SetBool("Attack_Down", true);

                    yield return new WaitForSecondsRealtime(1.0f);

                    Attack_Down.SetActive(true);

                    InGameSFXManager.instance.ChubberAttack();
                }
            if (Find_Player_Left)
            {
                Attack = Find_Player_Left; //플레이어를 발견하면  Attack상태를 true로 변경
                Direction = new Vector2(-1, 0);

                Debug.DrawRay(transform.position, Vector2.left * _Distance, Color.yellow);
                Head_Anim.SetBool("Attack_Right", true);

                    Sprite.flipX = true;
                    HeadSprite.flipX = true;

                    yield return new WaitForSecondsRealtime(1.0f);

                    Attack_Left.SetActive(true);

                    InGameSFXManager.instance.ChubberAttack();
                }

            if (Find_Player_Right)
            {
                Attack = Find_Player_Right; //플레이어를 발견하면  Attack상태를 true로 변경
                Direction = new Vector2(1, 0);

                Debug.DrawRay(transform.position, Vector2.right * _Distance, Color.yellow);
                Head_Anim.SetBool("Attack_Left", true);

                    Sprite.flipX = false;
                    HeadSprite.flipX = false;

                    yield return new WaitForSecondsRealtime(1.0f);

                    Attack_Right.SetActive(true);

                    InGameSFXManager.instance.ChubberAttack();
                }
        }



        else
            {
                yield return new WaitForSecondsRealtime(1.0f);
            }

        yield return null;
        }

    }

    IEnumerator MoveStop()
    {
        while (true)
        {
            if (Attack)
            {
                _Headrigid.velocity = Vector2.zero;
                rigid.velocity = Vector2.zero;
            }
            yield return null;
        }
    }



    void Update()
    {

        _Anim.SetFloat("Horizontal", rigid.velocity.x);
        _Anim.SetFloat("Vertical", rigid.velocity.y);
        _Anim.SetBool("UpDown", rigid.velocity.x < rigid.velocity.y);

        if(stat.Hp <=0)
        {
            Instantiate(_Destroy,transform.position,Quaternion.identity);

            Destroy(gameObject);
        }

    }

    IEnumerator Move()
    {
        while (true)
        {
            if (!Attack)
            {
                int r = Random.Range(1, 4);


                  _Headrigid.velocity = Vector2.zero;
                  rigid.velocity = Vector2.zero;

         
                if (r == 1)
                {
                    Sprite.flipX = false;
                    HeadSprite.flipX = false;
                    _Anim.Play("Move_Right");

                    _Headrigid.AddForce(Vector2.right * AttackSpeed * r, ForceMode2D.Impulse);
                    rigid.AddForce(Vector2.right * AttackSpeed * r, ForceMode2D.Impulse);
                }
                else if (r == 2)
                {
                    Sprite.flipX = true;
                    HeadSprite.flipX = true;
                    _Anim.Play("Move_Left");


                    _Headrigid.AddForce(Vector2.left * AttackSpeed * r, ForceMode2D.Impulse);
                    rigid.AddForce(Vector2.left * AttackSpeed * r, ForceMode2D.Impulse);
                }

                else if (r == 3)
                {
                    _Anim.Play("Move_up");

                    _Headrigid.AddForce(Vector2.up * AttackSpeed * r, ForceMode2D.Impulse);
                    rigid.AddForce(Vector2.up * AttackSpeed * r, ForceMode2D.Impulse);


                }
                else if (r == 4)
                {
                    _Anim.Play("Move_Down");

                    _Headrigid.AddForce(Vector2.down * AttackSpeed * r, ForceMode2D.Impulse);
                    rigid.AddForce(Vector2.down * AttackSpeed * r, ForceMode2D.Impulse);

                }
                yield return new WaitForSecondsRealtime(3.0f);

            }


            yield return null;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)//임시
    {
        if (collision.tag == "Player")
        {
            _Player_Stat.Hit(stat.Atk);
        }
        else if (collision.tag == "PlayerHead")
        {
            _Player_Stat.Hit(stat.Atk);
        }

        if (collision.tag == "PlayerBullet")
        {
            Sprite.color = Color.red;
            HeadSprite.color = Color.red;


            Damage(_Player_Stat.curPower);

        }
        if ((collision.tag == "Wall"))
        {

            _Headrigid.velocity = Vector2.Reflect(rigid.velocity.normalized, rigid.velocity.normalized);
            rigid.velocity = Vector2.Reflect(rigid.velocity.normalized, rigid.velocity.normalized);
        }

        if (collision.tag == "BombRange")
        {
            Sprite.color = Color.red;
            HeadSprite.color = Color.red;
            Damage(20);
        }


    }



    void Damage(float Damage)
    {
        stat.Hp -= Damage;
        Invoke("colorreturn", 0.2f);

    }


    private void colorreturn()
    {
        Dropblood.DropEffect();

        Sprite.color = Color.white;
        HeadSprite.color = Color.white;
    }




}
