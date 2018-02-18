using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホーミングなど、「一定範囲内に対象がいたら効果を発揮する」系の弾道用の敵感知処理
/// </summary>
public class BulletSearchArea : MonoBehaviour {
    private ISearchBullet target;
    public const string PATH = "Prefabs/BulletObject/BulletSearchArea";

    public void SetTarget(ISearchBullet target) {
        this.target = target;
    }

    public void SetRange(float value) {
        GetComponent<SphereCollider>().radius = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Hit(other);
    }

    private void Hit(Collider other) {

        if (target == null) {
            Destroy(this);
        }

        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy) {
            if (target.OnSearch(enemy))
            {
                //感知処理を継続する
            }
            else {
                //感知処理を終了する
                Destroy(gameObject);
            }
        }
    }

}

public interface ISearchBullet {
    /// <summary>
    /// 敵を発見した際に呼び出される関数
    /// </summary>
    /// <param name="target">対象のエネミー</param>
    /// <returns>感知処理を継続するか否か</returns>
    bool OnSearch(EnemyController target);
}
