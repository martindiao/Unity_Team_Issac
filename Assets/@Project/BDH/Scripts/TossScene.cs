using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		// 씬 로드시 파괴 불과로 설정
		DontDestroyOnLoad(gameObject);
	}

    // 최상위 오브젝트 파괴 함수
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
