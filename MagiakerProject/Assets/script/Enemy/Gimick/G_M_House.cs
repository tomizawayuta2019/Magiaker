using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_M_House : Gimick
{

    [SerializeField]
    List<GameObject> WallList = new List<GameObject>();

    [SerializeField]
    List<GameObject> EnemyList = new List<GameObject>();

    [SerializeField]
    int Monsterlimit; //モンスター数限界値

    int MonsterCount; //モンスターの数

    public bool Spawnflag; //モンスター増やしていいのか

    void Update()
    {
        int x = EnemyList.Count - 1;

        //常にモンスターがリストからいなくなったらリストを削除する
        for (int i = 0; i <= x; i++)
        {
            if (EnemyList[i] == null)
            {
                EnemyList.Remove(EnemyList[i]);
            }
        }
        //モンスター数限界値の判定
        if(x >= Monsterlimit )
        {
            Spawnflag = false;
        }
        else
        {
            Spawnflag = true;
        }
    }
    //SetActiveがfalseからtrueになった時（MagicMakeSceneに移行したとき）
    //モンスターを数えなおす
    private void OnDisable()
    {
        EnemyList.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーがモンスターハウス内に入った時
        //壁を作る
        if (other.gameObject.tag == Tags.Player)
        {
            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].SetActive(true);
            }
        }
        //モンスターがモンスターハウスにいるとき
        //リストに追加する
        else if (other.gameObject.tag == Tags.Enemy)
        {
            EnemyList.Add(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Enemyがいなくなったとき
        //壁がなくなる
        int x = EnemyList.Count;
        if (x == 0)
        {
            if (other.gameObject.tag == Tags.Player)
            {
                for (int i = 0; i < WallList.Count; i++)
                {
                    WallList[i].SetActive(false);
                }
            }
        }
    }
}