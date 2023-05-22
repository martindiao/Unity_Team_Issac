using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRockScripts : MonoBehaviour
{
	public GameObject m_unLock_prefab;

	private bool m_isUnLock = false;

	GameObject m_unLock_Clone;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (m_isUnLock)
			return;
		if (collision.CompareTag("Player") ||collision.CompareTag("PlayerHead"))
		{
			Player player;
			if (collision.CompareTag("Player"))
				player = collision.GetComponent<Player>();
			else
				player = collision.transform.parent.GetComponent<Player>();

			if (player.Keys < 1)
				return;

			m_isUnLock = true;
			player.Keys -= 1;

			m_unLock_Clone = Instantiate(m_unLock_prefab, transform);
			var sprite = GetComponent<SpriteRenderer>();
			sprite.sprite = null;
		}
	}

	public void DestroyObject()
	{
		Destroy(m_unLock_Clone);
		Destroy(gameObject);
	}
}
