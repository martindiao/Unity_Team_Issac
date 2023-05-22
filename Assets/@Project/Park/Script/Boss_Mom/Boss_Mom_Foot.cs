using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Boss_Mom_Foot : MonoBehaviour
{
     GameObject _Player;
     Player _Player_Stat;
    GameObject _Camera;

    public GameObject _Destroy;

     SpriteRenderer spriteRenderer;//스프라이트 렌더
    _Stat _stat;

    Vector2 _Position_Down;
    Vector2 _Postion_Up;

    public GameObject Shadow;
    public float Stamping_Speed;
    float savespeed;

    bool StamEnd;

    int destroyanim_num = 0;//죽었을때 피를 튀기는 액션횟수를 정할 변수


    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Player_Stat = _Player.GetComponent<Player>();

        _Camera = GameObject.FindGameObjectWithTag("MainCamera");

        _stat = GetComponentInParent<_Stat>();

        spriteRenderer = GetComponent<SpriteRenderer>();


 
        gameObject.SetActive(false);
    }
    IEnumerator DeathCheck()
    {
        while (true)
        {
 
            if (_stat.Hp <= 0)
            {
                StopAllCoroutines();

                StartCoroutine(DeathAnim());

                yield break;
            }

            yield return null;
        }
    }

    IEnumerator DeathAnim()
    {
        while (true)
        {
            if (spriteRenderer != null)
            {

                Instantiate(_Destroy, new Vector2(Random.Range(transform.position.x - 1.5f, transform.position.x + 1.5f), Random.Range(transform.position.y - 2f, transform.position.y + 2f)), Quaternion.identity);

            }

            if (destroyanim_num == 20)
            {
                if (spriteRenderer != null)
                {
                    _Destroy.transform.localScale = new Vector3(3, 3, 3);
                    Instantiate(_Destroy, transform.position, Quaternion.identity);
                    _Destroy.transform.localScale = new Vector3(1, 1, 1);
                }

            }


            destroyanim_num++;



            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 0.3f));

        }
    }


    // Update is called once per frame
    private void OnEnable()
    {
        _Position_Down = new Vector2(transform.position.x, transform.position.y - 10f);
        _Postion_Up = new Vector2(transform.position.x, transform.position.y);

        StartCoroutine(DeathCheck());

        savespeed = Stamping_Speed;
        StartCoroutine(Stamping());
    }
    IEnumerator Stamping()
    {
        while (true)
        {
             if ( transform.position.y >= _Position_Down.y)
             {
                transform.position = new Vector2(transform.position.x,transform.position.y - Stamping_Speed * Time.deltaTime);
                Stamping_Speed += 150f*Time.deltaTime;
             }

             else
            {
                transform.position = _Position_Down;
                InGameSFXManager.instance.BoosStomp();
                StartCoroutine(CameraShake());
                yield return new WaitForSecondsRealtime(1.0f);

                StamEnd = false;
                Stamping_Speed = savespeed;
                yield return new WaitForSecondsRealtime(1.0f);

                StartCoroutine(StampingEnd());
                yield break;
            }    
     

            
            yield return null;
        }

    }
    IEnumerator StampingEnd()
    {
        while (true)
        {
            if (transform.position.y <= _Postion_Up.y)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + Stamping_Speed * Time.deltaTime);
            }
            else
            {
                transform.position = _Postion_Up;
                gameObject.SetActive(false);
  

                yield break;
            }

            yield return null;
        }
    }
    IEnumerator CameraShake()
    {
        float angle =1;
        float timer = 0;
        while (true)
        {
            
            timer += Time.deltaTime;
            angle *= -1;  
            Vector3 shake = new Vector3(0, 0, angle);
            _Camera.transform.eulerAngles = shake;

            if(timer >=1)
            {
                _Camera.transform.eulerAngles = Vector3.zero;

                yield break;
            }
            yield return null;


        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (StamEnd)
        {
            if (collision.tag == "Player")
            {
                _Player_Stat.Hit(_stat.Atk * 2);
            }
        }

        if (collision.tag == "PlayerBullet")
        {
            spriteRenderer.color = Color.red;

            Damage(_Player_Stat.curPower);
        }

        if (collision.tag == "BombRange")
        {
            spriteRenderer.color = Color.red;

            Damage(20);
        }
    }

    void Damage(float Damage)
    {
        _stat.Hp -= _Player_Stat.curPower;
        Invoke("colorreturn", 0.2f);

    }


    private void colorreturn()
    {

        spriteRenderer.color = Color.white;
    }



    private void OnDisable()
    {
        StamEnd=true;
        Boss_Mom_Shadow boss_Mom_Shadow = GetComponentInParent<Boss_Mom_Shadow>();
        boss_Mom_Shadow.StampingTime = false;

        if (_Camera != null)
        {

            _Camera.transform.eulerAngles = Vector3.zero;
        }
    }




}
