using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour {
    private static BGMManager instance;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        //古いBGMを破棄する
        if (instance) {
            if (instance.audioSource.clip == audioSource.clip)
            {
                Destroy(gameObject);
                return;
            }
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
