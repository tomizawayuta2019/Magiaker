using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class SEState : MonoBehaviour {
    public float startTime;
    private AudioSource target;

    private void Awake()
    {
        target = GetComponent<AudioSource>();
        target.time = startTime;
    }

    internal void TestPlay() {
        target = GetComponent<AudioSource>();
        target.time = startTime;
        target.Play();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SEState))]
internal class SEUGUI : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("TestPlay"))
        {
            SEState sEState = target as SEState;
            sEState.TestPlay();
        }
    }

    //public override void OnInspectorGUI()
    //{

    //    base.OnInspectorGUI();

    //    if (GUILayout.Button("TestPlay"))
    //    {
    //        SEState sEState = target as SEState;
    //        sEState.TestPlay();
    //    }
    //}
}
#endif