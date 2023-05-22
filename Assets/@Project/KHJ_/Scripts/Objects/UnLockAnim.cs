using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockAnim : MonoBehaviour
{
	Animator anim;

	public GameObject m_Destroy_Prefab;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
		{
			Instantiate(m_Destroy_Prefab,transform.position,Quaternion.identity);
			transform.parent.GetComponent<LockRockScripts>().DestroyObject();
		}
	}
}
