using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImageChoes : MonoBehaviour
{
    private Image image;

    public Sprite[] Images;

    private void Awake()
    {
        image= GetComponent<Image>();
        image.sprite = Images[Random.Range(0, Images.Length)];
    }
}
