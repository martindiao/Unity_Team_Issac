using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageDoor : MonoBehaviour
{
	private void Start()
	{
		var anim = GetComponent<Animator>();
		anim.Play("NextDoorIdle");
	}

	public void DoorAtive()
	{
		gameObject.SetActive(true);
		var anim = GetComponent<Animator>();
		anim.Play("NextStageDoor");
	}

	public void DoorUnAtive()
	{
		gameObject?.SetActive(false);
	}
}
