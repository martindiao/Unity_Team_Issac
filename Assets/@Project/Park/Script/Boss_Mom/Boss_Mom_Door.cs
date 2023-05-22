using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Mom_Door : MonoBehaviour
{
     GameObject _Player;//플레이어 오브젝트
     Player Player_stat; //플레이어 스텟

    public GameObject[] Spawn_Body;//랜덤으로 몬스터를 소환함
    public GameObject Spawn_Hands;//문에 가까이오면 손이 생성되서 공격함


    _Stat stat;
    bool Spawn_Stop; //손이 생성됬을때는 소환을 하지않음
    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

        Player_stat = _Player.GetComponent<Player>();
    stat = GetComponentInParent<_Stat>();

        StartCoroutine(Spawn());
        StartCoroutine(DeathCheck());

    }
    IEnumerator DeathCheck()
    {
        while(true)
        {
            if (stat.Hp <= 0)
            {
                StopAllCoroutines();


                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Spawn_Stop = true;//소환을 멈추고 손을 가져옴
        }


    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if(!Spawn_Stop)
            {
                int Spawn_Choice = Random.Range(0, 10);
                if (Spawn_Choice == 0)
                {
                    Spawn_Body[0].SetActive(true);
                    InGameSFXManager.instance.CampFireOff();
                }
                else if (Spawn_Choice == 1)
                {
                    Spawn_Body[1].SetActive(true);
                    InGameSFXManager.instance.CampFireOff();
                }
                else if (Spawn_Choice == 2)
                {
                    Spawn_Body[2].SetActive(true);
                    InGameSFXManager.instance.CampFireOff();
                }
                else if (Spawn_Choice == 3)
                {
                    Spawn_Body[3].SetActive(true);
                    InGameSFXManager.instance.CampFireOff();
                }
                else
                {
                    //아무것도 소환안됌
                }

                yield return new WaitForSecondsRealtime(7.0f);

                for (int i = 0; i < Spawn_Body.Length; i++)
                {

                    Spawn_Body[i].SetActive(false);
                }
                yield return new WaitForSecondsRealtime(3.0f);
            }

            else
            {
                Spawn_Hands.SetActive(true);

                yield return new WaitForSecondsRealtime(3.0f);
                Spawn_Hands.SetActive(false);
                Spawn_Stop = false;//소환을 멈추고 손을 가져옴

            }
            yield return null;
        }
    }
}
