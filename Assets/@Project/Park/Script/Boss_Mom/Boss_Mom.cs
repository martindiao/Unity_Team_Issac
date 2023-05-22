using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Mom : MonoBehaviour
{

    GameObject map;
    GameObject doorObj;
    NextStageDoor nextDoor;

    SpriteRenderer spriteRenderer;//스프라이트 렌더

    _Stat stat;

    private void Start()
    {
        stat = GetComponent<_Stat>();

        map = FindObjectOfType<GameManager>().map;
        Debug.Log(map.name);
        //nextDoor = map.GetComponentInChildren<NextStageDoor>();
        //nextDoor.DoorUnAtive();

        Transform[] obj = map.GetComponentsInChildren<Transform>();

        foreach (var n in obj)
            if (n.name == "NextStageDoor")
            {
                nextDoor = n.GetComponent<NextStageDoor>();
                nextDoor.DoorUnAtive();
            }
        Debug.Log(nextDoor.name);
    }

    private void Update()
    {
        if (stat.Hp <= 0)
        {
            nextDoor.DoorAtive();
            

        }
    }

	private void OnDestroy()
	{
        nextDoor.DoorAtive();
    }
}

