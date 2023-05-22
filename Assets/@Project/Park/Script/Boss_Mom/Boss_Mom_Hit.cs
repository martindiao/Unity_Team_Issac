using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Mom_Hit : MonoBehaviour
{
     GameObject _Player;
     Player _Player_Stat;

     GameObject _Camera;

      SpriteRenderer spriteRenderer;//스프라이트 렌더
    public GameObject[] DestroyMonster; //죽었을때 필드에 있는 몬스터들을 전부 죽이기 위한 오브젝트

     _Stat stat;
    _Stat monstat;
    int destroyanim_num = 0;//죽었을때 피를 튀기는 액션횟수를 정할 변수

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Camera = GameObject.FindGameObjectWithTag("MainCamera");

        stat = GetComponentInParent<_Stat>();
        _Player_Stat = _Player.GetComponent<Player>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(DeathCheck());
    }

    private void Update()
    {
        DestroyMonster = GameObject.FindGameObjectsWithTag("Enemy");
        
    }


    //엄마의 체력을 상시체크하는 코루틴.
    //체력이 다되면 모든 코루틴을 정지하고 피가 터지는 연출의 코루틴을 재생
    IEnumerator DeathCheck()
    {
        while(true)
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
            if (destroyanim_num == 20)
            {

                Destroy(gameObject);
            }


            destroyanim_num++;



            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 0.3f));

		}
	}

	private IEnumerator CameraShake()
	{
        while(true)
		{
            float angle = 2;
            float timer = 0;
            while (true)
            {

                timer += Time.deltaTime;
                angle *= -1;
                Vector3 shake = new Vector3(0, angle, 0);
                _Camera.transform.eulerAngles = shake;

                if (timer >= 1)
                {
                    _Camera.transform.eulerAngles = Vector3.zero;

                    yield break;
                }
                yield return null;


            }
        }
        
            
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="PlayerBullet")
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
        stat.Hp -= _Player_Stat.curPower;
        Invoke("colorreturn", 0.2f);

    }


    private void colorreturn()
    {

        spriteRenderer.color = Color.white;
    }



    private void OnDestroy()
    {
        if(_Camera != null)
        {

        _Camera.transform.eulerAngles = Vector3.zero;
        }
        if (DestroyMonster != null)
        {

            for (int i = 0; i < DestroyMonster.Length; i++)
            {
                if (DestroyMonster[i].GetComponent<_Stat>() != null)
                {
                    Debug.Log(DestroyMonster.Length);
                    Debug.Log(i);
                    Debug.Log(DestroyMonster[i].name);

                    if(DestroyMonster[i].name.Contains("Globin"))
					{
                        Destroy(DestroyMonster[i]);
					}
                    else
					{

                        monstat = DestroyMonster[i].GetComponent<_Stat>();
                        monstat.Hp = 0;
                    }

                }

            }
        }



    }

}
