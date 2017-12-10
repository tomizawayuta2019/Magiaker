using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum aligment
{
    enemy,
    player,
    gimmick
}
public class Character : MonoBehaviour, ITakeAttack
{
    public static bool stop;//ポーズするときtrue
	public float HP;    
    public bool DamageObjDestroy = false;
    public virtual bool isAligment(aligment value)
    {
        return false;
    }

    //死亡の処理
    public virtual void Death()
    {
		Debug.Log (gameObject.name + " is Death");
    }
    /// <summary>
    /// 被攻撃処理
    /// </summary>
    /// <param name="value">受けるダメージ</param>
    /// <param name="ele">ダメージの属性</param>
    /// <param name="abnormalStatePersentage">状態異常発生確率（状態異常の仕様によって変更の可能性高）</param>
	public virtual void TakeAttack(float value, AbnState ele = null)
    {
        if (value <= 0) return;

		GetDamage(value);
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="value"></param>
    public void GetDamage(float value, bool isWeak = false)
    {
        HP -= value;
        //ダメージ表記
        var type = FadeOutText.DamageType.Enemy_Damage;
        if (isAligment(aligment.player))
        {
            type = FadeOutText.DamageType.Player_Damage;
        }
        else if (isWeak) type = FadeOutText.DamageType.Enemey_WeakDamage;

        GameObject uiObj = DamageUIManager.Instantiate(value, type);
		uiObj.transform.position = transform.position + DamageUIManager.GetDamageUIDeltaPos;

        if (HP <= 0)
            Death();
        //必要があればノックバックや無敵時間処理を追加
    }
}