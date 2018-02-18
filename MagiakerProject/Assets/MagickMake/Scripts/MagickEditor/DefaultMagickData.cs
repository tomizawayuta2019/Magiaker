using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//ゲーム開始時に保持しているデフォルトの魔法作成用クラス
[CreateAssetMenu(menuName = "Example/Create DefaultMagicDataManager")]
public class DefaultMagickData : ScriptableObject {
    [SerializeField]
    public List<Magick> defaultMagics;//保存中の魔法
    public int selectMagickNum;

    public void Save(int num) {
        if (Item_Magic.m_Magicks.Length > num && Item_Magic.m_Magicks[num] != null) {
            defaultMagics.Add(Item_Magic.m_Magicks[num]);
        }
    }

    public void Delete() {
        defaultMagics = new List<Magick>();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DefaultMagickData))]
internal class DefaultMagickDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save"))
        {
            DefaultMagickData t = target as DefaultMagickData;
            t.Save(t.selectMagickNum);
        }

        if (GUILayout.Button("Delete"))
        {
            DefaultMagickData t = target as DefaultMagickData;
            t.Delete();
        }
    }
}
#endif