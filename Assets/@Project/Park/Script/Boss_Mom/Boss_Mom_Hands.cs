using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Mom_Hands : MonoBehaviour
{
     GameObject _Player;
     Player _player_Stat;

    _Stat stat;

    GameObject _Camera;

    public GameObject _Destroy;

    SpriteRenderer spriteRenderer;//스프라이트 렌더


    int destroyanim_num = 0;//죽었을때 피를 튀기는 액션횟수를 정할 변수
    bool Hit_time;



    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Camera = GameObject.FindGameObjectWithTag("MainCamera");
        _player_Stat = _Player.GetComponent<Player>();

        stat = GetComponentInParent<_Stat>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Hit_time = false;
        StartCoroutine(DeathCheck());

        StartCoroutine(BodySizeSmaller());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Hit_time)
        {

        if (collision.tag == "Player")
        {
            _player_Stat.Hit(1);
        }

        }

        if (collision.tag == "PlayerBullet")
        {
            spriteRenderer.color = Color.red;

            Damage(_player_Stat.curPower);
        }

        if (collision.tag == "BombRange")
        {
            spriteRenderer.color = Color.red;

            Damage(20);
        }
    }
    IEnumerator DeathCheck()
    {
        while (true)
        {
            if (stat.Hp <= 0)
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

                Instantiate(_Destroy, new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f)), Quaternion.identity);

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


    IEnumerator BodySizeSmaller()
    {
        float s = 3.0f;
        while (true)
        {
            if (transform.localScale.x >= 1.0f)
            {
                Hit_time = true;
                transform.localScale = new Vector3(s, s, s);
        
              

                

            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                Hit_time = false;

                yield return new WaitForSecondsRealtime(2.0f);
                StartCoroutine(BodySizeBigger());
                yield break;

            }
            s -= 4 * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator BodySizeBigger()
    {
        float s = 1.0f;

        while (true)
        {
            if (transform.localScale.x <= 3.0f)
            {
                transform.localScale = new Vector3(s, s, s);

            }
            else
            {
                transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

                yield break;
            }
            s += 1 * Time.deltaTime;

            yield return null;
        }
    }


    void Damage(float Damage)
    {
        stat.Hp -= _player_Stat.curPower;
        Invoke("colorreturn", 0.2f);

    }


    private void colorreturn()
    {

        spriteRenderer.color = Color.white;
    }



    private void OnDestroy()
    {
        if (_Camera != null)
        {

            _Camera.transform.eulerAngles = Vector3.zero;
        }


    }
}
