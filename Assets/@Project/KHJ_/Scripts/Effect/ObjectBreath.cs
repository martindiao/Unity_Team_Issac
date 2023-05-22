using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreath : MonoBehaviour
{
	private float fadeCount = 0.3f;
	public float addCount = 0.001f;

	private SpriteRenderer sprite;

	private void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (fadeCount > 1f || fadeCount < 0.3f)
			addCount *= -1;

		sprite.color = new Color(1,1,1, fadeCount );

		fadeCount += addCount;
	}


}
