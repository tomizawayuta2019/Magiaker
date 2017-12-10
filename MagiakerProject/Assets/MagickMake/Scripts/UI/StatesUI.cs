using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 各種ステータス表示用スクリプト
/// </summary>
public class StatesUI : MonoBehaviour {
    [SerializeField]
    private Image[] magickimages;
    [SerializeField]
    private GameObject selectMark;
    [SerializeField]
    private Slider HPBar, MPBar;
    static float? maxHP, maxMP;
    [SerializeField]
    private PlayerController PC;

    static public StatesUI statesUI;
    private void Awake() {
        statesUI = this;
    }

    void Start () {
        if (!maxHP.HasValue) maxHP = PC.HP;
        if (!maxMP.HasValue) maxMP = PC.MP;
        HPBar.maxValue = maxHP.Value;
        MPBar.maxValue = maxMP.Value;
	}
	
	// Update is called once per frame
	void Update () {
        HPBar.value = PC.HP;
        MPBar.value = PC.MP;
	}

    /// <summary>
    /// 魔法選択アイコンの切り替え 存在しないものを指定されたらアイコンを消す
    /// </summary>
    /// <param name="value"></param>
    public void MagickSelect(int value) {
        if (value >= 0 && value < magickimages.Length && magickimages[value] != null) {
            selectMark.transform.SetParent(magickimages[value].transform);
			selectMark.transform.localPosition = Vector2.zero;
            selectMark.SetActive(true);
        }
        else selectMark.SetActive(false);
    }

    /// <summary>
    /// 魔法の画像更新
    /// </summary>
    /// <param name="images"></param>
    public void SetMagickImage(Sprite[] images) {
        foreach (var item in images.Select((v, i) => new { v, i })) {
            magickimages[item.i].sprite = item.v;
        }
    }
}
