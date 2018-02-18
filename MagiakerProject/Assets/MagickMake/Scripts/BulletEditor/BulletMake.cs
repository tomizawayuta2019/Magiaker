using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作成者　富澤勇太
/// </summary>
public class BulletMake : MonoBehaviour {
    [SerializeField]
    private Bullet defaultBullet;//このシーンを開いた時に設定されるデフォルトの弾道
    [SerializeField]
    public Bullet bullet;//現在編集中の弾道

    private bool isSaveCheck, isLoadCheck;//データ保存・破棄確認ウィンドウ
    private GameObject loadBullet;//データを読み込む弾道
    [SerializeField]
    private bool isBulletListOpen;//弾丸種類ウィンドウ

    private List<BulletGUI> BulletsGUI = new List<BulletGUI>();//弾道の経路ごとのGUI用データ

    [SerializeField]
    public GUIStyle style;//GUIラベル用のスタイル　デフォルトだと見難かったので変更

    [SerializeField]
    private BulletManager manager;//弾道のデータを格納しているScriptableObject

    const float LINE_SIZE_Y = 20, //GUIの縦幅(横幅は文字数による)
                GUI_OFFSET_X = 1f, GUI_OFFSET_Y = 1f,//GUIのoffset
                DEFAULT_POSITION_X = 20, DEFAULT_POSITION_Y = 10;//GUIの配置開始位置

    /// <summary>
    /// 弾道の軌道ごとに管理する変数
    /// </summary>
    public class BulletGUI {
        /// <summary>
        /// 入力されたデータがfloatに変換できなかった場合の仮置き用構造体　（0.1などを入力したい場合、[0.]まで入力すると[0]になってしまうので[0.]を文字列で保持しておく）
        /// </summary>
        public struct String2
        {
            public String2(Vector3 pos) {
                x = pos.x.ToString();
                y = pos.y.ToString();
            }
            public string x;
            public string y;
        }
        public List<String2> sPos = new List<String2>();//配列に変更すること（後で）
        public string sTime = "1";
        public bool isDateOpen = true,//データは開いた状態が基本
                    isEaseTimeOpen = false;//EaseTime設定は最初は閉じている
        public Bullet bul;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="bul">読み込む弾道</param>
        public BulletGUI(Bullet bul) {
            this.bul = bul;
            foreach (Vector3 path in bul.movePath) {
                sPos.Add(new String2(path));
            }
            //sPos.x = "0";sPos.y = "0";
        }
    }

    // Use this for initialization
    void Start () {
        if (!bullet) bullet = defaultBullet;

        if (manager == null) return;
        Debug.Log("現在保存されているBulletの数は" + manager.Bullets.Count + "です");
	}
    
