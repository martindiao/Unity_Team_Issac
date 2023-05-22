using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Destroy : MonoBehaviour
{
    public float AnimationTime;//죽는 애니메이션 시간을 저장하는 함수
    public Animator DeathAnim;

    public GameObject BulletObject;//블렛 오브젝트가 있을경우엔 총알 발사
    public int num; //발사되는 총알 갯수

    public GameObject SpawnObject;//애니메이션이 재생되고 생성될 오브젝트
    void Start()
    {
        DeathAnim = GetComponent<Animator>();

        AnimationTime = DeathAnim.GetCurrentAnimatorStateInfo(0).length;

        Invoke("DestroyObject", AnimationTime);

        InGameSFXManager.instance.TearDestroy();

    }

    void DestroyObject()
    {
        if (SpawnObject != null)
        {
            Instantiate(SpawnObject, transform.position,Quaternion.identity);
        }

        if(BulletObject != null)
        {
            for(int i = 0; i <num; i++)
            {
                Instantiate(BulletObject, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }


}
