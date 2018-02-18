using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Item_Magic))]
public class PlayerController : Character
{
    //作成　佐藤 竜也
    public GameObject Player;//プレイヤー
    public static PlayerController instance;
    [SerializeField]
    public LevelFloat MAX_MP;
    static public float _HP, _MP;//シーンを跨ぐ際のデータ持越し用
    private static bool initFlag = true;
    [SerializeField]
    float walkSpeed;//移動速度
    [SerializeField]
    float runSpeed;//shift押したときの移動速度
    private Rigidbody rb;//playerのrb
    const float radius = 1.5f;//キャラクターの半径　壁等との衝突判定用

    //作成　針ヶ谷天紀
    [SerializeField]
    GameObject Bullet;
    [SerializeField]
    GameObject playerCamera;
    [SerializeField]
    Animator anim;
    [SerializeField]
    OnMouse onmouse;

    private Item_Magic magick;


    [SerializeField]
    GameObject walkObj;

    public static bool walknow = false;

    protected override void Awake()
    {
        if (initFlag)
        {
            //max_HP = HP;
            //max_MP = MP;
            HP = _HP = MAX_HP.GetValue();
            _MP = MAX_MP.GetValue();
            initFlag = false;
        }
        else
        {
            HP = _HP;
        }

        instance = this;
    }

    private void Start()
    {
        //作成　佐藤 竜也
        //playerのRigitbody取得
        rb = Player.GetComponent<Rigidbody>();
        magick = GetComponent<Item_Magic>();
        //anim = GetComponent<Animator>();
    }

    public static void InitState()
    {
        initFlag = true;
    }

    void Update()
    {
        if (Time.timeScale == 0) {
            return;
        }

        _HP = HP;
        if (!stop)
        {
            walk();
            PlayerShot();
            AddMP();
            AddHP();

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Menu.OpenMenu();
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        ItemGet(other);
    }

    private void AddMP()
    {
        MPHeal(GetHealMP());
    }

    private void AddHP()
    {
        HPHeal(GetHealHP());
    }

    public void HPHeal(float value)
    {
        HP += value;
        if (HP > MAX_HP.GetValue()) HP = MAX_HP.GetValue();
    }

    public void MPHeal(float value)
    {
        _MP += value;
        if (_MP > MAX_MP.GetValue()) _MP = MAX_MP.GetValue();
    }
    

    public LevelFloat HealMP;

    private float GetHealMP()
    {
        switch (MainSceneManager.GetLevel())
        {
            case Level.normal:
                return Time.deltaTime * HealMP.GetValue();
            case Level.hard:
                return Time.deltaTime * HealMP.GetValue();
            default:
            case Level.easy:
                return Time.deltaTime * HealMP.GetValue();
        }
    }

    private static float GetHealHP()
    {
        switch (MainSceneManager.GetLevel())
        {
            case Level.normal:
                return Time.deltaTime * 0;
            case Level.hard:
                return Time.deltaTime * 0;
            default:
            case Level.easy:
                return Time.deltaTime * 0;
        }
    }

    //作成　佐藤 竜也
    void walk()
    {

        //カメラの前をとる
        Vector3 CameraFoward = playerCamera.transform.forward;
        //カメラの横をとる
        Vector3 CameraRight = playerCamera.transform.right;
        //Yいらないのでプレイヤーの位置にする
        CameraFoward.y = transform.forward.y;
        CameraRight.y = transform.right.y;

        //MagickMakeSceneへ
        if (Input.GetKeyDown(KeyCode.F4/*KeyCode.E*/))
        {
            MainSceneManager.OpenScene(ConstData.MagickMakeScene);
        }

        //現在のスピード
        float speed;

        //shiftを押したときspeedをrunSpeedの値に変更
        if (Input.GetKey(KeyCode.LeftShift)) { speed = runSpeed; }
        //離したときspeedをwalkSpeedの値に変更
        else speed = walkSpeed;

        //キーを押していないときmoveを0に
        Vector3 move = Vector3.zero;

        //Aを押したときmoveにxに-1を入れる
        if (Input.GetKey(KeyCode.A))
        {
            //move += Vector3.left * speed * Time.deltaTime;
            move += -CameraRight * speed;
        }
        //Wを押したときmoveにzに+1を入れる
        if (Input.GetKey(KeyCode.W))
        {
            //move += Vector3.forward * speed * Time.deltaTime;
            move += CameraFoward * speed;
        }
        //Sを押したときmoveにzに-1を入れる
        if (Input.GetKey(KeyCode.S))
        {
            //move += Vector3.back * speed * Time.deltaTime;
            move += -CameraFoward * speed;
        }
        //Dを押したときmoveにxに+1を入れる
        if (Input.GetKey(KeyCode.D))
        {
            //move += Vector3.right * speed * Time.deltaTime;
            move += CameraRight * speed;
        }

        //test用即死
        //if(Input.GetKey(KeyCode.F9))
        //{
        //    Death();
        //}

        if (move == Vector3.zero)
        { anim.SetBool("Move", false); }
        else if (move != Vector3.zero)
        { anim.SetBool("Move", true); }

        //移動距離を計算する
        float movedistance = (speed / Mathf.Sqrt(2.0f) * Time.deltaTime);
        RaycastHit hit;
        //自身の位置から移動方向に自身の半径+移動距離分の長さのRayを飛ばす
        if (Physics.Raycast(transform.position, move, out hit, movedistance + radius))
        {
            //Debug.Log(hit.point);
            //移動距離をClampして移動距離を制限する
            movedistance = Mathf.Clamp(movedistance, 0, hit.distance - radius > 0 ? hit.distance - radius : 0);
        }

        //transform.positionを変更して移動する
        transform.position += (move.normalized * movedistance);



        //歩いてる方向にplayerが向くobjを動かす
        walkObj.transform.position = transform.position + move.normalized * 10;

        //魔法うってないときに向く
        if (!Input.GetMouseButtonDown(0) || !Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.transform.position != walkObj.transform.position)
            {
                transform.LookAt(new Vector3(walkObj.transform.position.x, transform.position.y, walkObj.transform.position.z));
                walknow = true;
            }
            else
            {
                walknow = false;
            }
        }
        //斜め移動の時moveの値を斜め用に変える
        //rb.MovePosition(transform.position + (move.normalized * (speed / Mathf.Sqrt(2.0f) * Time.deltaTime)));

        //Debug.Log((move.normalized * (speed / Mathf.Sqrt(2.0f))));//加速度ログ
        //移動ここまで
    }

