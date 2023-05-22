using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Monstro_Hit : MonoBehaviour
{
    public GameObject _Player;
    public Player _Player_Stat;

    public GameObject _Camera;

    public GameObject _Destroy;

    public SpriteRenderer spriteRenderer;//스프라이트 렌더

    public _Stat stat;

    GameObject map;
    NextStageDoor nextDoor;

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Camera = GameObject.FindGameObjectWithTag("MainCamera");

        _Player_Stat = _Player.GetComponent<Player>();

        stat = GetComponentInParent<_Stat>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        map = FindObjectOfType<GameManager>().map;
        nextDoor = map.GetComponentInChildren<NextStageDoor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        stat.Hp -= Damage;
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


    }

}
