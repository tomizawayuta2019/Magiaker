using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 東谷 太喜
/// </summary>
public class DamageZone : MonoBehaviour {

    //public float capsuleRadi = 0.5f; //カプセルコライダーの太さ
    //public float damageInterval = 1f;
    //public float deleteTime;
    //public float damageInterval;//DamageZone内にいる敵の受けるダメージのインターバル
    //public float colInterval;//間隔を開けてコライダーを置くためのインターバル

    private GameObject Player;
    private Shot shot;
    private float createColTimer;//colliderを作るためのタイマー
    private float damageTimer;//DamageZone内にいる敵に与えるダメージのためのタイマー
    private float destroyTimer;//DamageZoneが削除されるまでのタイマー
    private Vector3 playerPosiTemp;//Playerの弾の発射時の位置

    void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        shot = Player.GetComponent<Shot>();
        playerPosiTemp = Player.transform.position;
    }
	
	void Update ()
    {
        createColTimer += Time.deltaTime;
        damageTimer += Time.deltaTime;
        destroyTimer += Time.deltaTime;
        if (shot.bullets != null)
        {
            //コライダーを間隔を空けて作る
            if (createColTimer >= shot.createColInterval)
            {
                shot.CreateZone();
                createColTimer = 0;
            }
        }
        //時間以上になると削除
        if (destroyTimer >= shot.deleteTime)
        {
            destroyTimer = 0;
            Destroy(gameObject);
            Debug.Log("DamageZone削除");
            //gameObject.SetActive(false);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Enemy" && damageTimer >= shot.damageInterval)
        {
            damageTimer = 0;
            Debug.Log("damage");
        }
    }
}
