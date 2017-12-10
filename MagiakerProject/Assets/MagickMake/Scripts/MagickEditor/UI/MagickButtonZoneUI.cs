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

    private List<Sprite> GetMagickIconData(Magick m) {
        if (m != null) return new List<Sprite>() { m.magickIcon };
        return null;
    }

    private List<string> GetMagickNameData(Magick m){
        if (m != null) return new List<string>() { m.magickName };
        return null;
    }

    private void Start()
    {
        //押された魔法の番号を返すだけのボタンを作成する
        List<List<string>> strings = new List<List<string>>();
        List<List<Sprite>> sprites = new List<List<Sprite>>();

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
                CreatedMagickData.Init();
                if (CreatedMagickData.CreatedMagicks == null) {
                    Debug.LogAssertion("作成済み魔法リストのUIを生成しようとしましたが、魔法データの参照が見つかりませんでした。");
                } 
                else foreach (Magick m in CreatedMagickData.CreatedMagicks) {
                    strings.Add(GetMagickNameData(m));
                    sprites.Add(GetMagickIconData(m));
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
			foreach (var item in CreatedMagickData.CreatedMagicks.Select((v,i) => new { v, i })) {
				buttons[item.i].button.onClick.AddListener(() => OnPushed(item.i));
			}
			break;
		}
    }

    private void OnPushed(int num) {
        MagickMakeManager.MM.MagicSelect(type, num, buttons[num].gameObject);
    }
}
