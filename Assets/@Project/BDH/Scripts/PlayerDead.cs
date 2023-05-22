using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
	// 애니메이션 제어 컴포넌트
	Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();

		StartCoroutine(rotate());
	}

	IEnumerator rotate()
	{
		// 히트 시 효과
		gameObject.transform.localScale = new Vector3(1, 0.75f, 0);
		yield return new WaitForSeconds(0.1f);
		gameObject.transform.localScale = new Vector3(1, 1.5f, 0);
		yield return new WaitForSeconds(0.1f);
		gameObject.transform.localScale = new Vector3(1, 1, 0);

		yield return new WaitForSeconds(0.4f);

		// 쓰러지는 효과

		Vector3 rotaVec = new Vector3(0, 0, -15);
		transform.Rotate(rotaVec);

		yield return new WaitForSeconds(0.075f);

		transform.Rotate(rotaVec);

		yield return new WaitForSeconds(0.05f);

		transform.Rotate(rotaVec);

		yield return new WaitForSeconds(0.015f);

		transform.Rotate(rotaVec);

		yield return new WaitForSeconds(0.01f);

		transform.Rotate(rotaVec);

		yield return new WaitForSeconds(0.025f);

		transform.Rotate(rotaVec);

		Invoke("Dead", 0.1f);
	}

	void Dead()
	{
		// 사망시 워래 각도와 크기로 복귀
		Vector3 rotaVec = new Vector3(0, 0, 90);
		transform.Rotate(rotaVec);

		transform.position += Vector3.down * 0.1f;

		// 완전 사망 스프라이트 출력
		anim.SetBool("isEnd", true);

		Invoke("backToMenu", 1.5f);
	}

	// 플레이어 사망 시 메인 메뉴로 복귀
	void backToMenu()
	{
		LoadinSceneController.Instance.LoadScene("MainMenu");
	}
}
