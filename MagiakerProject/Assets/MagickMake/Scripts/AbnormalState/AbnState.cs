using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Create AbnormalState Instance")]
public class AbnState : ScriptableObject,IState
{
    [SerializeField]
	public element type;//この状態異常の属性
    [SerializeField]
    public float LimitTime;//付与されてる最大時間
    [SerializeField]
    public float value;//効果量　炎なら一秒ごとのダメージ　氷なら炎ダメージの増加割合　雷なら炎状態ダメージの増加割合
    [SerializeField]
    public float percent;//付与確率
    [SerializeField]
    public float weakPercent;//対象の弱点の場合の付与確率

    /// <summary>
    /// 毎秒受けるダメージを取得
    /// </summary>
    /// <param name="target"></param>
    /// <param name="time"></param>
    /// <returns></returns>
	public float DotDamage(EnemyController target, float time = 0)
    {
        switch (type)
        {
			case element.fire: 
				//電気の状態異常が同時にかかっていたら、効果量が増える
				if (target.abnManager.isState (element.electric)) {
					return value * target.abnManager.states [(int)element.electric].Value.state.value;
				}
				return value;
        }
        return 0;
    }

    /// <summary>
    /// この状態異常が付与されていても行動が可能か
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
	public bool isAction(EnemyController target = null)
    {
        switch (type)
        {
			case element.ice:
			case element.electric:
                return false;//行動不可フラグを返す
        }
        return true;
    }
    
    /// <summary>
    /// この状態異常によって死亡するか
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
	public bool isDeath(EnemyController target)
    {
        switch (type)
        {
			//電気と氷が同時に発生したら即死する
			case element.electric:
				return target.abnManager.isState(element.ice);//死亡フラグを返す
        }
        return false;
    }

    /// <summary>
    /// 受けた攻撃のダメージ量に何等かの影響があるか
    /// </summary>
    /// <param name="value">受けたダメージ</param>
    /// <param name="damageType">ダメージの属性</param>
    /// <returns>影響後のダメージ</returns>
	public float TakeDamage(EnemyController target, float value, element damageType)
    {
        switch (type) {
			case element.ice:
				//氷状態のときに炎の攻撃を受けたらダメージが増える
				if (damageType == element.fire){
					//一度この効果を適用したら氷状態が解除される
					target.abnManager.states [(int)element.ice] = null;
					return value *= this.value;
				}
                break;
        }
        return value;
    }
}
