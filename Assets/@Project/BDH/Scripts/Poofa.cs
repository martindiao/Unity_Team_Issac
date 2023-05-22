using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poofa : MonoBehaviour
{
	private void Awake()
	{
		// 애니메이션 재생이 끝날 시 오브젝트 삭제
		Destroy(gameObject, 0.7f);
	}
}