    /// <summary>
    /// 各GUIの描画
    /// </summary>
    void OnGUI() {
#if UNITY_EDITOR
        //GUI配置基準点設定
        Vector2 pos = new Vector2(DEFAULT_POSITION_X, DEFAULT_POSITION_Y);

        GUILabel_refPosition(ref pos, 35, 20, "名称");
        bullet.name = GUITextField_refPosition(ref pos, 100, 20, bullet.name);

        if (GUIButton_refPosition(ref pos, 40, 20, "保存")) {
            if (manager.isBulletExist(bullet.name) >= 0)
                isSaveCheck = true;
            else
                bullet.CreatePrefab();
        }

        if (GUIButton_refPosition(ref pos, 70, 20, "読み込み")) {
            if ((loadBullet = manager.Load(bullet.name)))
            {
                isLoadCheck = true;
                Debug.Log("プレファブを発見しました");
            }
            else if (!loadBullet)
                Debug.Log(bullet.name + "プレファブが見つかりませんでした");
        }

        //データ読み込み確認
        if (isLoadCheck) {
            GUILabel_refPosition(ref pos, 400, 20, "現在作成中のデータを破棄し、他のデータを読み込みますか？");
            pos = GetNewLinePosition(pos);
            if(GUIButton_refPosition(ref pos,35,20,"はい")) {
                isLoadCheck = false;
                Destroy(bullet.gameObject);
                bullet = Instantiate(loadBullet.gameObject).GetComponent<Bullet>();
            }
            if(GUIButton_refPosition(ref pos,50,20,"いいえ"))
                isLoadCheck = false;
        }
        if(GUIButton_refPosition(ref pos,140,20,"新しい弾道を作成")) {
            loadBullet = defaultBullet.gameObject;
            isLoadCheck = true;
        }

        pos = GetNewLinePosition(pos);//改行

        if (GUIButton_refPosition(ref pos,70,20,"発射"))
            bullet.Enter(true);

        GUILabel_refPosition(ref pos, 50, 20, "消費MP");
        int.TryParse(GUITextField_refPosition(ref pos, 35, 20, bullet.useMP.ToString()), out bullet.useMP);

        pos = AllBulletsDateGUI(pos);

        pos = GetNewLinePosition(pos);//改行
        
        if(GUIButton_refPosition(ref pos,140,20,"次の弾道を設定"))
            bullet.AddChildBullet();

        pos = GetNewLinePosition(pos);//改行

        //弾道の種類を設定
        if (GUIButton_refPosition(ref pos,140,20, bullet.bulletType.ToString()))
            isBulletListOpen = !isBulletListOpen;

        //弾道リストの表示
        if (isBulletListOpen) {
            BulletType? type = EnumButtonListGUI<BulletType>(ref pos, 110, 20, 3);
            if (type.HasValue) {
                bullet.bulletType = type.Value;
                isBulletListOpen = false;
            }
        }

        pos = GetNewLinePosition(pos);

        //データ保存確認
        if (isSaveCheck) {
            GUILabel_refPosition(ref pos, 360, 20, "既に同じ名前のプレファブが存在します。上書きしますか？");
            if (GUIButton_refPosition(ref pos, 50, 20, "はい")) {
                isSaveCheck = false;
                bullet.CreatePrefab();
            }
            else if (GUIButton_refPosition(ref pos, 50, 20, "いいえ")) {
                isSaveCheck = false;
            }
        }
#endif
    }

