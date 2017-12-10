using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonList : MonoBehaviour {
    [SerializeField]
    protected MagiakerButton baseButton;//生成するボタンprefab
    [SerializeField]
    protected Transform parent;

    [SerializeField]
    protected int width;//横に並べる数

    [SerializeField]
    protected Vector2   pos,    //最初に配置する地点
                        size,   //ボタンのサイズ
                        offset; //ボタンとボタンの間の余剰幅

    protected List<MagiakerButton> buttons = new List<MagiakerButton>();//生成したボタンのリスト

    private void Start() {
        if (width <= 0) width = 1;//widthは１以上のもののみ受け付ける。
    }

    private Vector2 GetButtonPos(int num) {
        //                                                             上から下へ並べるので、-にする
        return pos + new Vector2(num % width * (offset.x + size.x), num / width * -(offset.y + size.y));
    }

    /// <summary>
    /// ボタンリストの生成
    /// </summary>
    /// <param name="stirngs"></param>
    /// <param name="sprites"></param>
    protected virtual void MakeButtons(List<List<string>> strings, List<List<Sprite>> sprites) {
        //大きいリストをループ数の基準にする
        int Length = strings.Count < sprites.Count ? sprites.Count : strings.Count;
        List<string> str;
        List<Sprite> spr;
        for (int i = 0; i < Length; i++) {
            str = i < strings.Count ? strings[i] : null;
            spr = i < sprites.Count ? sprites[i] : null;
            MagiakerButton b = MakeButton(str, spr);
            buttons.Add(b);

            b.transform.SetParent(parent);
			b.transform.localScale = baseButton.transform.localScale;//親が小さければボタンも小さくする
            b.transform.localPosition = GetButtonPos(i);
        }
    }

    /// <summary>
    /// ボタンの生成　*一個だけ
    /// </summary>
    /// <param name="strings"></param>
    /// <param name="sprites"></param>
    /// <returns></returns>
    protected MagiakerButton MakeButton(List<string> strings, List<Sprite> sprites) {
        return Instantiate(baseButton).Init(strings, sprites);
    }
}
