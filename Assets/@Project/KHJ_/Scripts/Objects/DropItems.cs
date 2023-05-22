using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 드랍 스크립트
public class DropItems : MonoBehaviour
{
	// 드랍할 아이템 목록
	public List<GameObject> items;

	// 1회 드랍용 bool
	private bool m_isDroped = false;

	// 다른 스크립트에서 호출
	public void ItemDropEvent()
	{
		if (m_isDroped)
			return;

		// 드랍할 아이템 선정
		int rand = Random.Range(0, items.Count);

		// 드랍 위치 선정
		float posX = Random.Range(-1f, 1f);
		float posY = Random.Range(-1f, 1f);

		// 인수목록(드랍 아이템, 드랍할 위치)
		Instantiate(items[rand],
			new Vector3(transform.position.x + posX,transform.position.y + posY,0),
			Quaternion.identity);

		if (rand == 0)
		{
			InGameSFXManager.instance.CoinDrop();
		}
		else if (rand == 1)
		{
			InGameSFXManager.instance.KeyDrop();
		}

		m_isDroped = true;
	}


}
