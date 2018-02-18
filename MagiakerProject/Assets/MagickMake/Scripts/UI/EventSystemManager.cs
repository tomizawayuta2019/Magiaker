using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemManager : MonoBehaviour {
    private const string Prefab = "Prefabs/EventSystem";
    private static GameObject instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<GameObject>(Prefab));
        }
        Destroy(gameObject);
    }
}
