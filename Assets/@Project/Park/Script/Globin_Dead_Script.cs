using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globin_Dead_Script : MonoBehaviour
{
    _Stat stat;

    SpriteRenderer spriteRenderer;
    DropBloodEffect_Monster DropBlood;

    public GameObject _Death;//죽었을때 생성되는 오브젝트

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stat = GetComponent<_Stat>();
        DropBlood = GetComponent<DropBloodEffect_Monster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            spriteRenderer.color = Color.red;


            Invoke("colorreturn", 0.2f);

        }
    }

    private void colorreturn()
    {
        stat.Hp -= 1;
        DropBlood.DropEffect();

        spriteRenderer.color = Color.white;
    }
    // Update is called once per frame
    void Update()
    {

            if(stat.Hp <=0)
            {

            Instantiate(_Death,transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

        
    }
}
