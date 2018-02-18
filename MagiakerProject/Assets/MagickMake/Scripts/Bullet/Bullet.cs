using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 作成者　富澤勇太
/// </summary>

//弾の種類
public enum BulletType {
    normal,//通常弾
    explosion,//爆発
    homing,//追跡
    paint,//ダメージ床
    fixe,//発射者追尾
    beam,//ビーム
    homingExplosion,//追跡爆発
}

public class Bullet : MonoBehaviour {
    public string BulletName;
    public string GetBulletName() { return abnState ? BulletName + "(" + abnState.GetTypeName() + ")" : BulletName; }

	private GameObject prefab,bulletObjPrefab;
    public GameObject bulletObj;//子の弾丸オブジェクト
	public AbnState abnState;
	public Character parent;
    private Vector3 defautPosition;//弾道の基準点
    [SerializeField]
    public Vector3[] movePath;//ListにするとITweenに渡す時に変換が必要なので配列で確保

    [SerializeField]
    public float moveTime;//移動時間
    [SerializeField]
    public iTween.EaseType type = iTween.EaseType.linear;
    [SerializeField]
    public BulletType bulletType;
    [SerializeField]
    public int useMP = 1,damage = 2,BombDamage;//使用するMP量、ダメージ量、爆発のダメージ

    [SerializeField]
    private Bullet childBullet;//この弾道に連続して起動される弾道
    private Bullet parentBullet;//この弾道の前に起動される弾道
    public Bullet GetChildBullet() {return childBullet;}
    public void SetChildBullet(Bullet bul) { childBullet = bul; }
    public Bullet GetParentBullet() {return parentBullet;}
    public int GetFamiryCount() { return 1 + (GetChildBullet() ? childBullet.GetFamiryCount() : 0); }

    [SerializeField]
    private Bullet pairBullet;//同時に発射する弾道

    [SerializeField]
    public Sprite Icon;//UIとして表示する際に使用する画像

    [SerializeField]
    private BulletManager Manager;

    private bool isTest = false;//テストで発射しているか否か　trueなら弾が止まったら元の場所に戻る

    void Start() {
        SetDefaultPosition();
		InitBulletObj ();

        foreach (GameObject obj in lines) {
            if (!obj) { continue; }
            Vector3 pos = obj.transform.localPosition;
            pos.y = 0;
            obj.transform.localPosition = pos;
        }
    }

	private void InitBulletObj(){
		if (!bulletObj.GetComponent<BulletObject>())
			bulletObj.AddComponent<BulletObject>();
	}

    /// <summary>
    /// 各種変数・子オブジェクトの初期化
    /// </summary>
    public void Init() {
        movePath = new Vector3[1];
        useMP = 0;
        Icon = null;
        SetDefaultPosition();
        DeleteDrawLine();

        if (pairBullet) { pairBullet.Init(); }
    }

    /// <summary>
    /// 弾丸基準点の再設定
    /// </summary>
    public void SetDefaultPosition() {
        if (bulletObj) {
            defautPosition = bulletObj.transform.localPosition;
        }
    }

	public Vector3 GetDefaultPos(){
		return defautPosition;
	}

	public void SetPrefab(GameObject prefab){
		if (this.prefab == null) {
			this.prefab = prefab;
		}
	}

	public GameObject GetPrefab(){
		return prefab;
	}
    
