using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {
	/// <summary>
	/// 行動が可能か否か
	/// </summary>
	/// <param name="target">対象キャラクタ</param>
	/// <returns></returns>
	bool isAction(EnemyController target);

	/// <summary>
	/// Dotダメージの取得
	/// </summary>
	/// <param name="target">対象キャラクタ</param>
	/// <param name="time">前回ダメージを与えてからの待機時間　時間は呼び出し元で保持すること</param>
	/// <returns></returns>
	float DotDamage(EnemyController target, float time = 0.0f);

	/// <summary>
	/// 攻撃を受けた際の処理
	/// </summary>
	/// <param name="value">受けるダメージ</param>
	/// <param name="damageType">ダメージの属性</param>
	/// <returns></returns>
	float TakeDamage(EnemyController target,float value,element damageType);

	/// <summary>
	/// 即死の判定
	/// </summary>
	/// <param name="target">対象キャラクタ</param>
	/// <returns></returns>
	bool isDeath(EnemyController target);
}
