using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
	// 그림자 렌더 상태 확인
    SpriteRenderer s_renderer;

    void Start()
    {
		s_renderer = GetComponent<SpriteRenderer>();

		// 그림자 불투명도 조정
		s_renderer.color = new Color(1, 1, 1, 0.5f);
	}

}
