using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefabs;
	public GameObject map;

    public GameObject _Camera;

	private void Awake()
    {
        int i;
        i = Random.Range(0, 4);
        
        switch(i)
        {
            case 0:
                map = GameObject.Find("One");
                Debug.Log(map.name);
				break;
			case 1:
                map = GameObject.Find("Two");
				Debug.Log(map.name);
				break;
			case 2:
				map = GameObject.Find("Three");
				Debug.Log(map.name);
				break;
			case 3:
				map = GameObject.Find("Four");
				Debug.Log(map.name);
				break;
		}

        // 플레이어 소환
        SpawnPlayer();

        // 게임 매니저 씬 로드시 파괴 불과로 설정   
        //DontDestroyOnLoad(gameObject);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void SpawnPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("PlayerObj");

        Transform startRoom = map.transform.GetChild(0);

        Vector3 spawnVec = startRoom.position + Vector3.down * 2;

        Vector3 cameraVec = startRoom.position;
        cameraVec.z += -10;

        // 플레이어 소환
        if (player == null)
            Instantiate(PlayerPrefabs, spawnVec, Quaternion.identity);
        else
        {
			player.transform.position = spawnVec;

			var playerObj = player.transform.Find("Player");

			playerObj.gameObject.SetActive(true);
			playerObj.gameObject.transform.position = spawnVec;
		}

		_Camera.transform.position = cameraVec;

        Player stageNum = FindObjectOfType<Player>();

        switch(stageNum.nowStage)
        {
			case 0:
				stageNum.nowStage = 1;
				break;
			case 1:
                stageNum.nowStage = 2;
                break;
        }
	}
}
