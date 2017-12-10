using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeAttack
{
    /// <summary>
    /// 引数が味方の物ならtrueが、敵ならfalseが返り値になる
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool isAligment(aligment value);
	void TakeAttack(float value, AbnState ele = null);
    void GetDamage(float value, bool isWeak = false);
}