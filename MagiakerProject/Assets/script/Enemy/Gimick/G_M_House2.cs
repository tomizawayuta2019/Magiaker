using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_M_House2 : Gimick {
    [SerializeField]
    List<GameObject> WallList = new List<GameObject>();

    [SerializeField]
    List<GameObject> EnemyList = new List<GameObject>();

    [SerializeField]
    private AudioSource SE,spawnSE;
    [SerializeField]
    private GameObject spawnEffect;
    [SerializeField]
    private float effectTime;

    private void Start()
    {
        foreach (GameObject wall in WallList) {
            wall.SetActive(false);
        }

        foreach (GameObject enemy in EnemyList) {
            enemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
        //常にモンスターがリストからいなくなったらリストを削除する
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null)
            {
                EnemyList.Remove(EnemyList[i--]);
            }
        }

        if (EnemyList.Count == 0) {
            EndMonsterHouse();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        bool isStart = false;
        //プレイヤーがモンスターハウス内に入った時
        if (other.gameObject.tag == Tags.Player)
        {
            //壁を作る
            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].SetActive(true);
                isStart = true;
            }
            //enemyが出現
            for (int i = 0; i < EnemyList.Count; i++)
            {
                if (!EnemyList[i].activeSelf) {
                    EnemyList[i].SetActive(true);

                    ////見た目が変になってしまったので無し
                    //Vector3 pos = EnemyList[i].transform.position;
                    ////pos.y -= 0.5f;
                    //GameObject effect = Instantiate(spawnEffect);
                    //effect.transform.position = pos;
                    //Destroy(effect, effectTime);

                    isStart = true;
                }
            }

            if (isStart) {
                SEManager.SetSE(spawnSE);
            }
        }

    }

    private void EndMonsterHouse() {
        for (int i = 0; i < WallList.Count; i++)
        {
            WallList[i].SetActive(false);
        }
        SEManager.SetSE(SE);
        gameObject.SetActive(false);
    }
    
}