    /// <summary>
    /// 弾道のデータ表示（移動時間・経路・速度オプション）
    /// </summary>
    /// <param name="posx">基準点ｘ</param>
    /// <param name="posy">基準点ｙ</param>
    /// <returns>縦軸でどこまで描画したか</returns>
    private Vector2 BulletDateGUI(BulletGUI date,Vector2 pos) {

        if (GUIButton_refPosition(ref pos, 110, 20, date.isDateOpen ? "データを閉じる" : "データを開く"))
            date.isDateOpen = !date.isDateOpen;
        if (GUIButton_refPosition(ref pos, 40, 20, "消去"))
        {
            //現在選択中のBulletを削除し、自身に子があるならそれを親の子にする
            Bullet child = date.bul.GetChildBullet();
            Bullet parent = date.bul.GetParentBullet();

            ///1.自身以外がない場合　自身を初期化する
            ///2.自身以外がいる場合　自身を削除する
            ///2.1.親がいる場合　自身を削除する
            ///2.1.1.子がいる場合　子を親につける
            ///2.2.親がおらず子がいる場合　子を独立させる
            if (!parent && !child) date.bul.Init();
            else {
                if (parent && child) {
                    child.SetParentBullet(parent, false);
                    child.transform.SetParent(parent.transform);
                }
                else if (child) {
                    //自身の子が存在し親がいない（自身が一番親）とき、子を独立させる
                    child.SetParentBullet(null, false);
                    bullet = child;
                    //名前を親と同じにする（子は(Clone)が付いているため）
                    bullet.name = date.bul.name;
                }

                Destroy(date.bul.gameObject);
                return pos;//これ以上の描画は必要無いため抜ける
            }
        }

        if (!date.isDateOpen) return pos;//データが閉じられているなら終了

        ////

        GUILabel_refPosition(ref pos, 60, 20, "移動時間");
        //string s = GUITextField_refPosition(ref pos, 20, 20, date.bul.moveTime.ToString());
        date.sTime = GUITextField_refPosition(ref pos, 20, 20, date.sTime);
        float parse;
        if (float.TryParse(date.sTime, out parse)) date.bul.ChangeMoveTime(parse);

        pos = GetNewLinePosition(pos);

        GUILabel_refPosition(ref pos, 140, 20, "現在設定中の経路");
        if(GUIButton_refPosition(ref pos,120,20, "新しい経路を追加"))
            date.bul.AddPath(Vector3.zero);
        for (int i = 0; i < date.bul.movePath.GetLength(0); i++) {
            
            pos = GetNewLinePosition(pos);

            //座標表示
            var sPos = new BulletGUI.String2();//List内の構造体を書き換えられない為、一度新しく宣言
            if (date.sPos.Count <= i)
                date.sPos.Add(new BulletGUI.String2(date.bul.movePath[i]));

            //x座標表示
            GUILabel_refPosition(ref pos, 10, 20, "x");
            //string s = GUITextField_refPosition(ref pos, 35, 20, date.bul.movePath[i].x.ToString());
            sPos.x = GUITextField_refPosition(ref pos, 35, 20, date.sPos[i].x);
            if (float.TryParse(date.sPos[i].x, out parse))
                date.bul.movePath[i].x = parse;
            //z座標表示
            GUILabel_refPosition(ref pos, 10, 20, "z");
            sPos.y = GUITextField_refPosition(ref pos, 35, 20, date.sPos[i].y);
            if (float.TryParse(date.sPos[i].y, out parse))
                date.bul.movePath[i].z = parse;

            //座標代入 Listが足りなかったら最後尾に追加
            date.sPos[i] = sPos;

            if (GUIButton_refPosition(ref pos,75,20, "一段上げる") && i != 0)
                date.bul.ReplacePath(i, i - 1);
            if (GUIButton_refPosition(ref pos,75,20, "一段下げる") && i != date.bul.movePath.GetLength(0) - 1)
                date.bul.ReplacePath(i, i + 1);
            if (GUIButton_refPosition(ref pos, 75, 20, "削除"))
                date.bul.RemovePath(i);
        }

        pos = GetNewLinePosition(pos);

        GUILabel_refPosition(ref pos, 120, 20, "速度オプション");
        if (GUIButton_refPosition(ref pos ,120,20, date.bul.type.ToString()))
            date.isEaseTimeOpen = !date.isEaseTimeOpen;

        //EaseTime設定ウィンドウを開く
        if (date.isEaseTimeOpen) {
            //EaseTypeの一覧をボタンで表示・押されたボタンの要素を取得する
            iTween.EaseType? type = EnumButtonListGUI<iTween.EaseType>(ref pos, 110, 20, 4);

            //ボタンが押されたならそのボタンの要素を代入してEaseTime設定ウィンドウを閉じる
            if (type.HasValue) {
                date.isEaseTimeOpen = false;
                date.bul.type = type.Value;
            }
        }

        return pos;
    }

    /// <summary>
    /// Enumの要素を一つずつボタンにして表示する処理
    /// </summary>
    /// <typeparam name="T">表示したいEnum</typeparam>
    /// <param name="pos">ボタン配置の基準位置</param>
    /// <param name="x">ボタン横幅</param>
    /// <param name="y">ボタン縦幅</param>
    /// <param name="width">ボタン横配置数</param>
    /// <returns>押されたボタン　何も押されなかったらnullを返す</returns>
    public T? EnumButtonListGUI<T>(ref Vector2 pos, float x,float y,int width = 4) where T:struct {
        //Enumの一覧をボタンで表示・押されたらそのEnumをreturnする
        int length = System.Enum.GetValues(typeof(T)).Length;
        for (int i = 0; i < length; i++) {
            if (i % width == 0) pos = GetNewLinePosition(pos);
            if (GUIButton_refPosition(ref pos, x, y, System.Enum.GetName(typeof(T), i)))
                return (T)System.Enum.ToObject(typeof(T), i);
        }

        //何も押されなかったらnullを返す
        return null;
    }