    [SerializeField]
    private AbnState defElementice;
    [SerializeField]
    private AbnState defElementfai;
    //作成　針ヶ谷天紀
    protected void PlayerShot()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            onmouse.MouseLook();
            if (magick.UseMagick()) {
                //攻撃アニメーションを再生中なら最初から再生しなおす
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    AnimatorReplay(anim);
                    anim.gameObject.transform.localEulerAngles = Vector3.zero;
                }
                else {
                    anim.SetTrigger("Attack");
                }
            }
        }
    }

    /// <summary>
    /// 現在のアニメーションを最初から再生する
    /// </summary>
    /// <param name="animator"></param>
    public static void AnimatorReplay(Animator animator)
    {
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
    }

    /// <summary>
    /// 魔法の使用
    /// </summary>
    /// <returns><c>true</c>, if use was magicked, <c>false</c> otherwise.</returns>
    protected bool MagickUse()
    {
        return false;
    }

    //作成　針ヶ谷天紀
    void ItemGet(Collider other)
    {
        if (other.gameObject.GetComponent<Item>())
        {
            other.gameObject.GetComponent<Item>().Used(gameObject);
        }
    }
    /// <summary>
    /// 引数が味方の物ならtrueが、敵ならfalseが返る
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool isAligment(aligment value)
    {
        //味方判別処理
        return (value == aligment.player);
    }
    public override void Death()
    {
        base.Death();
        //自身のオブジェクトを消す
        //Destroy(gameObject);
        gameObject.layer = Layers.Death;
        anim.Play("Dead");
        GotoResult.GameOver();
    }

    public override void TakeAttack(float value, Vector3 HitPosition, AbnState ele = null)
    {
        base.TakeAttack(value, HitPosition, ele);
        SEManager.SetSE(MagicSystemManager.instance.SEManager.Damage);
    }
}
