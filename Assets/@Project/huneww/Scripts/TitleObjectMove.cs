using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleObjectMove : MonoBehaviour
{
    // 움직임 속도
    public float Speed;
    // 움직일 거리
    public float MoveLength;
    // 현재 위치
    private Vector3 curVec;
    // 다음 위치
    private Vector3 nextVec;
    // 타이틀에 있는지 확인
    private bool inTitle = true;

    private void Awake()
    {
        // 현재 위치 정보 저장
        curVec = transform.position;
    }

    private void Update()
    {
        if (inTitle)
        {
            if (transform.position.y <= curVec.y)
            {
                nextVec.y = 1f * Speed * Time.deltaTime;
            }
            else if (transform.position.y >= curVec.y + MoveLength)
            {
                nextVec.y = (-1f) * Speed * Time.deltaTime;
            }

            transform.position += nextVec;
        }
    }
}
