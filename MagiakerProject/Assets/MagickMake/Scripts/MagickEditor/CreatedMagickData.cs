using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Example/CreatedMagickData")]
public class CreatedMagickData : ScriptableObject {
    [SerializeField]
    private List<Magick> defaultMagicks;//最初から保存されている魔法履歴（開発陣が作ったやつとか）
    public static List<Magick> magickList;//作成された魔法の情報を取得し、ここに格納しておく
    private static List<Magick> saveMagicks = new List<Magick>();//このゲーム中に保存した魔法のリスト
    public static Magick cleardSaveMagick;//クリア時に保存した魔法

    public void Init()
    {
        magickList = new List<Magick>();
        DataUpdate();
    }

    /// <summary>
    /// データの更新
    /// </summary>
    public void DataUpdate() {
        //初期保持魔法と作成履歴を合わせる
        magickList = new List<Magick>(saveMagicks);
        magickList.AddRange(defaultMagicks);
        magickList.AddRange(MagicSystemManager.instance.defaultMagickData.defaultMagics);
    }

    /// <summary>
    /// ゲームクリア時の魔法保存
    /// </summary>
    /// <param name="m"></param>
    public static void CleardSaveMagick(Magick m) {
        cleardSaveMagick = m;
    }

    /// <summary>
    /// 魔法の作成履歴を保存する
    /// </summary>
    /// <param name="m">対象の魔法</param>
    public static void AddSaveMagick(Magick m) {
        //引数の魔法を一番目に保存する
        saveMagicks.Reverse();
        saveMagicks.Add(m);
        saveMagicks.Reverse();
        magickList.Add(m);
    }

    //選択中の魔法をリストに追加
    public void Save()
    {
        if (Item_Magic.GetSelectMagick() != null)
        {
            cleardSaveMagick = Item_Magic.GetSelectMagick();
            Init();
            //現在保持中の魔法を初期化する
            Item_Magic.InitMagicks();
        }
    }

    //全ての魔法をリストから破棄
    internal void Delete()
    {
        defaultMagicks = new List<Magick>();
    }

    internal void Update()
    {
        magickList = defaultMagicks;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CreatedMagickData))]
internal class SaveDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save"))
        {
            CreatedMagickData t = target as CreatedMagickData;
            t.Save();
        }

        if (GUILayout.Button("Delete"))
        {
            CreatedMagickData t = target as CreatedMagickData;
            t.Delete();
        }

        if (GUILayout.Button("Update"))
        {
            CreatedMagickData t = target as CreatedMagickData;
            t.Update();
        }
    }
}
#endif

