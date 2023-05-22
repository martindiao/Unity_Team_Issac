using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chubber_Attack_Script : MonoBehaviour
{
    public GameObject _Player;
    public Player _Player_Stat;

    public float Range;
    public float timer;

    public _Stat stat;
    public SpriteRenderer sprite;

    public GameObject Chubber;
    public GameObject reverse;

    public Animator anim;


    public Vector2 AttackSpeed;
    public Vector2 Attack;
    public Vector2 savepoint;

    public Collider2D parentscol;

    public Animator saveAnim;


    public bool colStart = false;
    public bool flip;
    public bool Attack_Succes;

    private void Start()
    {

        _Player = GameObject.FindGameObjectWithTag("Player");
        _Player_Stat = _Player.GetComponent<Player>();

        stat = GetComponentInParent<_Stat>();
        anim = GetComponent<Animator>();


        sprite = GetComponent<SpriteRenderer>();
        if (sprite.flipX == true)
        {
            sprite.flipX = false;

        }
        else
        {

            sprite.flipX = true;
        }
        anim.SetBool("Reverse", false);

        AttackSpeed *= -1;

        gameObject.SetActive(false);


    }

    private void OnEnable()
    {
        Attack = transform.position;
        savepoint = Attack;
        timer = 0;
        colStart = false;
        Attack_Succes = false;
    }


    void Update()
    {

            Attack += AttackSpeed * Time.deltaTime;
            transform.position = Attack;
            timer += Time.deltaTime;

        

        if (timer >= Range)
        {
            if (!colStart)
            {

                if(sprite.flipX == true)
                {
                    sprite.flipX = false;

                }
                else
                {
                    sprite.flipX = true;
                }

                anim.SetBool("Reverse", true);

                AttackSpeed *= -1;
            }

            colStart = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(colStart)
        {
            if(collision == parentscol)
            {
                
                Chubber_Move chubber = collision.GetComponent<Chubber_Move>();
                chubber.Attack = false;


                gameObject.SetActive(false);

                chubber.Head_Anim.SetBool("Attack_Up", false);
                chubber.Head_Anim.SetBool("Attack_Left", false);
                chubber.Head_Anim.SetBool("Attack_Right", false);
                chubber.Head_Anim.SetBool("Attack_Down", false);

            }
        }

        if(!Attack_Succes)
        {
            if (collision.tag == "Player")
            {

                _Player_Stat.Hit(stat.Atk);
                Attack_Succes = true;

                Invoke("AttackFalse", 2.0f);
            }
            else if (collision.tag == "PlayerHead")
            {

                _Player_Stat.Hit(stat.Atk);
                Invoke("AttackFalse", 2.0f);

                Attack_Succes = true;
            }
        }


        if (collision.tag == "PlayerBullet")
        {
            sprite.color = Color.red;


            Damage(_Player_Stat.curPower);

        }
        if (collision.tag == "BombRange")
        {
            sprite.color = Color.red;

            Damage(stat.MaxHp / 2);
        }



    }

    void Damage(float Damage)
    {
        stat.Hp -= Damage;
        Invoke("colorreturn", 0.2f);

    }


    private void colorreturn()
    {
        sprite.color = Color.white;

    }

    private void OnDisable()
    {
        if (sprite.flipX == true)
        {
            sprite.flipX = false;

        }
        else
        {

            sprite.flipX = true;
        }
        anim.SetBool("Reverse", false);

        AttackSpeed *= -1;
    }
}
