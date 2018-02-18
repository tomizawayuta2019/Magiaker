using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 終了したSEを破棄するClass
/// </summary>
public class DestroySE : MonoBehaviour {
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!source || !source.isPlaying) {
            Destroy(gameObject);
        }
    }

}
