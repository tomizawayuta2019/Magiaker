using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_AttributeWall : Gimick
{
    AttackArea AttackMagic; 
    [SerializeField]
    bool All = false; //なんでも壊れる用
    float HP = 1.0f; //ダメージ0だと通らない用
    [SerializeField]
    element Weakele; //壁の弱点属性


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("gethit");
        //AttackAreaのついている攻撃が来た
        if (other.gameObject.GetComponent<AttackArea>())
        {
            AttackMagic = other.gameObject.GetComponent<AttackArea>();
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
                else if (AttackMagic.element.type == Weakele)
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
