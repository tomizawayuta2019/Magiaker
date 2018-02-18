using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 作成者　富澤勇太
/// </summary>
public class BulletBottonZoneUI : ButtonList {
    [SerializeField]
    public MagickMakeManager magickManager;
    [SerializeField]
    private element selectElement;
    [SerializeField]
    private Image bulletFrame;

    [System.Serializable]
    private class Folders : ElementValue<GameObject> { }
    [SerializeField]
    private Folders folders;

    protected override void Start() {
        base.Start();
        InitBulletButton();
		ElementButtonClick (1);
    }

    /// <summary>
    /// 全弾道ボタンの生成
    /// </summary>
    public void InitBulletButton() {
        magickManager.bulletManager.Init();
		List<List<string>> strings = new List<List<string>> ();
		List<List<Sprite>> sprites = new List<List<Sprite>> ();

		List<string> str;
		List<Sprite> spr;
		foreach (Bullet bul in magickManager.bulletManager.Bullets) {
			str = new List<string> (){ bul.GetBulletName() };
			spr = new List<Sprite>() { bul.Icon };

			strings.Add (str);
			sprites.Add (spr);
		}

		MakeButtons (strings, sprites);

		foreach (var item in magickManager.bulletManager.Bullets.Select( (v, i) => new{v, i})) {
			buttons [item.i].button.onClick.AddListener (() => OnClick (item.v));
		}
    }

    /// <summary>
    /// 子要素のボタンを押した際のアクション
    /// </summary>
    /// <param name="bul">そのボタンに対応した弾道</param>
    public void OnClick(Bullet bul) {
        magickManager.BulletInit(bul);
    }

	/// <summary>
	/// 属性タブボタン
	/// </summary>
	/// <param name="num">属性のボタン</param>
	public void ElementButtonClick(int num) {
		element ele = (element)System.Enum.ToObject (typeof(element), num);
		//bulletFrame.color = ConstData.GetElementColor (ele);
		magickManager.SetElement (ele);
        folders.GetValue(ele).transform.SetSiblingIndex(2);
	}
}
