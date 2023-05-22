using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRock : MonoBehaviour
{
	// 气藕 橇府普
	[SerializeField]
	GameObject m_BombAction_Prefab;

	// 橇府普 努沸阑 包府且 函荐
	private GameObject m_clone;


	private void OnTriggerStay2D(Collider2D collision)
	{
		// 气迫 裹困俊 搓阑 版快
		if (collision.CompareTag("BombRange"))
		{
			// 气藕 汲摹
			m_clone = Instantiate(m_BombAction_Prefab, this.transform.position, Quaternion.identity);
			m_clone.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll; 
			m_clone.GetComponent<Bomb>().m_boomDelay = 0;

			// 积己茄 橇府普 力芭 内风凭
			StartCoroutine(DestroyClone());

			// 倒 力芭
			Destroy(this.gameObject);
		}
	}
	// 1f檬 饶 积己等 橇府普 力芭
	IEnumerator DestroyClone()
	{
		yield return new WaitForSeconds(1f);

		Destroy(m_clone);
	}
}
