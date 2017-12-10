using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_AttributeWall : Gimick
{
    //作成　佐藤竜也
    element Attackele; //攻撃の属性
    public element Weakele; //壁の弱点属性
    AttackArea AttackMagic; 
    [SerializeField]
    bool All = false; //なんでも壊れる用
    float HP = 1.0f; //ダメージ0だと通らない用


    private void OnTriggerEnter(Collider other)
    {
        //AttackAreaのついている攻撃が来たら属性を調べる
        if (other.gameObject.GetComponent<AttackArea>())
        {
            AttackMagic = other.gameObject.GetComponent<AttackArea>();
			Attackele = AttackMagic.element.type;
            //プレイヤーの攻撃でしか壊せない
            if (AttackMagic.aligment==aligment.player)
            {
                //全ての攻撃で壊れるbool
                if (All)
                {
                    HP -= AttackMagic.Damage;
                    if (HP <= 0)
                    {
                        //HPを減らす攻撃であれば壊れる
                        Destroy(gameObject);
                    }
                }
                //壁の弱点と攻撃の属性があっていれば
                else if (Attackele == Weakele)
                {
                    HP -= AttackMagic.Damage;
                    if (HP <= 0)
                    {
                        //HPを減らす攻撃であれば壊れる
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

}
