using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Enemy_Spawn : MonoBehaviour
{
    //PlayerSensor継承
    [SerializeField]
    GameObject _PLSenser;
    PlayerSensor PLSenser;

    [SerializeField]
    float SpawnTime = 0.0f;

    [SerializeField]
    int MonsterAmount = 0;

    [SerializeField]
    List<GameObject> EnemyList = new List<GameObject>();

    bool SpawnStart = false;

    [SerializeField]
    bool ALLSpawn;

    [SerializeField]
    GameObject Enemy;

    [SerializeField]
    List<GameObject> SpawnPosList = new List<GameObject>();

    [SerializeField]
    float HP = 100f;

	[SerializeField]
	bool ALL = true;

    //aligment aligment = aligment.enemy;
    AttackArea AttackMagic;
    element Attackele; //攻撃の属性
    public element Weakele; //壁の弱点属性


    // Use this for initialization
    void Start()
    {
        PLSenser = _PLSenser.GetComponent<PlayerSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        //一回だけ
        if (PLSenser.GetPL_Search() && !SpawnStart)
        {
            StartCoroutine(Monstar_Spawn());
            SpawnStart = true;
        }
    }
    //敵が倒されたときにListから消える
    public void EnemyListDest(EnemyController obj) {
        EnemyList.Remove(obj.gameObject);
    }

    //敵がスポーン位置から出現
    IEnumerator Monstar_Spawn()
    {
        //PLSenserにプレイヤーが入ったら
        while (PLSenser.GetPL_Search())
        {
            int a = 0;
            int Rand = Random.Range(0,SpawnPosList.Count);
            //スポーン位置に敵がいたら
            if (SpawnPosList[Rand].GetComponent<EnemySpawnArea>().inArea)
            {
                if (a <= 50)
                {
                    a++;
                    yield return null;
                    continue;
                }
            }

            //敵の数(MonsterAmount)が現在の表示している数より少ない時
            if (EnemyList.Count < MonsterAmount)
            {
                GameObject PopEnemy = Instantiate(Enemy, SpawnPosList[Rand].transform.position, Quaternion.identity);
                EnemyList.Add(PopEnemy);
                PopEnemy.GetComponent<EnemyController>().SetOnDest(EnemyListDest);
                //Debug.Log("a");
            }
            yield return new WaitForSeconds(SpawnTime);
          
        }
        SpawnStart = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //AttackAreaのついている攻撃が来たら属性を調べる
        if (other.gameObject.GetComponent<AttackArea>())
        {
            AttackMagic = other.gameObject.GetComponent<AttackArea>();

            //プレイヤーの攻撃でしか壊せない
            if (AttackMagic.aligment == aligment.player)
            {
				Attackele = AttackMagic.element.type;
                //全ての攻撃で壊れるbool
				if (ALL)
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