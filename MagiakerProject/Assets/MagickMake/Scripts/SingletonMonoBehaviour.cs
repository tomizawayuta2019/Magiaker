using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シングルトンなクラスの基底クラス
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null) instance = this as T;
        else Destroy(this);
    }
}
