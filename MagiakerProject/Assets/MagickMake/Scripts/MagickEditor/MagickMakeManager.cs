using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 作成者　富澤勇太
/// 選択中の弾道にどんな操作を加えるか
/// </summary>
public enum BulletState {
    none,
    move,
    rotate
}

/// <summary>
/// 作成者　富澤勇太
/// </summary>
public class MagickMakeManager : MonoBehaviour {
    [SerializeField]
    public BulletManager bulletManager;
	[SerializeField]
	public AbnStateManager abnManager;

    //public Vector3 bulletDefaultPosition;
    public Transform BulletParent;

    [SerializeField]
    public Magick magick = new Magick();
    [SerializeField]
	private MagickButtonZoneUI.TargetType selectType;
	[SerializeField]
	static public int selectNum;
    [SerializeField]
    private GameObject selectImage;

    [SerializeField]
	private Bullet selectBullet{ 
		get{ 
			return nowSelectBullet ?? oldSelectBullet;
		} set { 
			if (value != nowSelectBullet) {
				oldSelectBullet = nowSelectBullet;
				nowSelectBullet = value;
			}
		} }
	private Bullet nowSelectBullet,oldSelectBullet;
    [SerializeField]
    private BulletState state;//弾道を選択している間の弾道の挙動（移動する・回転する等）
    [SerializeField]
    private Text BulletNameText;

	[SerializeField]
	public element selectElement;

    static public MagickMakeManager MM;

	void Start(){
		MM = this;
		MagickStatesUI.magickStatesUI.SetMagick (magick);
	}

    /// <summary>
    /// 魔法のプレビュー
    /// </summary>
    public void MagickEnter() {
        BulletMoveEnter();
        BulletRotateEnter();
		magick.TestEnter (BulletParent.gameObject);
    }

    void Update()
    {
        bool isLeftButtonDown = Input.GetMouseButtonDown(0);
        BulletNameText.text = selectBullet ? selectBullet.name : " ";
        //弾道の移動・回転
		if (nowSelectBullet) {
            switch (state)
            {
                case BulletState.move:
                    if (isLeftButtonDown)
                        BulletMoveEnter();
                    else BulletMove(selectBullet);
                    break;
                case BulletState.rotate:
                    if (isLeftButtonDown)
                        BulletRotateEnter();
                    else BulletRotate(selectBullet);
                    break;
                default: break;
            }
        }
        //弾道の選択
        else if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit,Mathf.Infinity,Physics.DefaultRaycastLayers,QueryTriggerInteraction.Collide)) {//メニューで選択した弾道を優先的にhitさせるようにレイヤーを分ける（予定）
                GameObject hitObj = hit.transform.gameObject;
                BulletObject hitBullet = hitObj.GetComponent<BulletObject>();
                if (hitBullet) {
                    BulletSelect(hitBullet.parent);
                }
            }
        }

        //プレビュー
        if (Input.GetKeyDown(KeyCode.R)) {
            MagickEnter();
        }

        //テスト用 シーン終了
		if (Input.GetKeyDown(KeyCode.E)){
			MainSceneManager.CloseScene ();
		}
            //SceneManager.LoadScene(2);
    }

	/// <summary>
	/// 生成する弾道の属性
	/// </summary>
	/// <param name="ele">属性</param>
	public void SetElement(element ele){
		selectElement = ele;
	}

    /// <summary>
    /// 弾道を生成して魔法に登録する
    /// </summary>
    /// <param name="bul">弾道のプレファブ</param>
    public void BulletInit(Bullet bul) {
        Bullet bulObj = Instantiate(bul, BulletParent.position, Quaternion.identity);
        bulObj.transform.SetParent(BulletParent);
		bulObj.SetPrefab (bul.gameObject);
		bulObj.ChangeElement (selectElement);
        magick.AddBullet(bulObj,bul);
        BulletSelect(bulObj);
    }

    /// <summary>
    /// 弾道の選択
    /// </summary>
    /// <param name="bul"></param>
    public void BulletSelect(Bullet bul) {
        selectBullet = bul;
        state = BulletState.move;
        selectBullet.SetBulletObjEnabled(false);
    }

    /// <summary>
    /// マウスのワールド座標を取得
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMousePosToWorldPos() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - 1.5f;//カメラy座標（上から見下ろし）に設定
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    /// <summary>
    /// 弾道の移動
    /// </summary>
    /// <param name="bul"></param>
    private void BulletMove(Bullet bul) {
        bul.transform.position = GetMousePosToWorldPos();
    }

    /// <summary>
    /// 弾道移動の終了
    /// </summary>
    public void BulletMoveEnter() {
        if (selectBullet) {
            state = BulletState.rotate;
            //selectBullet.SetDefaultPosition();
            selectBullet.SetBulletObjEnabled(true);
        }
    }

    /// <summary>
    /// Bulletの回転
    /// </summary>
    /// <param name="bul"></param>
    private void BulletRotate(Bullet bul) {
        if (selectBullet) {
            selectBullet.transform.LookAt(GetMousePosToWorldPos());
            Vector3 rotate = selectBullet.transform.localEulerAngles;
            rotate.x = 0;
            rotate.z = 0;
            selectBullet.transform.localEulerAngles = rotate;
        }
    }

    /// <summary>
    /// 弾道回転終了
    /// </summary>
    public void BulletRotateEnter() {
        state = BulletState.none;
        selectBullet = null;
    }

    /// <summary>
    /// 選択中の弾道の削除
    /// </summary>
    public void BulletDelete() {
        if (selectBullet) {
            magick.RemoveBullet(selectBullet);
            Destroy(selectBullet.gameObject);
            selectBullet = null;
        }
    }

	public void BulletCopy() {
		if (selectBullet) {
			BulletInit (selectBullet.GetPrefab ().GetComponent<Bullet> ());
		}
	}

    /// <summary>
    /// 弾道への属性変更
    /// </summary>
    /// <param name="bul"></param>
    public void BulletAddAligment(Bullet bul) {

    }

    public void MagicSelect(MagickButtonZoneUI.TargetType type, int num, GameObject button = null) {
        selectType = type;
        selectNum = num;

        if (button)
        {
            selectImage.transform.SetParent(button.transform);
            selectImage.transform.localPosition = Vector2.zero;
            selectImage.SetActive(true);
        }
        else {
            selectImage.SetActive(false);
        }
    }

    /// <summary>
    /// 魔法の保存
    /// </summary>
	public void SaveMagick() {
		if (selectType == MagickButtonZoneUI.TargetType.have) {
            magick.Save();
			Item_Magic.m_Magicks [selectNum] = magick.GetClone ();
		}
    }

	/// <summary>
	/// 魔法のロード
	/// </summary>
	/// <param name="value">Value.</param>
	public void LoadMagick(){
        Magick m = null;
        switch (selectType) {
            case MagickButtonZoneUI.TargetType.created:
                m = CreatedMagickData.CreatedMagicks[selectNum];
                //もし未設定なら新しい魔法を生成する
                break;
            case MagickButtonZoneUI.TargetType.have:
                m = Item_Magic.m_Magicks[selectNum];
                break;
        }
        if (m == null) {
            m = new Magick();
            Debug.Log("magick = null");
        } 

        Debug.Log("data load");

        //現在編集中の魔法を破棄する
        magick.Delete();

		//新しく魔法を生成（コピー）する
        magick = m.GetClone();
        magick.Instantiate(BulletParent.gameObject);

		//魔法ステータス表示を更新する
		MagickStatesUI.magickStatesUI.SetMagick(magick);
    }
}
