using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnVisivle : MonoBehaviour {
    [SerializeField]
    private float falseValue;
    private float trueValue;

    private SpriteRenderer image;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        trueValue = image.color.a;
    }

    public void SetVivivlity(bool value) {
        Color color = image.color;
        color.a = value ? trueValue : falseValue;
        image.color = color;
    }
}
