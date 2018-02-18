using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MagickButtonZoneUI : ButtonList {
    public enum TargetType {
        have,//現在保持している魔法
        created,//これまでに作成された魔法
    }
    [SerializeField]
    public TargetType type;

    public Text text;

    /// <summary>
    /// 魔法のアイコンを取得
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private List<Sprite> GetMagickIconData(Magick m) {
        if (m != null) return new List<Sprite>() { m.magickIcon };
        return null;
    }

    /// <summary>
    /// 魔法の名前・MPを取得
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private List<string> GetMagickNameData(Magick m){
        if (m != null) {
            //Debug.Log(m.GetMP);
            return new List<string>() { m.magickName, m.GetMP.ToString() + "MP" };
        }
        return null;
    }

    protected override void Start()
    {
        base.Start();
        //押された魔法の番号を返すだけのボタンを作成する
        var strings = new List<List<string>>();
        var sprites = new List<List<Sprite>>();

        // [条件式] ? [tureの場合] : [falseの場合]だと結果の型が違うとエラー。書き方は同じだけど、型が配列とListなので…
        //foreach (Magick m in type == TargetType.have ? Item_Magic.m_Magicks : createdMagick.CreatedMagicks) {
        //    strings.Add(GetMagickNameData(m));
        //    sprites.Add(GetMagickIconData(m));
        //}

        //上記のものが出来なかったのでswicthに書き換え
        switch (type) {
            case TargetType.have:
                foreach (Magick m in Item_Magic.m_Magicks) {
                    strings.Add(GetMagickNameData(m));
                    sprites.Add(GetMagickIconData(m));
                }
                break;
            case TargetType.created:
                //CreatedMagickData.Init();
                if (CreatedMagickData.magickList == null) {
                    Debug.LogAssertion("作成済み魔法リストのUIを生成しようとしましたが、魔法データの参照が見つかりませんでした。");
                }
                else {
                    foreach (Magick m in CreatedMagickData.magickList) {
                        strings.Add(GetMagickNameData(m));
                        sprites.Add(GetMagickIconData(m));
                    }
                }
                break;
        }
        
        MakeButtons(strings, sprites);

		switch (type) {
		case TargetType.have:
			//ボタンのOnPshedを設定する 実行文をラムダ式にした場合、使用した変数をアドレス参照するので注意（for()でiを宣言して使うとiの最終値が呼ばれる）
			foreach (var item in Item_Magic.m_Magicks.Select((v,i) => new { v, i })) {
				buttons[item.i].button.onClick.AddListener(() => OnPushed(item.i));
			}
			break;
		case TargetType.created:
			foreach (var item in CreatedMagickData.magickList.Select((v,i) => new { v, i })) {
				buttons[item.i].button.onClick.AddListener(() => OnPushed(item.i));
			}
			break;
		}
    }

    private void OnPushed(int num) {
        MagickMakeManager.Instance.MagicSelect(type, num, buttons[num].gameObject);
    }

    protected override void Update()
    {
        base.Update();

        switch (type) {
            case TargetType.have:
                //ボタンのテキストと画像を更新する
                foreach (var item in Item_Magic.m_Magicks.Select((v, i) => new { v, i })) {
                    if (item.v != null) {
                        if (buttons[item.i].texts[0].text != item.v.magickName) {
                            buttons[item.i].texts[0].text = item.v.magickName;
                        }
                        if (buttons[item.i].texts[1].text != item.v.GetMP.ToString()) {
                            buttons[item.i].texts[1].text = item.v.GetMP.ToString();
                        }
                        if (buttons[item.i].images[0].sprite != item.v.magickIcon) {
                            buttons[item.i].images[0].sprite = item.v.magickIcon;
                            buttons[item.i].images[0].color = Color.white;
                        }
                            
                    }
                    else {
                        buttons[item.i].texts[0].text = "";
                        buttons[item.i].texts[1].text = "";
                        buttons[item.i].images[0].sprite = null;
                    }
                    
                }
                break;
            case TargetType.created:
                MagicSystemManager.instance.createdMagickData.DataUpdate();
                if (buttons.Count < CreatedMagickData.magickList.Count) {

                    //初期化する
                    foreach (MagiakerButton target in buttons) {
                        Destroy(target.gameObject);
                    }
                    buttons = new List<MagiakerButton>();
                    Start();
                    //var strings = new List<List<string>>();
                    //var sprites = new List<List<Sprite>>();

                    //foreach (var m in CreatedMagickData.magickList.Select((v,i) => new { v, i }))
                    //{
                    //    if (m.i >= buttons.Count) {
                    //        strings.Add(GetMagickNameData(m.v));
                    //        sprites.Add(GetMagickIconData(m.v));
                    //    }
                    //}

                    //for (int i = 0; i < strings.Count; i++) {
                    //    int buttonNum = buttons.Count;
                    //    AddButton(strings[i], sprites[i]).button.onClick.AddListener(() => OnPushed(buttonNum));
                    //}
                }
                break;
        }

        if (text)
            text.text = buttons.Count.ToString();
    }
}
