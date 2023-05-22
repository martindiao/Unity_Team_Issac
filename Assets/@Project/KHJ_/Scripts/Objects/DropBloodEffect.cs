using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// 블러드 이펙트
public class DropBloodEffect : MonoBehaviour
{
	// 떨어트릴 이펙트 오브젝트 종류
	public List<GameObject> Objects;

	// 콜백용 메소드
	public void DropEffect()
	{
		// 떨어트릴 이펙트 랜덤선정
		int rand = Random.Range(0, Objects.Count);

		// 이펙트 생성
		var obj = Instantiate(Objects[rand],transform.position,Quaternion.identity);

		// 이펙트 지우기
		StartCoroutine(DelayObj(obj));

	}

	// n초간 지연 후 지우는 액션
	private IEnumerator DelayObj(GameObject _obj)
	{
		yield return new WaitForSeconds(5f);

		StartCoroutine(DestroyObj(_obj));

	}

	// 지우는 액션
	private IEnumerator DestroyObj(GameObject obj)
	{
		while(true)
		{
			// 약간의 텀을 줌
			yield return new WaitForSeconds(0.01f);
			
			// 오브젝트의 크기가 일정사이즈 이하일 경우
			if(obj.transform.localScale.x < 0.3)
			{
				// 파괴
				Destroy(obj);
				break;
			}

			// 오브젝트 크기 줄이기
			obj.transform.localScale += new Vector3(-0.1f, -0.1f, 0);
		}
	}

}
