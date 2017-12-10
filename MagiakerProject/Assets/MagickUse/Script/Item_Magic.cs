using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作成者　東谷 太喜
/// </summary>
public class Item_Magic : MonoBehaviour
{
    public GameObject Character; //キャラクターの読み込み
	private PlayerController PC;

	public static Magick[] m_Magicks = new Magick[ConstData.MAGICK_COUNT];
    [SerializeField]
	static private int m_magicNo; //アイテムの現在の番号
    private float m_Interval; //ホイールの認識のインターバル
    private const float INTERVAL_TIME = 0.5f;

    void Start()
    {
		if (!PC) {
			PC = GetComponent<PlayerController> ();
			if(PC == null)Debug.LogAssertion("魔法使用スクリプトからプレイヤーを参照出来ませんでした");
		}

        ////魔法アイコンの取得 できればInspecter上で指定する方法に変えたい…＠富澤
        //for (int i = 0; i < m_MagickImages.Length; i++) {
        //    m_MagickImages[i] = GameObject.Find("Image" + (i + 1));
        //}
        ChangeMagic(m_magicNo);
    }

    // Update is called once per frame
    void Update()
    {
        //連続での認識を抑える。
        if (Time.time - m_Interval > INTERVAL_TIME)
        {

			if (Input.GetAxis("Mouse ScrollWheel") >= 0.1f || Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_Interval = Time.time;
				BackItem();
            }

			if (Input.GetAxis("Mouse ScrollWheel") <= -0.1f || Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_Interval = Time.time;
				NextItem();
            }
        }


        ////魔法の使用＠富澤2017/11/15
        //if (Input.GetMouseButtonDown(0)) {
        //    m_Magicks[m_magicNo].Enter(Character);
        //}
    }

    private void NextItem()
    {
		m_magicNo = m_magicNo + 1 >= ConstData.MAGICK_COUNT ? 0 : m_magicNo + 1;

        //Debug.Log(m_magicNo);

        ChangeMagic(m_magicNo);
    }

    private void BackItem()
    {
		m_magicNo = m_magicNo - 1 < 0 ? ConstData.MAGICK_COUNT - 1 : m_magicNo - 1;

        //Debug.Log(m_magicNo);

        ChangeMagic(m_magicNo);
    }

    private void ChangeMagic(int num)
    {
        StatesUI.statesUI.MagickSelect(num);
    }

	/// <summary>
	/// 選択中の魔法の使用
	/// </summary>
	public void UseMagick(){
		UseMagick (m_magicNo);
	}

	/// <summary>
	/// 魔法の使用
	/// </summary>
	/// <param name="num">使用する魔法の番号</param>
	public void UseMagick(int num){
		if (m_Magicks [num] != null) {
			if (PC.MP >= m_Magicks [num].GetMP) {
				m_Magicks [num].Enter (Character);
				PC.MP -= m_Magicks [num].GetMP;
			}
		}
	}
}
