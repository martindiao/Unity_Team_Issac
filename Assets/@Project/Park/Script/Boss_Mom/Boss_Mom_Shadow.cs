using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss_Mom_Shadow : MonoBehaviour
{
    GameObject _Player;
    Player _Player_Stat;

    public GameObject Mom_Foot;

    public Transform shadowSize;
    public SpriteRenderer _Sprite;


    public Transform Now_Position;//몬스터의 현재 위치
    private Vector2 _Position;//현재위치를 변환시키기위한 변수

    public float Shadow_Speed;
    public bool StampingTime;


    _Stat stat;

    bool death;


    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Player_Stat = _Player.GetComponent<Player>();
        _Sprite = GetComponent<SpriteRenderer>();

        _Sprite.color = new Color(0.8f, 0.8f, 0.8f, 0);

        stat = GetComponentInParent<_Stat>();


        Now_Position = GetComponent<Transform>();//현재위치 데이터
        _Position = transform.position;

        StartCoroutine(StopmpingStart());
        StartCoroutine(DeathCheck());

    }

    IEnumerator DeathCheck()
    {
        while(true)
        {
            if (stat.Hp <= 0)
            {
                death = true;
                StopAllCoroutines();
                


                yield break;
            }
            yield return null;
        }
    }

    IEnumerator StopmpingStart()
    {
        while(true)
        {
            if (!StampingTime)
            {
                if (transform.position.x < _Player.transform.position.x)
                {
                    _Position.x += Shadow_Speed * Time.deltaTime;

                }
                else
                {
                    _Position.x -= Shadow_Speed * Time.deltaTime;
                }

                if (transform.position.y < _Player.transform.position.y)
                {
                    _Position.y += Shadow_Speed * Time.deltaTime;


                }
                else
                {
                    _Position.y -= Shadow_Speed * Time.deltaTime;


                }

                Now_Position.position = _Position;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!StampingTime)
        {
            if(!death)
			{

            if (collision.tag ==("Player"))
            {
            StampingTime = true;


                transform.position = _Player.transform.position;
            StartCoroutine(ShadowColor());
            Invoke("StampingStart", 1.0f);
            }
			}
        }
    }


    IEnumerator ShadowColor()
    {
        float a = 0;
        while(true)
        {
            
            if(_Sprite.color.a < 0.8f)
            {
                _Sprite.color = new Color(0.050f, 0.050f, 0.050f, a);

            }
            else
            {

                yield return new WaitForSecondsRealtime(2.5f);
                StartCoroutine(ShadowColorReverse());
                yield break;

            }
            a += 1*Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator ShadowColorReverse()
    {
        float a = 0.8f;
        while (true)
        {
            if (_Sprite.color.a >=0f)
            {
                _Sprite.color = new Color(0.050f, 0.050f, 0.050f, a);
            }
            else
            {
                _Sprite.color = new Color(0.050f, 0.050f, 0.050f, 0);
                yield break;

            }
            a -= 1 * Time.deltaTime;
            yield return null;
        }
    }

    void StampingStart()
    {
        Mom_Foot.SetActive(true);
    }
}
