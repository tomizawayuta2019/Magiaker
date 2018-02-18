using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagickStatesUI : MonoBehaviour {
    [SerializeField]
    private Text Damage, MP;
	[SerializeField]
	private InputField Name;
	private string oldName;
	[SerializeField]
	private Image Icon;

    private Coroutine MPCor, DamageCor;
	private float targetDamage = -100, targetMP = -100;//コルーチンで遷移中のMPとダメージ

	static public MagickStatesUI magickStatesUI;

	void Awake(){
		magickStatesUI = this;
	}

	private Magick SelectedMagick;//選択中の魔法　Managerクラスを参照するかManagerからSetするように書き換え
    public void SetMagick(Magick m) {
        SelectedMagick = m;
		Name.text = SelectedMagick.magickName;
    }

    private void Start()
    {
        if (!Name || !Damage || !MP) {
            Debug.LogAssertion("UITextが設定されていません");
        }
    }

    private void Update()
    {
		if (Name.text != SelectedMagick.magickName) {
			//Debug.Log ("Update nameText to "  + SelectedMagick.magickName);
			SelectedMagick.magickName = Name.text;
		}

        //数値がぬるぬる動く感じにする
        //魔法の総合ダメージを表示・プレビュー中は与えたダメージの表示
		if (targetDamage != SelectedMagick.GetDamage) {
            //DamageCor = StartCoroutine(TextDisplay(Damage, (int)SelectedMagick.magickDamage));
			targetDamage = SelectedMagick.GetDamage;
			Damage.text = targetDamage.ToString ();
        }

        if (targetMP != SelectedMagick.GetMP) {
			targetMP = SelectedMagick.GetMP;
			MP.text = targetMP.ToString();
        }

        if (Icon.sprite != SelectedMagick.magickIcon) {
            Icon.sprite = SelectedMagick.magickIcon;
        }
    }

    /// <summary>
    /// 整数で表示されるTextを目的の値まで段階的に変更する
    /// </summary>
    /// <param name="text">対象のText</param>
    /// <param name="target">目的の値</param>
    /// <returns></returns>
    IEnumerator TextDisplay(Text text,int target,float waitTime = 0.2f) {
        int now = int.Parse(text.text);
        bool isUpper = now < target;
        while (now != target) {
            if (isUpper)
                text.text = (++now).ToString();
            else if (!isUpper)
                text.text = (--now).ToString();
            yield return new WaitForSeconds(waitTime);
        }
    }
}
