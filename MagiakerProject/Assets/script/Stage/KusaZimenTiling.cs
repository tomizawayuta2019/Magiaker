using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ステージのFloorに適用しているマテリアルのタイリングを変更できない際に使用する、指定数コピーを生成する処理
/// </summary>
public class KusaZimenTiling : MonoBehaviour {
    public GameObject prefab;//生成するオブジェクト
    public Vector3 offset;//生成するオブジェクトのサイズ
    public int countX,countZ;//生成数

    [SerializeField]
    private List<GameObject> objects;

    internal void Init() {
        foreach (GameObject obj in objects) {
            DestroyImmediate(obj);
            //Destroy(obj);
        }
        Vector3 rotation = transform.localEulerAngles;
        transform.eulerAngles = Vector3.zero;
        objects = new List<GameObject>();
        for (int i = 0; i < countX; i++) {
            for (int j = 0; j < countZ; j++) {
                GameObject obj = Instantiate(prefab);
                obj.transform.localScale = prefab.transform.lossyScale;
                obj.transform.SetParent(transform);
                obj.transform.position = transform.position + (new Vector3(offset.x * i, 0, offset.z * j));
                objects.Add(obj);
                obj.transform.localEulerAngles = Vector3.zero;
            }
        }
        transform.eulerAngles = rotation;
    }

    internal void Maintenance() {
        for (int i = 0; i < objects.Count; i++) {
            if (objects[i] == null) {
                objects.RemoveAt(i--);
            }
        }
    }

    internal void RandRotation() {
        foreach (GameObject obj in objects) {
            Vector3 rot = obj.transform.localEulerAngles;
            rot.y = Random.Range(0f,360f);
            obj.transform.localEulerAngles = rot;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(KusaZimenTiling))]
public class KusaZimenUGUI : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Init")) {
            var kusaZimenTiling = target as KusaZimenTiling;
            kusaZimenTiling.Init();
        }

        if (GUILayout.Button("Maintenance"))
        {
            var kusaZimenTiling = target as KusaZimenTiling;
            kusaZimenTiling.Maintenance();
        }

        if (GUILayout.Button("randRotation"))
        {
            var kusaZimenTiling = target as KusaZimenTiling;
            kusaZimenTiling.RandRotation();
        }
    }
}
#endif
