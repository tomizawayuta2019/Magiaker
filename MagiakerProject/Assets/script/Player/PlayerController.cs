using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item_Magic))]
public class PlayerController : Character
{
    //作成　佐藤 竜也
    public GameObject Player;//プレイヤー
	[SerializeField]
	public float MP;
	static public float? _HP = null,_MP = null;//シーンを跨ぐ際のデータ持越し用
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
    float ShotSpeed = 30;

	[SerializeField]
	GameObject playerCamera;

    private Item_Magic magick;

    private void Start()
    {
        //作成　佐藤 竜也
        //playerのRigitbody取得
        rb = Player.GetComponent<Rigidbody>();
        magick = GetComponent<Item_Magic>();
		if (!_HP.HasValue)
			_HP = HP;
		else
			HP = _HP.Value;
		if (!_MP.HasValue)
			_MP = MP;
		else
			MP = _MP.Value;
    }

    void Update()
    {
		_HP = HP;
		_MP = MP;
        if (!stop)
        {
            walk(); 
			PlayerShot();
			AddMP ();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ItemGet(other);
    }

	private void AddMP(){
		MP += Time.deltaTime;
	}

    //作成　佐藤 竜也
    void walk()
    {
		Vector3 CameraFoward = playerCamera.transform.forward;
		Vector3 CameraRight = playerCamera.transform.right;

		CameraFoward.y = transform.forward.y;
		CameraRight.y = transform.right.y;

		Debug.Log (CameraFoward);
		if (Input.GetKeyDown (KeyCode.E))
			MainSceneManager.OpenScene (ConstData.MagickMakeScene);

        float speed;//現在のスピード

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
			move += CameraFoward* speed;
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
        //移動距離を計算する
        float movedistance = (speed / Mathf.Sqrt(2.0f) * Time.deltaTime);
        RaycastHit hit;
        //自身の位置から移動方向に自身の半径+移動距離分の長さのRayを飛ばす
        if (Physics.Raycast(transform.position, move, out hit, movedistance + radius))
        {
            Debug.Log(hit.point);
            //移動距離をClampして移動距離を制限する
            movedistance = Mathf.Clamp(movedistance, 0, hit.distance - radius > 0 ? hit.distance - radius : 0);
        }

        //transform.positionを変更して移動する
        transform.position += (move.normalized * movedistance);

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
		if (Input.GetMouseButtonDown (0) || Input.GetKeyDown(KeyCode.Space)) {
            magick.UseMagick();
		}
		/*
        if (Input.GetMouseButtonDown(0))
        {
            GameObject Bullets = null;
            //弾を生成する
            Bullets = Instantiate(Bullet, transform.position, Quaternion.identity);
            //弾に初速を与える
            Bullets.GetComponent<Rigidbody>().AddForce(transform.forward * ShotSpeed, ForceMode.Impulse);
            //Bullets.GetComponent<DamageScript>().aligment = aligment.player;
            Bullets.AddComponent<AttackArea>();
            Bullets.GetComponent<AttackArea>().aligment = aligment.player;
            Bullets.GetComponent<AttackArea>().DestroyCheck = DamageObjDestroy;
            Bullets.GetComponent<AttackArea>().Damage = 1;
            Bullets.tag = Tags.Magic;
			Bullets.GetComponent<AttackArea>().element = defElementice;
        }
        if(Input.GetMouseButtonDown(1))
        {
            GameObject Bullets = null;
            //弾を生成する
            Bullets = Instantiate(Bullet, transform.position, Quaternion.identity);
            //弾に初速を与える
            Bullets.GetComponent<Rigidbody>().AddForce(transform.forward * ShotSpeed, ForceMode.Impulse);
            //Bullets.GetComponent<DamageScript>().aligment = aligment.player;
            Bullets.AddComponent<AttackArea>();
            Bullets.GetComponent<AttackArea>().aligment = aligment.player;
            Bullets.GetComponent<AttackArea>().DestroyCheck = DamageObjDestroy;
            Bullets.GetComponent<AttackArea>().Damage = 1;
            Bullets.tag = Tags.Magic;
            Bullets.GetComponent<AttackArea>().element = defElementfai;
        }*/
    }

	/// <summary>
	/// 魔法の使用
	/// </summary>
	/// <returns><c>true</c>, if use was magicked, <c>false</c> otherwise.</returns>
	protected bool MagickUse(){
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
    }
}
