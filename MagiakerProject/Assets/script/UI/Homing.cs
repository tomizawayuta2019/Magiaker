using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 東谷 太喜
/// </summary>
public class Homing : MonoBehaviour,ISearchBullet {

    public GameObject target; //目標
    public List<GameObject> targets = new List<GameObject>();
    bool isTarget;

    private Vector3 TargetPos
    {
        get
        {
            if (target)
            {
                return _targetPos = target.transform.position + new Vector3(0,0.5f,0);
            }
            else
            {
                return _targetPos;
            }
        }
    }
    private Vector3 _targetPos;

    public float speed = 10; //弾の速さ
    public float homingAbility = 5.0f; // ホーミング性能
    public float searchRange = 10.0f; //ホーミングの開始する範囲
    //public float destroyInterval;時間経過で削除するまでの時間
    public float waitTime;

    //private float destroyTimer;//時間経過で削除する用のタイマー
    private float angleAdjustment = 20.0f; //角度調整
    private float angleRotation = 180.0f; //回転角

    void Update ()
    {
        if (!isTarget && targets.Count > 0) {
            isTarget = true;
            float distance = 10000;
            foreach (GameObject obj in targets) {
                if (obj != null && Vector3.Distance(transform.position, obj.transform.position) < distance) {
                    distance = Vector3.Distance(transform.position, obj.transform.position);
                    target = obj;
                }
            }
        }

        if (/*Vector3.Distance(transform.position,target.transform.position) < searchRange*/true) {
            //Vector3 targetVector = target.transform.position - transform.position;
            Vector3 targetVector = TargetPos - transform.position;
            Vector3 front = transform.TransformDirection(Vector3.forward);

            float targetAngle = Vector3.Angle(front, targetVector); //角度差
            float angleAdd = (angleRotation * Time.deltaTime);

            //ターゲットの方向
            Quaternion rotaTarget = Quaternion.LookRotation(targetVector);

            if (targetAngle - angleAdjustment <= angleAdd)
            {
                //ターゲットが回転角以内なら完全にターゲットのほうを向く
                //Debug.Log("曲がる");
                transform.rotation = 
                    Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(TargetPos - transform.position),
                    0.002f);
            }
            else
            {
                //ターゲットが回転角の外なら、指定角度だけターゲットに向かせる
                float angle = (angleAdd / targetAngle) + (Time.deltaTime * homingAbility);
                Vector3 rotation = transform.eulerAngles;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotaTarget, angle);
                //Debug.Log("指定角度向く");
            }
        }
        //時間経過で削除する用スクリプト
        //destroyTimer += Time.deltaTime;
        //if (destroyTimer >= destroyInterval)
        //{
        //    Destroy(gameObject);
        //    destroyTimer = 0;
        //}
        //ワールド空間にして移動
        transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;

        //Enemyが消えた場合はEnemyがいた場所にたどり着いたら消える
        if (Vector3.Distance(TargetPos, transform.position) < 2.0f)
        {
            waitTime += Time.deltaTime;
            if (waitTime > 0.5f) {
                BulletObject bul = GetComponent<BulletObject>();
                if (bul && bul.bulletType == BulletType.homingExplosion) {
                    bul.HitExplosion();
                }
                Destroy(gameObject);
            }
        }
        else {
            waitTime = 0;
        }
    }

    public bool OnSearch(EnemyController target)
    {
        enabled = true;
        //this.target = target.gameObject;
        targets.Add(target.gameObject);

        //元の弾道による移動を停止させる
        BulletObject bul = GetComponent<BulletObject>();
        bul.parent.MoveStop();

        //StartHoming();

        return false;
    }

    /// <summary>
    /// ホーミング開始時に呼ばれる初期化関数
    /// </summary>
    private void StartHoming() {
        Vector3 velocity = GetComponent<BulletObject>().Velocity;
        //現在のスピードを引き継ぐ
        speed = velocity.magnitude * 20;
        if (speed <= 0) {
            speed = 10;
        }
        //現在の移動方向へ向く
        transform.LookAt(transform.position + (velocity * 100f));
    }
}