    /// <summary>
    /// GUIデータの取得・更新
    /// </summary>
    public void InitBulletsGUIDates() {
        BulletsGUI = new List<BulletGUI>();

        if (!bullet) return;

        Bullet bul = bullet;
        do {
            BulletsGUI.Add(new BulletGUI(bul));
        } while ((bul = bul.GetChildBullet()));//子が存在するなら、それを新しい対象としてもう一度
    }

    /// <summary>
    /// 現在設定中の経路すべての情報を表示
    /// </summary>
    /// <param name="pos">GUI配置位置</param>
    /// <returns>GUI配置終点位置</returns>
    public Vector2 AllBulletsDateGUI(Vector2 pos) {
        ///現在のBulletとその子要素のBulletの情報を表示
        //Bullet bul = bullet;
        if (bullet.GetFamiryCount() != BulletsGUI.Count)
            InitBulletsGUIDates();

        //ループ内でBulletGUIの要素数が変わる事がある（データ削除ボタン）のでforeachではなくforを使用
        for (int i = 0; i < BulletsGUI.Count; i++) {
            pos = GetNewLinePosition(pos);//改行
            pos = BulletDateGUI(BulletsGUI[i], pos);
        }

        return pos;
    }

    /// <summary>
    /// テキストフィールドを配置し、そのサイズに応じて次の配置場所を返す処理
    /// </summary>
    /// <param name="pos">配置する場所　ref 次の配置場所</param>
    /// <param name="x">横幅</param>
    /// <param name="y">縦幅</param>
    /// <param name="value">GUIに入れるtext</param>
    private string GUITextField_refPosition(ref Vector2 pos,float x,float y, string value)
    {
        string str = GUI.TextField(new Rect(pos.x, pos.y, x, y), value);
        pos.x += x + GUI_OFFSET_X;
        return str;
    }
    
    /// <summary>
    /// ラベルを配置し、そのサイズに応じて次の配置場所を返す処理
    /// </summary>
    /// <param name="pos">配置する場所　ref 次の配置場所</param>
    /// <param name="x">横幅</param>
    /// <param name="y">縦幅</param>
    /// <param name="value">GUIに入れるtext</param>
    private void GUILabel_refPosition(ref Vector2 pos, float x,float y, string value)
    {
        GUI.Label(new Rect(pos.x, pos.y, x, y), value, style);
        pos.x += x + GUI_OFFSET_X;
    }

    /// <summary>
    /// ボタンを配置し、そのサイズに応じて次の配置場所を返す処理
    /// </summary>
    /// <param name="pos">配置する場所　ref 次の配置場所</param>
    /// <param name="x">横幅</param>
    /// <param name="y">縦幅</param>
    /// <param name="value">GUIに入れるtext</param>
    /// <returns>ボタンが押されたか</returns>
    private bool GUIButton_refPosition(ref Vector2 pos,float x,float y,string value) {
        bool isPush = (GUI.Button(new Rect(pos.x, pos.y, x, y), value));
        pos.x += x + GUI_OFFSET_X;
        return isPush;
    }

    /// <summary>
    /// GUI配置位置の新しい行の開始位置を取得
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector2 GetNewLinePosition(Vector2 pos) {
        pos.x = DEFAULT_POSITION_X;
        pos.y += LINE_SIZE_Y + GUI_OFFSET_Y;//縦方向にGUIが重ならないように規定の縦幅に余剰幅を設定
        return pos;
    }
}
