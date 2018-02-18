using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Switch : Gimick {

    float HP = 1.0f;

    [SerializeField]
    List<GameObject> WallList = new List<GameObject>();

    AttackArea AttackMagic;
    [SerializeField]
    AudioSource SE;

    private GameObject _child;
    private void Start()
    {
        //int count = 0;
        //foreach (Transform child in transform)
        //{
        //    GameObject Childwall = GameObject.Find(child.name);
        //    WallList.Add(Childwall);
        //    //count++;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //AttackAreaのついている攻撃が来たら
        if (other.gameObject.GetComponent<AttackArea>())
        {
            AttackMagic = other.gameObject.GetComponent<AttackArea>();
            //プレイヤーの攻撃でしか壊せない
            if (AttackMagic.aligment == aligment.player)
            {
                GetDamage(AttackMagic.Damage);

                if (HP <= 0)
                {
                    foreach (GameObject obj in WallList) {
                        Destroy(obj);
                    }
                    //HPを減らす攻撃であれば壊れる
                    SEManager.SetSE(SE);
                    Destroy(gameObject);
                }
            }
        }
    }

    public override void GetDamage(float value, bool isWeak = false)
    {
        base.GetDamage(value, isWeak);
        HP -= AttackMagic.Damage;
    }
}