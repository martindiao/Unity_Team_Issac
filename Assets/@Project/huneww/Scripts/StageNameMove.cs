using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNameMove : MonoBehaviour
{
    private RectTransform recttransform;

    public RectTransform[] points;

    public float MoveSpeed;

    public Text StageName;

    private void Start()
    {
        recttransform = GetComponent<RectTransform>();
        StageName = GetComponent<Text>();

        Invoke("MoveName", 0.5f);

    }

    private void MoveName()
    {
        StartCoroutine(Move());
    }

    /// <summary>
    /// 스테이지 이름 변경 함수
    /// </summary>
    /// <param name="stagename">변경할 스테이지 이름</param>
    public void ChangeStageName(string stagename)
    {
        StageName.text = stagename;
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(2.3f);
        while (recttransform.position.x < points[0].transform.position.x)
        {
            yield return null;
            recttransform.position += new Vector3(1, 0, 0) * MoveSpeed;
        }

        yield return new WaitForSeconds(1.0f);

        while (recttransform.position.x < points[1].transform.position.x)
        {
            yield return null;
            recttransform.position += new Vector3(1, 0, 0) * MoveSpeed;
        }

        Destroy(gameObject);

    }

}
