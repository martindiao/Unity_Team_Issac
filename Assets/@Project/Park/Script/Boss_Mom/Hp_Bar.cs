using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp_Bar : MonoBehaviour
{
     RectTransform _Rect;
     _Stat stat;


     float MaxHp;
     float Hp;


    private void Start()
    {
        stat = GetComponentInParent<_Stat>();

        _Rect = gameObject.GetComponent<RectTransform>();
        MaxHp = stat.Hp;
    }

    private void Update()
    {
        
        Hp = (stat.Hp / MaxHp) * 100;
        _Rect.sizeDelta = new Vector2(Hp, _Rect.rect.height);
    }



}
