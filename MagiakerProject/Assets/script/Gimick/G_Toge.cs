using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Toge : Gimick
{
    //作成　佐藤竜也
    [SerializeField]
    float InvincibleTime = 2.0f;
    private float inv_time;
    [SerializeField]
    float Damage = 10.0f;
    //このスクリプトがついてるものをギミックと定義
    aligment aligment = aligment.gimmick;
    GameObject Player;

    PlayerMotion PM;

    private void Start()
    {
        //初期ダメージ
        inv_time = InvincibleTime;
        Player = GameObject.FindWithTag("Player");
        PM = Player.GetComponent<PlayerMotion>();

    }
    //トゲでダメージする処理
    void OnTriggerStay(Collider other)
    {
        //無敵処理
        if (inv_time <= InvincibleTime)
        {
            if (other.gameObject.tag == Tags.Player)
            {
                inv_time += Time.deltaTime;
            }
        }
        //ダメージ処理
        else
        {
            if (other.gameObject.tag == Tags.Player)
            {
                Debug.Log(Damage);
                inv_time = 0.0f;
                G_TogeDamage(other.gameObject);
                PM.G_Damage();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        //抜けた後すぐダメージ
        if(other.gameObject.tag == Tags.Player)
        {
            inv_time = InvincibleTime;
        }
    }
    void G_TogeDamage(GameObject Target)
    {
        
        Character target = Target.GetComponent<Character>();
        if (target)
        {
            if (!target.isAligment(aligment))
            { target.TakeAttack(Damage); }
        }
    }
}