using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public delegate void RemoveEnemyList(EnemyController obj);

//作成　針ヶ谷天紀
public class EnemyController : Character
{
    [SerializeField]
    private element weakType;//弱点の属性
    [SerializeField]
    private AbnStateManager abnStateManager;
    public AbnormalStateManager abnManager = new AbnormalStateManager();
    public Vector3 StateSymbolPosition;//状態異常エフェクトの表示位置
    public Vector3 StateSymbolSize = Vector3.one;//状態異常エフェクトのサイズ
    [SerializeField]
    GameObject PlayerSensor;
    [SerializeField]
    GameObject HPItem;
    [SerializeField]
    GameObject MPItem;
    [SerializeField]
    LevelInt HPDropPercent;
    [SerializeField]
    LevelInt MPDropPercent;
    [SerializeField]
    LevelFloat HealPoint;
    Itemtype itemtype;
    NavMeshAgent agent;
    Action Action;
    GameObject PC;
    PlayerController PLController;
    GameObject Items;
    PlayerSensor Sensor;
    Animator anim;

    public virtual void InitFlag()
    {
        Action.StopCoroutines();
    }
    protected override void Awake()
    {
        //敵に対して攻撃が二重ヒットするバグが発生中。対処として敵のHPを二倍に。
        MAX_HP.easy *= 2;
        MAX_HP.normal *= 2;
        MAX_HP.hard *= 2;
        base.Awake();
        Action = GetComponent<Action>();
    }
    protected virtual void Start()
    {
        abnManager.StateSymbolMaterial = abnStateManager.AbnStateSymbol;
        abnManager.objtransform = transform;
        abnManager.symbolPos = StateSymbolPosition;
        abnManager.symbolSize = StateSymbolSize;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        PC = GameObject.FindGameObjectWithTag("Player");
        //PLController = PC.GetComponent<PlayerController>();
        PLController = PlayerController.instance;
       
        Sensor = PlayerSensor.GetComponent<PlayerSensor>();
    }
    private void OnEnable()
    {
        if(Action!=null)
        Action.PauseEnd();
    }
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (stop)
        {
            Action.StopCoroutines();
            return;
        }