    /// <summary>
    /// 弾丸を発射する
    /// </summary>
    public void Enter(bool isTestScene = false) {

        isTest = isTestScene;
        
        if (abnState == null)
        {
            bulletObj = Instantiate(prefab.GetComponent<Bullet>().bulletObj, transform);
            if (!bulletObj.GetComponent<BulletObject>())
            {
                BulletObject bulObj = bulletObj.AddComponent<BulletObject>();
                bulObj.parent = this;
            }
        }
        else
        {
            ChangeElement(abnState);
        }

        //弾道を自身の子にする
        //bulletObj.transform.SetParent(transform);
        //bulletObj.transform.localPosition = GetDefaultPos ();
        ReturnToDefaultPos();

        switch (bulletType) {
            case BulletType.beam:
                StartCoroutine(WaitShotEndTime(moveTime));
                break;
            case BulletType.fixe:
                StartCoroutine(WaitShotEndTime(moveTime));
                //StartCoroutine(WaitShotEndTime(moveTime));
                RotateBullet rotate = bulletObj.AddComponent<RotateBullet>();
                rotate.SetTarget(GameObject.FindGameObjectWithTag(Tags.Player));
                rotate.speed = 600;
                rotate.SetTime(moveTime);
                break;
            default:
                Hashtable hash = new Hashtable();
                switch (movePath.GetLength(0))
                {
                    case 0:
                        Debug.LogAssertion("経路が設定されていません");
                        return;
                    case 1:
                        hash.Add("position", transform.position + Rotate(movePath[0]));
                        break;
                    default:
                        Vector3[] path = new Vector3[movePath.Length];
                        for (int i = 0; i < path.GetLength(0); i++)
                        {
                            path[i] = transform.position + Rotate(movePath[i]);
                        }
                        hash.Add("path", path);
                        break;
                }
                hash.Add("time", moveTime);
                hash.Add("easetype", type);
                hash.Add("oncompletetarget", gameObject);
                hash.Add("oncomplete", "MoveEnd");

                iTween.MoveTo(bulletObj, hash);

                //弾道を親から外す
                bulletObj.transform.parent = null;
                break;
        }

        switch (bulletType) {
            case BulletType.beam:
                break;
            default:
                //経路予測線を消去
                DeleteDrawLine();

                //if (!isDraw)
                if (isTest)
                    StartCoroutine(MovePathDraw());
                break;
        }
        

        bulletObj.GetComponent<BulletObject>().ShotStart();

        if (pairBullet) {
            pairBullet.Enter(isTestScene);
        }
    }
    
    /// <summary>
    /// このオブジェクトの向きを加味した相対位置を返す
    /// </summary>
    /// <param name="pos">このオブジェクトからの距離</param>
    /// <returns></returns>
    public Vector3 Rotate(Vector3 pos) {
        Vector3 x = pos.x * new Vector3(transform.forward.z, 0, -transform.forward.x);//x
        Vector3 z = pos.z * new Vector3(transform.forward.x, 0, transform.forward.z);//z
        return x + z;
    }

    const float drawFrame = 0f;//経路を描画する際の描画頻度
    [SerializeField]
    private GameObject drawLine;//経路の描画に用いる２Dオブジェクトプレファブ
    [SerializeField]
    List<GameObject> lines = new List<GameObject>();//生成した経路オブジェクトのリスト
    public bool isDraw = false;//弾丸の発射時に経路を描画するか

    /// <summary>
    /// 経路の描画　デフォルトの描画頻度は0.1f　停止はMoveEnd()
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovePathDraw(float draw = drawFrame) {
        isDraw = true;
		while (true && bulletObj != null) {
            GameObject line = Instantiate(drawLine);
            line.transform.position = bulletObj.transform.position;
            Vector3 rotate = bulletObj.transform.eulerAngles;
            rotate.x = 90;
            line.transform.eulerAngles = rotate;
            line.transform.parent = transform;
            lines.Add(line);
			yield return new WaitForSeconds(drawFrame);
        }
    }

    /// <summary>
    /// 自身と自身の子・孫・曾孫…全てのBulletをListにして返す
    /// </summary>
    /// <returns></returns>
    private List<Bullet> GetAllBullet() {
        List<Bullet> famiry = new List<Bullet>();
        Bullet bul = this;
        do { famiry.Add(bul); } while ((bul = bul.childBullet));
        return famiry;
    }

    /// <summary>
    /// 描画した経路を削除・初期化する
    /// </summary>
    public void DeleteDrawLine() {
        foreach (GameObject line in lines)
            Destroy(line);
        lines = new List<GameObject>();
        if (childBullet) childBullet.DeleteDrawLine();
    }

