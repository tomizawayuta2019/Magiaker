using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private ScrollRect scroll;
    private float defaultHeight;

    private void Awake()
    {
        scroll = GetComponent<ScrollRect>();
        if (scroll) {
            defaultHeight = scroll.content.sizeDelta.y;
        }
    }

    protected virtual void Start() {
        if (width <= 0) width = 1;//widthは１以上のもののみ受け付ける。
    }

    protected virtual void Update() {
        UpdateScrollView();
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
			b.transform.localScale = baseButton.transform.localScale;//新しい親の大きさに合わせてサイズ調整
            b.transform.localPosition = GetButtonPos(i);
        }
    }

    /// <summary>
    /// ボタンの生成　*一個だけ
    /// </summary>
    /// <param name="strings"></param>
    /// <param name="sprites"></param>
    /// <returns></returns>
    private MagiakerButton MakeButton(List<string> strings, List<Sprite> sprites) {
        return Instantiate(baseButton).Init(strings, sprites);
    }

    /// <summary>
    /// ボタンの生成　＊後から生成する用
    /// </summary>
    /// <returns></returns>
    protected MagiakerButton AddButton(List<string> strings, List<Sprite> sprites) {
        MagiakerButton button = Instantiate(baseButton).Init(strings, sprites);
        button.transform.SetParent(parent);
        button.transform.localScale = Vector2.one;//親が小さければボタンも小さくする
        button.transform.localPosition = GetButtonPos(buttons.Count);
        buttons.Add(button);
        return button;
    }

    protected void UpdateScrollView() {
        if (scroll == null) return;
        
        if (buttons.Count > 0) {
            MagiakerButton button = buttons[buttons.Count - 1];
            Vector2 buttonPos = GetButtonPos(buttons.Count - 1);
            buttonPos.y -= buttons[0].rect.sizeDelta.y / 2;
            scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, -buttonPos.y < defaultHeight ? defaultHeight : -buttonPos.y);
            //Vector2 defPos = new Vector2(pos.x, scroll.content.sizeDelta.y / 2 - button.rect.sizeDelta.y / 2 - 165f);
            //Vector2 defPos = new Vector2(pos.x, pos.y);
            for (int i = 0; i < buttons.Count; i++) {
                //buttons[i].transform.localPosition = defPos + GetButtonPos(i) - pos;
                //buttons[i].rect.anchoredPosition = pos;
                buttons[i].rect.localPosition = GetButtonPos(i);
            }
        }
        else {
            scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, defaultHeight);
        }
    }
}
