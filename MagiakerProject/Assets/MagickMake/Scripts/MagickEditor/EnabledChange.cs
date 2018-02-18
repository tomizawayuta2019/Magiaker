using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObjectのEnabledを切り替える
/// </summary>
public class EnabledChange : MonoBehaviour {
    public List<GameObject> list;
    private bool state = true;
    
    public void Change() {
        state = !state;
        foreach (GameObject obj in list) {
            obj.SetActive(state);
        }
    }

    public void Change(bool value) {
        state = value;
        foreach (GameObject obj in list) {
            obj.SetActive(state);
        }
    }
}
