using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//魔法で発射される玉オブジェクトコンポーネント
public class BulletObject : AttackArea {
    //public float damage;
	public element elementType { get { return element.type; } }
    public Bullet parent;

	public void Start()
    {
        Collider col = GetComponent<Collider>();
        if (!col) return;
        GetComponent<Collider>().isTrigger = true;

        Rigidbody rig = GetComponent<Rigidbody>();
        if (!rig) rig = gameObject.AddComponent<Rigidbody>();
        rig.useGravity = false;

        if (parent == null)
            parent = transform.parent.GetComponent<Bullet>();
			

		//AttackArea用の初期設定
		gameObject.tag = Tags.Magic;
		aligment = aligment.player;
        character = parent.parent;

		Damage = parent.damage;

		base.Start ();
    }

    /// <summary>
    /// 属性の変更
    /// </summary>
    /// <param name="value"></param>
	public void ChangeElementType(AbnState value) {
		element = value;
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