        //諸々の状態異常効果の反映
        if (abnManager.isDeath(this))
        {
            Death();
            return;
        }
        abnManager.TimeCheck(Time.deltaTime);
        float dot = abnManager.DotDamage(this, Time.deltaTime);
        if (dot != 0)
            GetDamage(dot,transform.position);
        //状態異常による行動不能の確認　状態異常の時間経過処理をした後に確認
        if (!abnManager.isAction(this))
        {
            if (Action) {
                Action.StopCoroutines();
            }
            return;
        }
        //前のフレームで停止状態だったなら、いろいろ初期化する
        else if(abnManager.IsActionStopStateEnd()) {
            Action.PauseEnd();
            return;
        }
        if (Sensor && Sensor.GetPL_Search())
        {
            if (PLController.HP <= -1)
            { Action.PlayerDeathStop(); }
            Action.ActionEnter(PC, gameObject);
        }
    }

    /// <summary>
    /// 被攻撃処理 enemyは弱点に応じてダメージが増え、状態異常を受ける可能性がある
    /// </summary>
    /// <param name="value">受けるダメージ</param>
    /// <param name="ele">ダメージの属性</param>
    public override void TakeAttack(float value, Vector3 HitPosition, AbnState ele = null)
    {
        if (value <= 0) return;

        bool isWeak = false;

        //属性による影響
        if (ele)
        {
            isWeak = (ele.type == weakType);
            //弱点の反映
            value *= isWeak ? ConstData.WEAK_DAMAGE_BONUS : 1;

            //状態異常による影響
            value = abnManager.TakeDamage(this, value, ele.type);

            //状態異常の反映
            if (Random.Range(0f, 1f) <= (isWeak ? ele.weakPercent : ele.percent))
            {
                TakeAbnormalState(ele);
                //Debug.Log("ok");
            }
        }

        GetDamage(value,HitPosition, isWeak);
    }

    /// <summary>
    /// 状態異常を受ける処理
    /// </summary>
    /// <param name="value"></param>
    public virtual void TakeAbnormalState(AbnState value)
    {
        abnManager.AddAbnormalState(value);
    }

    public override void Death()
    {
        base.Death();
        Action.StopCoroutines();
        StartCoroutine(DeathStart());
    }
    private IEnumerator DeathStart()
    {
        gameObject.layer = Layers.Death;
        Action.Death();
        if (agent)
        {
            agent.Stop();
            agent.velocity *= 0;
        }
        if (anim)
        {
            Sensor = null;
            anim.Play("Death");
            yield return null;
            yield return StartCoroutine(WaitAnimationEnd("Death"));
        }

        //アイテムが出るかの確立
        ItemDrop();
        //EnemySpawnリストから消す
        ListDestroy();
        //自身のオブジェクトを消す
        Destroy(gameObject);
        yield break;
    }
    protected virtual void ItemDrop()
    {
        if (Random.Range(0,100) < HPDropPercent.GetValue())
        {
            Items = Instantiate(HPItem, transform.position, Quaternion.identity);
            Items.GetComponent<Item>().itemtype = Itemtype.HPHeal;
        }
        else if (Random.Range(0, 100) < MPDropPercent.GetValue())
        {
            Items = Instantiate(MPItem, transform.position, Quaternion.identity);
            Items.GetComponent<Item>().itemtype = Itemtype.MPHeal;
        }
        if (Items != null)
        {
            Items.GetComponent<Item>().Heal = HealPoint.GetValue();
            //ItemにPlayerを登録
            Items.GetComponent<Item>().Player = PlayerController.instance.gameObject;
            //アイテムを散らすように出す
            Items.GetComponent<Rigidbody>().AddForce(
                new Vector3(0, 5, 0),
                ForceMode.Impulse);
            Items.tag = "Item";
        }

    }

    private IEnumerator WaitAnimationEnd(string animatorName)
    {
        bool finish = false;
        while (!finish)
        {
            AnimatorStateInfo nowState = anim.GetCurrentAnimatorStateInfo(0);
            if (!nowState.IsName(animatorName)) { anim.Play("Death"); }
            if (nowState.IsName(animatorName) && nowState.normalizedTime >= 0.9f)
            { finish = true; }
            else
            {
                //yield return new WaitForSeconds(0.1f); 
                yield return null;
            }
        }
    }

    /// <summary>
    /// 引数が味方の物ならtrueが、敵ならfalseが返り値になる
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool isAligment(aligment value)
    {
        //味方判別処理
        return (value == aligment.enemy || value == aligment.gimmick);
    }

    //EnemySpawnのListの変更
    //Charactorが破棄される前に呼ばれる関数を設定する
    public void SetOnDest(RemoveEnemyList dest)
    {
        this.dest = dest;
    }

    //Enemyが破棄されたときに呼ばれる関数
    RemoveEnemyList dest;

    private void ListDestroy()
    {
        if (dest != null)
            dest(this);
    }
}

public class AbnormalStateManager : IState
{
    public Transform objtransform;
    public Vector3 symbolPos,symbolSize;
    const int SYMBOL_COUNT = 3;
    GameObject[,] AbnormalStateSymbol = new GameObject[4,SYMBOL_COUNT];//エフェクトが一つだと薄くて見難いので三つ重ねる
    public GameObject[] StateSymbolMaterial;
    //状態異常の配列　各種類一つずつまでしか同時に付与されない
    //状態異常の格納場所はその状態異常のenum(element)番号に依存
    public State?[] states = new State?[System.Enum.GetNames(typeof(element)).Length];

    /// <summary>
    /// その状態異常にかかっているか否か
    /// </summary>
    /// <returns><c>true</c>, if state was ised, <c>false</c> otherwise.</returns>
    public bool IsState(element type)
    {
        return states[(int)type].HasValue;
    }

    public int Debug_stateCount()
    {
        int i = 0;
        foreach (State? s in states)
            if (s.HasValue)
                i++;
        return i;
    }

    /// <summary>
    /// 新しい状態異常の追加
    /// </summary>
    /// <param name="state">状態異常の情報</param>
    public void AddAbnormalState(AbnState value)
    {
        Vector3 set = objtransform.position;
        //set.x += 0 + (int)value.type;
        //既に同じタイプの状態異常があれば、それを上書きする
        if (!AbnormalStateSymbol[(int)value.type,0])
        {
            for (int i = 0; i < SYMBOL_COUNT; i++) {
                AbnormalStateSymbol[(int)value.type,i] = GameObject.Instantiate(StateSymbolMaterial[(int)value.type], set, Quaternion.identity);
                AbnormalStateSymbol[(int)value.type,i].transform.SetParent(objtransform);
                Vector3 size = AbnormalStateSymbol[(int)value.type, i].transform.localScale;
                AbnormalStateSymbol[(int)value.type, i].transform.localScale = new Vector3(symbolSize.x * size.x, symbolSize.y * size.y, symbolSize.z * size.z);
                AbnormalStateSymbol[(int)value.type, i].transform.localPosition = symbolPos;
            }
        }
        states[(int)value.type] = new State(value);
    }

