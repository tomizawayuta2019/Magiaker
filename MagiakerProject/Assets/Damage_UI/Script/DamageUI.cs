using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 東谷
/// </summary>
public class DamageUI : MonoBehaviour {

    public float speed;

    void Start()
    {
        speed = 2;
    }

    // Update is called once per frame
    void Update ()
    {
        transform.position += transform.up * Time.deltaTime * speed;
        transform.rotation = Camera.main.transform.rotation;
    }
}
