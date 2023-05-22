using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfDestroy : MonoBehaviour
{
    public float DestroyTime;

    private void Awake()
    {
        Destroy(gameObject, DestroyTime);
    }
}
