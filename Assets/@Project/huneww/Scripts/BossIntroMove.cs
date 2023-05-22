using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossIntroMove : MonoBehaviour
{
    public GameObject[] objects;
    public RectTransform[] points;
    public GameObject Vsimage;

    public float PSpeed;
    public float BSpeed;

    public CanvasGroup CGroup;

    public Image BossImage;
    public Image BossName;

    public Sprite[] bossimage;
    public Sprite[] bossname;

    public void StartIntro()
    {
        StartCoroutine(Move());

        var player = GameObject.FindGameObjectWithTag("Player");
        var stage = player.GetComponent<Player>();
        switch (stage.nowStage)
        {
            case 1:
                BossImage.sprite = bossimage[0];
                BossName.sprite = bossname[0];
                break;
            case 2:
                BossImage.sprite = bossimage[1];
                BossName.sprite = bossname[1];
                break;
        }
    }

    IEnumerator Move()
    {
        Time.timeScale = 0f;

        CGroup.alpha = 1.0f;

        while (objects[0].transform.position.x < points[0].transform.position.x)
        {
            yield return null;
            objects[0].transform.position += Vector3.right * PSpeed;
        }

        while (Vsimage.transform.localScale.x < 1.0f || Vsimage.transform.localScale.y < 1.0f)
        {
            yield return null;
            Vsimage.transform.localScale += new Vector3(0.01f, 0.01f, 0);
        }

        while (objects[1].transform.position.x > points[1].transform.position.x)
        {
            yield return null;
            objects[1].transform.position += Vector3.left * BSpeed;
        }

        //yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade());

        Destroy(gameObject);
    }

    IEnumerator Fade()
    {
        while (CGroup.alpha != 0f)
        {
            yield return null;
            CGroup.alpha -= Time.unscaledDeltaTime * 2;
        }

        if (CGroup.alpha <= 0f)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
}
