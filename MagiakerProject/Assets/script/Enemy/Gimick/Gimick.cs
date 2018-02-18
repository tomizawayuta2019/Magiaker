using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimick : MonoBehaviour
{
    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="value"></param>
    public virtual void GetDamage(float value, bool isWeak = false)
    {
        var type = FadeOutText.DamageType.Player_Damage;

        GameObject uiObj = DamageUIManager.Instantiate(value, type);
        uiObj.transform.position = transform.position + DamageUIManager.GetDamageUIDeltaPos;
    }
    
}