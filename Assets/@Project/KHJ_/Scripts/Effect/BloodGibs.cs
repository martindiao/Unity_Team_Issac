using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// blood오브젝트들의 퍼짐 효과
public class BloodGibs : MonoBehaviour
{
	// 자식객체들의 정보를 받을 배열
	private Collider2D[] AllChilds;

	private void Awake()
	{
		// 자식객체 정보 전달
		AllChilds = gameObject.GetComponentsInChildren<Collider2D>();
	}

	// 생성과 동시에 효과 실행
	private void Start()
	{
		StartCoroutine(DestroyDealy());	
	}

	IEnumerator DestroyDealy()
	{
		// 2초의 대기시간 후
		yield return new WaitForSeconds(2.0f);

		// 모든 자식 순환하며 설정
		foreach(var child in AllChilds)
		{
			// rigidbody의 Constrains 전부 Freeze
			child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			// 자식객체의 BoxCollider 비활성화
			child.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