    /// <summary>
    /// 弾道の持続時間待機し、その後に移動終了処理を呼ぶ
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator WaitShotEndTime(float time) {
        yield return new WaitForSeconds(time);
        MoveEnd();
    }

    /// <summary>
    /// 弾丸の移動が終了した際に呼ばれる処理
    /// </summary>
    public void MoveEnd() {
        StopAllCoroutines();

        //弾道に続きがあるなら次の弾道を起動
        if (childBullet) {
            childBullet.bulletObj = bulletObj;
            childBullet.SetDefaultPosition();
            childBullet.Enter(isTest);
        }
        //テスト用のシーンなら元の場所に戻る
        else if (isTest)
        {
            if (bulletObj)
            {
                BulletObject bul = bulletObj.GetComponent<BulletObject>();
                if (bul && bul.bulletType == BulletType.explosion || bul.bulletType == BulletType.homingExplosion)
                {
                    bul.HitExplosion();
                }
            }

            //ReturnToDefaultPos();
            Destroy(bulletObj);
            Debug.Log(prefab);
            bulletObj = Instantiate(prefab.GetComponent<Bullet>().bulletObj);
            ChangeElement(abnState);
            ReturnToDefaultPos();
        }
        else {
            if (bulletObj) {
                BulletObject bul = bulletObj.GetComponent<BulletObject>();
                if (bul && bul.bulletType == BulletType.explosion || bul.bulletType == BulletType.homingExplosion) {
                    bul.HitExplosion();
                }
                Destroy(bulletObj);
            }
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 弾丸の移動を終了させる処理
    /// </summary>
    public void MoveStop() {
        //StopAllCoroutines();
        if (bulletObj)
            iTween.Stop(bulletObj);
    }

    /// <summary>
    /// 弾丸を発射前の位置に戻す
    /// </summary>
    public void ReturnToDefaultPos() {
        if (parentBullet) {
            parentBullet.ReturnToDefaultPos();
        }
        else {
            bulletObj.transform.SetParent(transform);
            bulletObj.transform.localPosition = GetDefaultPos();
        }
			
    }

    /// <summary>
    /// このオブジェクトをプレファブとして「Assets/Resources/Prefabs/」に保存
    /// 既に同じ名前のプレファブがある場合、上書きされることに注意
    /// </summary>
    public void CreatePrefab() {
        Manager.Save(this);
    }

    /// <summary>
    /// 移動経路の追加
    /// </summary>
    /// <param name="pos">追加する経路の位置</param>
    public void AddPath(Vector3 pos) {
        Vector3[] newPath = new Vector3[movePath.GetLength(0) + 1];
        for (int i = 0; i < movePath.GetLength(0); i++)
            newPath[i] = movePath[i];
        newPath[newPath.GetLength(0) - 1] = pos;

        movePath = newPath;
    }

    /// <summary>
    /// 移動経路一つの削除
    /// </summary>
    /// <param name="num">削除する経路の番号</param>
    public void RemovePath(int num) {
        Vector3[] newPath = new Vector3[movePath.GetLength(0) - 1];

        int length = movePath.GetLength(0);
        for (int i = 0,j = 0;i < length; i++) {
            if (i == num) continue;
            newPath[j++] = movePath[i];
        }
        movePath = newPath;
    }

    /// <summary>
    /// 移動経路の番号aと番号bを入れ替える
    /// </summary>
    /// <param name="a">一つ目の番号</param>
    /// <param name="b">二つ目の番号</param>
    public void ReplacePath(int a,int b) {
        try
        {
            Vector3 aPos = movePath[a];
            movePath[a] = movePath[b];
            movePath[b] = aPos;
        }
        catch {
            Debug.LogError("Bullet.ReplacePath()で例外が発生");
        }
    }

    /// <summary>
    /// 経路全ての削除
    /// </summary>
    public void RemoveAllPath() {
        movePath = new Vector3[0];
    }

    /// <summary>
    /// 設定中のiTween.EaseTypeの変更
    /// </summary>
    /// <param name="type"></param>
    public void ChangeEaseType(iTween.EaseType type) {
        this.type = type;
    }

    /// <summary>
    /// 設定中の移動時間の変更
    /// </summary>
    /// <param name="time"></param>
    public void ChangeMoveTime(float time) {
        moveTime = time;
    }

    /// <summary>
    /// 子の弾道を生成する
    /// </summary>
    public void AddChildBullet() {
        //既に子がいるなら子の下に子を生成
        if (childBullet)
            childBullet.AddChildBullet();
        else {
            childBullet = Instantiate(gameObject).GetComponent<Bullet>();
            childBullet.SetParentBullet(this);
        }
    }

    /// <summary>
    /// 自身の親に他Bulletを設定 
    /// </summary>
    /// <param name="bul">親となるBullet　nullなら独立したBulletになる</param>
    /// <param name="isInit">生成時の設定か</param>
    public void SetParentBullet(Bullet bul,bool isInit = true) {
        if (bul) {
            transform.SetParent(bul.transform);
            parentBullet = bul;
            if (parentBullet.bulletObj != bulletObj)
                Destroy(bulletObj);
            bulletObj = bul.bulletObj;
            //もし親の子に自身が設定されていなければ、自身を子に設定する
            if (parentBullet.GetChildBullet() != this)
                parentBullet.SetChildBullet(this);
        }
        else {
            transform.parent = null;
        }

        if (isInit)
            Init();
    }

    /// <summary>
    /// 弾丸のアクティブ・非アクティブ切り替え
    /// </summary>
    /// <param name="value"></param>
    public void SetBulletObjEnabled(bool value) {
		if (bulletObj)
			bulletObj.GetComponent<Collider> ().enabled = value;
    }

	public AbnState GetAbnState(){
		//return bulletObj.GetComponent<BulletObject> ().element;
		return abnState;
	}

	/// <summary>
	/// 属性の変更
	/// </summary>
	/// <param name="type">変更先の属性</param>
	public void ChangeElement(element type){
		BulletObject bulObj = bulletObj.GetComponent<BulletObject> ();
		AbnState state = null;
		if (bulObj) {
			if (bulObj.elementType == type)
				return;
			state = MagicSystemManager.instance._abnstateManager.GetElement (bulObj.elementType, bulObj.element);
		} 
		if (state == null)
			state = MagicSystemManager.instance._abnstateManager.GetElement (MagickMakeManager.Instance.selectElement);
		ChangeElement (state);
	}

	public void ChangeElement(AbnState state){
		Destroy (bulletObj);
        GameObject prefab = MagicSystemManager.instance._abnstateManager.GetElementPrefab(state.type,bulletType);
        bulletObj = Instantiate (prefab, transform);
        //bulletObj.transform.localPosition = defautPosition;
        bulletObj.transform.localPosition = GetDefaultPos();
        bulletObj.transform.localEulerAngles = Vector3.zero;
        InitBulletObj ();
		BulletObject bulObj = bulletObj.GetComponent<BulletObject> ();
		bulObj.element = state;
		abnState = bulObj.element;
		bulObj.parent = this;

        if (pairBullet) {
            pairBullet.ChangeElement(state);
        }
	}

#if UNITY_EDITOR
    [SerializeField]
    public bool x, z;

    [ContextMenu("MirrorBullet")]
    public void MirrorBullet() {
        if (!pairBullet) {
            pairBullet = Instantiate(gameObject).GetComponent<Bullet>();
            for (int i = 0; i < pairBullet.movePath.GetLength(0); i++) {
                //x,z がtrueならそれを反転させる
                pairBullet.movePath[i] = new Vector3(x ? -pairBullet.movePath[i].x : pairBullet.movePath[i].x, pairBullet.movePath[i].y, z ? -pairBullet.movePath[i].z : pairBullet.movePath[i].z);
                pairBullet.transform.SetParent(transform);
                pairBullet.transform.localPosition = Vector3.zero;
                pairBullet.transform.localEulerAngles = Vector3.zero;
            }
        }
    }
#endif
}