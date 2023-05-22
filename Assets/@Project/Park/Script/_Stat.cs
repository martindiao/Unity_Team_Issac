using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Stat : MonoBehaviour
{
    public float MaxHp;
    public float Hp;//오브젝트의 체력
    public int Atk; //오브젝트 공격력

    public int Bullet_Num; //오브젝트가 총알을 쏘는 오브젝트일때 발사할 총알 갯수
    public float Atk_Speed; //공격하는 속도

	public void Start()
	{
        Hp = MaxHp;
	}
}
