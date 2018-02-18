using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 東谷 太喜
/// </summary>
public class Shot : MonoBehaviour
{
    [HideInInspector]
    public GameObject bullets;//複製された弾
    public GameObject bullet; //弾のPrefab
    public GameObject paintZone;//PaintZoneのPrefab
    public Transform Muzzle; //発射する場所
    public float speed = 1000; //弾の速度

    // DamageZoneのインターバル系統
    public float damageInterval;//ダメージのインターバル
    public float deleteTime;//DamageZoneを削除するまでの時間
    public float createColInterval;//間隔を開けてコライダーを置くためのインターバル
    public float capsuleRadi; //カプセルコライダーの太さ

    private GameObject damageZones;//DamageZoneをまとめる用の空のオブジェクト

    void Start()
    {
        damageZones = new GameObject();
        damageZones.name = "DamageZones";
        CapsuleCollider capsule = paintZone.GetComponent<CapsuleCollider>();
        capsule.isTrigger = true;
        capsule.radius = capsuleRadi;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //弾を前に発射
            bullets = Instantiate(bullet) as GameObject;
            bullets.transform.position = Muzzle.transform.position;
            Vector3 force = bullets.transform.forward * speed;
            bullets.GetComponent<Rigidbody>().AddForce(force);

            //DamageZoneを生成する
            CreateZone();
        }
    }

    public void CreateZone()
    {
        //ペイントされたダメージゾーンを生成
        GameObject damageZone = Instantiate(paintZone);
        damageZone.transform.position = bullets.transform.position;
        //DamageZonesにまとめる
        damageZone.transform.parent = damageZones.transform;
    }
}