    /// <summary>
    /// 経過時間の更新
    /// </summary>
    /// <param name="time">Time.</param>
    public void TimeCheck(float time)
    {
        //ループ内で構造体の情報を更新することがあるので、foreachではなくforを使用
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].HasValue)
            {
                //TimeCheck関数内で数値を書き換えているため、一度仮置きしてから代入する
                State s = states[i].Value;
                if (!s.TimeCheck(time))
                {
                    for (int j = 0; j < SYMBOL_COUNT; j++) {
                        GameObject.Destroy(AbnormalStateSymbol[i,j]);
                    }
                    states[i] = null;
                }
                else
                {
                    states[i] = s;
                }
            }
            if (states[i] == null && AbnormalStateSymbol[i,0] != null)
            {
                for (int j = 0; j < SYMBOL_COUNT; j++) {
                    GameObject.Destroy(AbnormalStateSymbol[i, j]);
                }
            }
        }
    }

    private bool? isStop,nowIsStop;//停止の状態異常の前のフレームでの状態確認
    /// <summary>
    /// 行動が可能か否か
    /// </summary>
    /// <param name="target">対象キャラクタ</param>
    /// <returns></returns>
    public bool isAction(EnemyController target)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].HasValue && !states[i].Value.isAction(target)) {
                isStop = nowIsStop;
                nowIsStop = true;
                return false;
            }
                
        }
        isStop = nowIsStop;
        nowIsStop = false;
        return true;
    }

    /// <summary>
    /// 前フレームで停止の状態異常にかかっていたか isAcitonよりも前に呼び出すこと
    /// </summary>
    /// <returns></returns>
    public bool IsActionStopStateEnd() {
        return isStop == null ? false : isStop.Value;
    }

    /// <summary>
    /// Dotダメージの取得
    /// </summary>
    /// <param name="target">対象キャラクタ</param>
    /// <param name="time">前回ダメージを与えてからの待機時間　時間は呼び出し元で保持すること</param>
    /// <returns></returns>
    public float DotDamage(EnemyController target, float time = 0.0f)
    {
        float value = 0;
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].HasValue)
            {
                //TimeCheck関数内で数値を書き換えているため、一度仮置きしてから代入する
                State s = states[i].Value;
                value += s.DotDamage(target, time);
                states[i] = s;
            }
        }
        return value;
    }

    /// <summary>
    /// 攻撃を受けた際の処理
    /// </summary>
    /// <param name="value">受けるダメージ</param>
    /// <param name="damageType">ダメージの属性</param>
    /// <returns></returns>
    public float TakeDamage(EnemyController target, float value, element damageType)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].HasValue)
                value = states[i].Value.TakeDamage(target, value, damageType);
        }
        return value;
    }

    /// <summary>
    /// 即死の判定
    /// </summary>
    /// <param name="target">対象キャラクタ</param>
    /// <returns></returns>
    public bool isDeath(EnemyController target)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].HasValue && states[i].Value.isDeath(target))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 状態異常管理用構造体
    /// </summary>
    public struct State : IState
    {
        public State(AbnState state)
        {
            this.state = state;
            elapsedTime = 0;
            dilayTime = 0;
        }
        public AbnState state;
        public element type { get { return state.type; } }
        public float elapsedTime;//経過した時間
        private float dilayTime;//一定時間ごとに発揮される効果用の待機時間

        public bool TimeCheck(float time)
        {
            elapsedTime += time;
            //Debug.Log(elapsedTime);
            return (elapsedTime < state.LimitTime);
        }

        public bool isAction(EnemyController target)
        {
            return ((IState)state).isAction(target);
        }

        public float DotDamage(EnemyController target, float time = 0)
        {
            dilayTime += time;
            if (dilayTime > 1)
            {
                dilayTime -= 1;
                return ((IState)state).DotDamage(target, dilayTime);
            }
            return 0;
        }

        public float TakeDamage(EnemyController target, float value, element damageType)
        {
            return ((IState)state).TakeDamage(target, value, damageType);
        }

        public bool isDeath(EnemyController target)
        {
            return ((IState)state).isDeath(target);
        }
    }
}