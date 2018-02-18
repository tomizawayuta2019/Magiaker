using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 各種ステータス表示用スクリプト
/// </summary>
public class StatesUI : MonoBehaviour
{
    [SerializeField]
    private Image[] magickimages;
    [SerializeField]
    private GameObject selectMark;
    [SerializeField]
    private Slider HPBar, MPBar;
    [SerializeField]
    private Text HPText, MPText;
    static float? maxHP, maxMP;
    private PlayerController PC;

    //魔法詳細表示追加
    private float alpha;
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private Text magickNameUI;//size82付近 中央揃え

    static public StatesUI statesUI;
    private void Awake()
    {
        statesUI = this;
    }

    void Start()
    {
        PC = PlayerController.instance;

        if (!maxHP.HasValue) maxHP = PlayerController._HP;
        if (!maxMP.HasValue) maxMP = PlayerController._MP;
        HPBar.maxValue = maxHP.Value;
        MPBar.maxValue = maxMP.Value;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(HPBar.value);
        if (PC && HPBar && MPBar) {
            HPBar.value = PlayerController._HP;
            MPBar.value = PlayerController._MP;

            if (HPText != null)
            {
                HPText.text = HPBar.value.ToString();
            }
            if (MPText != null)
            {
                MPText.text = MPBar.value.ToString();
            }
        }

        for (int i = 0; i < magickimages.Length; i++)
        {
            if (Item_Magic.m_Magicks[i] != null)
            {
                magickimages[i].sprite = Item_Magic.m_Magicks[i].magickIcon;
            }
            else
            {
                magickimages[i].sprite = null;
            }
        }

        if (alpha >= 0)
        {
            alpha -= 1.0f * (fadeSpeed * Time.deltaTime);
            magickNameUI.color = new Color(0, 0, 0, alpha);
        }
    }

    /// <summary>
    /// 魔法選択アイコンの切り替え 存在しないものを指定されたらアイコンを消す
    /// </summary>
    /// <param name="value"></param>
    public void MagickSelect(int value)
    {
        if (value >= 0 && value < magickimages.Length && magickimages[value] != null)
        {
            selectMark.transform.SetParent(magickimages[value].transform);
            selectMark.transform.localPosition = Vector2.zero;
            selectMark.SetActive(true);
            //魔法名などの表示処理
            SetMagicText(Item_Magic.m_Magicks[value]);
        }
        else
        {
            selectMark.SetActive(false);
        }
    }

    /// <summary>
    /// 魔法の画像更新
    /// </summary>
    /// <param name="images"></param>
    public void SetMagickImage(Sprite[] images)
    {
        foreach (var item in images.Select((v, i) => new { v, i }))
        {
            magickimages[item.i].sprite = item.v;
        }
    }

    /// <summary>
    /// 消費MP・魔法名テキスト表示
    /// </summary>
    /// <param name="magick"></param>
    private void SetMagicText(Magick magick)
    {
        if (magick != null)
        {
            magickNameUI.text = "消費MP:" + magick.GetMP + " " + magick.magickName;
            magickNameUI.enabled = true;
            alpha = 1;
        }
        else {
            magickNameUI.text = "";
            magickNameUI.enabled = true;
            alpha = 0.01f;
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;

///// <summary>
///// 各種ステータス表示用スクリプト
///// </summary>
//public class StatesUI : MonoBehaviour {
//    [SerializeField]
//    private Image[] magickimages;
//    [SerializeField]
//    private GameObject selectMark;
//    [SerializeField]
//    private Slider HPBar, MPBar;
//    [SerializeField]
//    private Text HPText, MPText;
//    static float? maxHP, maxMP;
//    [SerializeField]
//    private PlayerController PC;

//    static public StatesUI statesUI;
//    private void Awake() {
//        statesUI = this;
//    }

//    void Start () {
//        if (PC) {
//            if (!maxHP.HasValue)
//            {
//                maxHP = PC.HP;
//                HPBar.maxValue = maxHP.Value;
//            }
//            if (!maxMP.HasValue)
//            {
//                maxMP = PC.MP;
//                MPBar.maxValue = maxMP.Value;
//            }
//        }

//	}

//	// Update is called once per frame
//	void Update () {
//        if (PC) {
//            if (HPBar)
//            {
//                HPBar.value = PC.HP;
//                if (HPText != null)
//                {
//                    HPText.text = HPBar.value.ToString();
//                }
//            }

//            if (MPBar)
//            {
//                MPBar.value = PC.MP;
//                if (MPText != null)
//                {
//                    MPText.text = MPBar.value.ToString();
//                }
//            }
//        }


//        for (int i = 0; i < magickimages.Length; i++) {
//            if (Item_Magic.m_Magicks[i] != null)
//            {
//                magickimages[i].sprite = Item_Magic.m_Magicks[i].magickIcon;
//            }
//            else {
//                magickimages[i].sprite = null;
//            }
//        }
//	}

//    /// <summary>
//    /// 魔法選択アイコンの切り替え 存在しないものを指定されたらアイコンを消す
//    /// </summary>
//    /// <param name="value"></param>
//    public void MagickSelect(int value) {
//        if (value >= 0 && value < magickimages.Length && magickimages[value] != null) {
//            selectMark.transform.SetParent(magickimages[value].transform);
//			selectMark.transform.localPosition = Vector2.zero;
//            selectMark.SetActive(true);
//        }
//        else selectMark.SetActive(false);
//    }

//    /// <summary>
//    /// 魔法の画像更新
//    /// </summary>
//    /// <param name="images"></param>
//    public void SetMagickImage(Sprite[] images) {
//        foreach (var item in images.Select((v, i) => new { v, i })) {
//            magickimages[item.i].sprite = item.v;
//        }
//    }
//}
