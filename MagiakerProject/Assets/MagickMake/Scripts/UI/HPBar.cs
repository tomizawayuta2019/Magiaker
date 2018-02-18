using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 東谷 太喜
/// </summary>
public class HPBar : MonoBehaviour {

    public int Hp;//体力
    public int Mp;//マナ

    public float reduceSpead;//バーの減衰する速さ
    public float waitSec;//コルーチンが待つ秒数

    private Text hpText;//テキスト表示する体力
    private Text mpText;//テキスト表示するマナ

    private int hpCount;//後ろのバーを下げる補助フラグ
	private int mpCount;//後ろのバーを下げる補助フラグ
    private float hpTimer;//waitSec秒後backHpCorBoolをtrueにする
	private float mpTimer;//waitSec秒後にbackMpCorBoolをtrueにする
	private bool hpTimeBool;//trueのときhpCoroutineの値を動かす
    private bool mpTimeBool;//trueのときmpCoroutineの値を動かす
    private bool backHpTimeBool;//trueのとき後ろのHPバーを減らす
    private bool backMpTimeBool;//trueのとき後ろのMPバーを減らす

    [SerializeField]
    private Slider hpBar;//見えているHPバー
    [SerializeField]
    private Slider backHpBar;//遅れて減るHPバー
    [SerializeField]
    private Slider mpBar;//見えているMPバー
    [SerializeField]
    private Slider backMpBar;//遅れて減るMPバー

    void Start ()
    {
        //HPとMPのスライダーをHPとMPと同じ値にする。
        hpBar.value = hpBar.maxValue = backHpBar.value = backHpBar.maxValue = Hp;
        mpBar.value = backMpBar.value = mpBar.maxValue = backMpBar.maxValue = Mp;
        hpTimeBool = false;
        mpTimeBool = false;

        //Textを習得
        hpText = GameObject.Find("HpText").GetComponent<Text>();
        mpText = GameObject.Find("MpText").GetComponent<Text>();

        //Textの初期化
        hpText.text = "" + Mathf.Floor(hpBar.value) + " / " + hpBar.maxValue;
        mpText.text = "" + Mathf.Floor(mpBar.value) + " / " + mpBar.maxValue;
    }

    //HPが0なら全てのバーの値を0にする
    void GameOver()
    {
        Mp = 0;
        backHpBar.value -= 20 * reduceSpead * Time.deltaTime;
        backMpBar.value -= 20 * reduceSpead * Time.deltaTime;
        mpBar.value -= 20 * reduceSpead * Time.deltaTime;
        Debug.Log("GameOver");
    }

    void Update ()
    {
        //HP・MP表示の値を変更
        hpText.text = "" + Mathf.Floor(hpBar.value) + " / " + hpBar.maxValue;
        mpText.text = "" + Mathf.Floor(mpBar.value) + " / " + mpBar.maxValue;

        //HPが0なら全てのバーの値を0にする
        if (Hp <= 0) GameOver();

		//HPが減った時、HPバーの値を減らす
		if (Hp < hpBar.value && hpBar.value >= 0)
		{
			hpTimer = 0;
			hpCount++;
            //HPを減らす
            hpBar.value = IncreaseAndDecrease(0, hpBar.value,Hp);
        }
        //HPとHPバーが一致したら後ろのバーを下げるまでの時間のカウントを開始
        else if (Hp == hpBar.value && hpCount >= 1)
        {
            hpCount = 0;
            hpTimeBool = true;
        }

        //回復したとき
        if (hpBar.value < Hp && hpBar.value < hpBar.maxValue)
		{
            hpBar.value = IncreaseAndDecrease(1,hpBar.value,Hp);

			//HP回復した時、後ろのバーも同時に回復させる
			if (backHpBar.value < hpBar.value)
			{
                //backHpBar.value += Mathf.Ceil((Hp - backHpBar.value) * reduceSpead);
                backHpBar.value = IncreaseAndDecrease(1,backHpBar.value,hpBar.value);
                hpTimeBool = false;
			}
		}

		

        //MPが減った時、MPバーの値を減らす
        if (Mp < mpBar.value && mpBar.value >= 0)
        {
			mpTimer = 0;
			mpCount++;
            //MPバー減少処理
			mpBar.value = IncreaseAndDecrease(0, mpBar.value, Mp);
        }
        //MPとMPバーが一致したら後ろのバーを下げるまでの時間のカウントを開始
        else if (Mp == mpBar.value && mpCount >= 1)
        {
            mpCount = 0;
            mpTimeBool = true;
        }

        //MPが回復したとき
        if (mpBar.value < Mp && mpBar.value < mpBar.maxValue)
        {
			mpBar.value = IncreaseAndDecrease(1, mpBar.value, Mp);

            //回復した時、後ろのバーも同時に回復させる
            if (backMpBar.value < mpBar.value)
			{
                backMpBar.value = IncreaseAndDecrease(1, backMpBar.value, mpBar.value);
                mpTimeBool = false;
			}
		}

        //後ろのHPバーを下げるまでの間隔
        if (hpTimeBool && Hp == hpBar.value || hpBar.value > backHpBar.value && !hpTimeBool)
        {
            hpTimer += Time.deltaTime;
            if (hpTimer >= waitSec)
            {
                hpTimer = 0;
                backHpTimeBool = true;
                hpTimeBool = false;
            }
        }

        if (backHpTimeBool)
        {
            backHpBar.value = IncreaseAndDecrease(0, backHpBar.value, hpBar.value);

            if (backHpBar.value <= hpBar.value)
            {
                backHpBar.value = hpBar.value;
                backHpTimeBool = false;
            }
        }

        //後ろのMPバーを下げるまでの間隔
        if ((mpTimeBool && Mp == mpBar.value) || (mpBar.value > backMpBar.value && !mpTimeBool))
        {
            mpTimer += Time.deltaTime;

            if (mpTimer >= waitSec)
            {
                //BackBool(backMpBar.value, mpBar.value,mpTimer);
                mpTimer = 0;
                backMpTimeBool = true;
                mpTimeBool = false;
            }
        }

        //Trueなら後ろのMPバーを減らし始める
        if (backMpTimeBool)
        {
            backMpBar.value = IncreaseAndDecrease(0, backMpBar.value, mpBar.value);

            if (backMpBar.value <= mpBar.value)
            {
                backMpBar.value = mpBar.value;
                backMpTimeBool = false;
            }
        }
    }

    //barValue 減らすバーの値
    //intValue 変更されたの値
    float IncreaseAndDecrease(int type,float barValue,float floatValue)
    {
        switch (type)
        {
            //減少処理
            case 0:
                barValue -= Mathf.CeilToInt((barValue - floatValue) * reduceSpead * Time.deltaTime);
                break;
            //増加処理
            case 1:
                barValue += Mathf.CeilToInt((floatValue - barValue) * reduceSpead * Time.deltaTime);
                break;
        }
        return barValue;
    }
}
