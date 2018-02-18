using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//魔法で発射される玉オブジェクトコンポーネント
public class BulletObject : AttackArea {
    //public float damage;
	public element elementType { get { return element.type; } }
    public BulletType bulletType;
    public Bullet parent;
    [HideInInspector]
    public bool isActive = false;//攻撃が当たるタイミングか否か

    private float bombDamage;
    private bool isBomb;

    public new void Start()
    {
        Collider col = GetComponent<Collider>();
        if (!col) return;
        GetComponent<Collider>().isTrigger = true;

        Rigidbody rig = GetComponent<Rigidbody>();
        if (!rig) rig = gameObject.AddComponent<Rigidbody>();
        rig.useGravity = false;

        if (parent == null)
            parent = transform.parent.GetComponent<Bullet>();

        bulletType = parent.bulletType;

		//AttackArea用の初期設定
		gameObject.tag = Tags.Magic;
		aligment = aligment.player;
        character = parent.parent;

        //爆弾として生成された場合の処理
        if (isBomb)
        {
            //持続ダメージの発生はビームの処理を流用中
            bulletType = BulletType.beam;
            Damage = parent.BombDamage;
        }
        else {
            Damage = parent.damage;
        }
        bombDamage = parent.BombDamage;

		base.Start ();

        _pos = _nowPos = transform.position;
        //transform.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 弾が発射された際の処理
    /// </summary>
    public void ShotStart() {
        bulletType = parent.bulletType;
        //弾道の種別による効果設定
        AddBulletEffect(bulletType);

        isActive = true;
        switch (bulletType) {
            case BulletType.beam:
                SEManager.SetSE(MagicSystemManager.instance.SEManager.elementBeam.GetValue(elementType),gameObject);
                break;
            case BulletType.homingExplosion:
            case BulletType.explosion:
                //後に爆発するが今は直線に飛ぶ弾の場合
                if (!isBomb) {
                    //通常のSEを鳴らす
                    SEManager.SetSE(MagicSystemManager.instance.SEManager.elementShot.GetValue(elementType), gameObject);
                }
                break;
            default:
                SEManager.SetSE(MagicSystemManager.instance.SEManager.elementShot.GetValue(elementType),gameObject);
                break;
        }
    }

    private Vector3 _pos,_nowPos;
    public Vector3 Velocity {
        get {
            return _nowPos - _pos;
        }
    }
    protected void Update()
    {
        _pos = _nowPos;
        _nowPos = transform.position;
        switch (bulletType) {
            case BulletType.beam:
                break;
            default:
                transform.LookAt(transform.position + Velocity);
                break;
        }
    }

    public void ShotEnd() {
        Destroy(gameObject);
    }

    /// <summary>
    /// 属性の変更
    /// </summary>
    /// <param name="value"></param>
	public void ChangeElementType(AbnState value) {
		element = value;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Hit(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        Hit(other);
    }

    struct HitData {
        public GameObject target;
        public float hitTIme;
        //public int hitCount;
    }

    private List<HitData> hitDatas = new List<HitData>();
    /// <summary>
    /// ビーム等の連続ダメージの待機時間を確認する処理
    /// </summary>
    /// <param name="target">攻撃対象</param>
    /// <returns>ダメージが発生するか否か</returns>
    private bool IsBeamHIt(GameObject target) {
        bool isHIt = false;
        foreach (var item in hitDatas.Select((v, i) => new { v, i }))
        {
            if (item.v.target != target)
            {
                continue;
            }
            if (item.v.hitTIme + ConstData.BEAM_DAMAGE_DELAY > Time.time)
            {
                return false;
            }
            HitData data = item.v;
            data.hitTIme = Time.time;
            hitDatas[item.i] = data;
            return true;
        }
        if (!isHIt)
        {
            hitDatas.Add(new HitData { target = target, hitTIme = Time.time });
            return true;
        }

        return false;
    }

    /// <summary>
    /// 対象にダメージ等の情報をコピーし、爆発の状態にする
    /// </summary>
    /// <param name="target"></param>
    public void SetBomb(BulletObject target) {
        ////爆発は持続ダメージなので、ビームの処理を流用する
        //target.bulletType = BulletType.beam;
        ////親のタイプを参照してる部分があるので親も変える　あとで変えなくてもすむように直したい
        //parent.bulletType = BulletType.beam;
        target.isBomb = true;

        //爆発の初期化
        target.element = element;
        target.parent = parent;
        target.isActive = true;
        target.ShotStart();

        target.transform.position = transform.position;
        target.transform.eulerAngles = transform.eulerAngles;

        //SEManager.SetSE(MagicSystemManager.instance.SEManager.elementBomb.GetValue(elementType));

        target.StartCoroutine(BombStart(ConstData.BOMB_END_DELAY,target.gameObject));
    }

    private IEnumerator BombStart(float time,GameObject target) {
        SEManager.SetSE(MagicSystemManager.instance.SEManager.elementBomb.GetValue(elementType),target);
        yield return new WaitForSeconds(time);
        Destroy(target);
    }

    private void Hit(Collider other) {
        if (!isActive) return;
        Character target = other.gameObject.GetComponent<Character>();
        if (!target || target.isAligment(aligment)) {
            return;
        }
        switch (bulletType) {
            case BulletType.beam:
                DestroyCheck = false;
                Damage = parent.BombDamage;
                if (!IsBeamHIt(other.gameObject)) {
                    return;
                }
                break;
            case BulletType.explosion:
            case BulletType.homingExplosion:
                if (!isActive) return;
                base.OnTriggerEnter(other);
                HitExplosion();
                isActive = false;
                return;
        }
        base.OnTriggerEnter(other);
    }

    public void HitExplosion() {
        GameObject bomb = Instantiate(MagicSystemManager.instance._abnstateManager.GetBombPrefab(elementType));
        BulletObject bombBul = bomb.AddComponent<BulletObject>();
        SetBomb(bombBul);
        Destroy(gameObject);
    }

    private void AddBulletEffect(BulletType type) {
        switch (type)
        {
            case BulletType.normal:
                break;
            case BulletType.homingExplosion:
                bulletType = BulletType.explosion;
                SetHoming();
                break;
            case BulletType.homing:
                SetHoming();
                break;
            default:
                break;
        }
    }

    private void SetHoming() {
        Homing homing = GetComponent<Homing>() ?? gameObject.AddComponent<Homing>();
        //感知範囲に入るまで起動しない
        homing.enabled = false;
        InitBulletSearchArea(homing);
    }

    /// <summary>
    /// 弾道感知範囲の作成
    /// </summary>
    /// <param name="target">感知範囲が起動する処理</param>
    private void InitBulletSearchArea(ISearchBullet target,float range = 10.0f) {
        GameObject search = Instantiate(Resources.Load<GameObject>(BulletSearchArea.PATH), transform);
        search.transform.localPosition = Vector3.zero;
        BulletSearchArea area = search.GetComponent<BulletSearchArea>();
        area.SetTarget(target);
        area.SetRange(range);
    }

    /// <summary>
    /// 攻撃の命中処理 旧デバッグ用の処理　現在は使用していないのでコメントアウト
    /// </summary>
    /// <param name="target"></param>
    /*
    public void HitTarget(GameObject target) {
        EnemyScript t = target.GetComponent<EnemyScript>();

        if (!t) return;

        t.TakeAttack(damage, elementType);

        iTween.Stop(gameObject);
        parent.MoveEnd();
    }*/

    /*
    public void OnTriggerEnter(Collider other)
    {
		base.OnTriggerEnter (other);
        //HitTarget(other.gameObject);
    }*/
}
